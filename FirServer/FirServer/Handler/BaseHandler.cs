using FirServer.Common;
using FirServer.Define;
using FirServer.Interface;
using Google.Protobuf;

namespace FirServer.Handler
{
    public abstract class BaseHandler : BaseBehaviour, IHandler
    {
        public virtual Task OnMessage(MsgChannel channel, ByteString bytes)
        {
            throw new NotImplementedException();
        }

        public async Task SendMessage(MsgChannel channel, string protoName, IMessage bytes)
        {
            if (netMgr != null)
            {
                await netMgr.SendMessage(channel, protoName, bytes);
            }
        }

        public async Task BroadcastMessage(string protoName, IMessage msg)
        {
            if (netMgr != null)
            {
                await netMgr.BroadcastMessage(protoName, msg);
            }
        }
    }
}
