using FirCommon.Utility;
using FirServer.Define;
using Google.Protobuf;
using Grpc.Core;
using GrpcGateway;
using log4net;
using System.Collections.Concurrent;

namespace FirServer.Manager
{
    public class NetworkManager : BaseManager
    {
        private static readonly ILog logger = LogManager.GetLogger(AppConst.LogRepos?.Name, typeof(NetworkManager));
        private ConcurrentDictionary<string, MsgChannel> channels = new ConcurrentDictionary<string, MsgChannel>();

        public override void Initialize()
        {
        }

        internal string TryAddClient(string userKey, string name, IServerStreamWriter<HelloReply> responseStream)
        {
            channels.TryGetValue(userKey, out MsgChannel? channel);
            if (channel == null)
            {
                var newToken = Guid.NewGuid().ToString();
                channels.TryAdd(userKey, new MsgChannel()
                {
                    Name = name, Token = newToken, Stream = responseStream
                });
                return newToken;
            }
            return string.Empty;
        }

        internal MsgChannel? GetChannel(string token)
        {
            foreach (var channel in channels.Values)
            {
                if (channel.Token == token) return channel;
            }
            return null;
        }

        internal MsgChannel? TryRemoveClient(string name) 
        { 
            channels.TryRemove(name, out MsgChannel? channel);
            return channel;
        }

        public async Task SendMessage(MsgChannel channel, string protoName, IMessage msg)
        {
            var msgPack = msg.Serialize();
            var resMsg = new HelloReply()
            {
                ProtoName = protoName,
                ByteData = msgPack
            };
            if (channel != null && channel.Stream != null)
            {
                try
                {
                    await channel.Stream.WriteAsync(resMsg);
                }
                catch (Exception ex) 
                {
                    if (channel != null && !string.IsNullOrEmpty(channel.Name))
                    {
                        TryRemoveClient(channel.Name);
                    }
                }
            }
        }

        public async Task BroadcastMessage(string protoName, IMessage msg)
        {
            var msgPack = msg.Serialize();
            foreach (var channel in channels.Values)
            {
                if (channel == null || string.IsNullOrEmpty(channel.Name))
                {
                    continue;
                }
                var channelName = channel.Name;
                try
                {
                    var resMsg = new HelloReply()
                    {
                        ProtoName = protoName,
                        ByteData = msgPack
                    };
                    if (channel != null && channel.Stream != null)
                    {
                        await channel.Stream.WriteAsync(resMsg);
                    }
                }
                finally
                {
                    TryRemoveClient(channelName);
                }
            }
        }

        internal async Task OnRecvData(HelloRequest? request)
        {
            if (handlerMgr != null && request != null)
            {
                var channel = GetChannel(request.Token);
                try
                {
                    if (channel != null)
                    {
                        await handlerMgr.OnRecvData(request.ProtoName, channel, request.ByteData);
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(channel));
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }
        }

        public override void OnDispose()
        {
        }
    }
}
