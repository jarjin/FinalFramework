using FirCommon.Utility;
using FirServer.Define;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcGateway;
using log4net;

namespace FirServer.Service
{
    public partial class GatewayService : Gateway.GatewayBase
    {
        private static readonly ILog logger = LogManager.GetLogger(AppConst.LogRepos?.Name, typeof(GatewayService));

        public override async Task ReqConnect(IAsyncStreamReader<ConnectRequest> requestStream, 
            IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var clientIP = context.GetHttpContext().Connection.RemoteIpAddress!.ToString();
            var clientPort = context.GetHttpContext().Connection.RemotePort;
            try
            {
                if (netMgr != null)
                {
                    await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
                    {
                        var userKey = $"{request.Name}@{clientIP}:{clientPort}";
                        var userToken = netMgr.TryAddClient(userKey, request.Name, responseStream);
                        if (!string.IsNullOrEmpty(userToken))
                        {
                            var connectReply = new ConnectReply { Token = userToken };
                            await Task.FromResult<HelloReply>(new HelloReply()
                            {
                                ProtoName = request.Name,
                                ByteData = connectReply.Serialize()
                            });
                            logger.Info(string.Format("userKey:{0} token:(1) connected!!!", userKey, userToken));
                        }
                        else
                        {
                            throw new Exception("Failed to connect the server!!!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to connect {ex.Message}");
            }
            await base.ReqConnect(requestStream, responseStream, context);
        }

        public override async Task ReqGateway(IAsyncStreamReader<HelloRequest> requestStream, 
            IServerStreamWriter<Empty> responseStream, ServerCallContext context)
        {
            if (netMgr != null)
            {
                await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
                {
                    await netMgr.OnRecvData(request);
                }
            }
            await base.ReqGateway(requestStream, responseStream, context);
        }
    }
}