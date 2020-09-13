using LiteNetLib;

namespace FirServer.Handler
{
    class DefaultHandler : BaseHandler
    {
        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            //netMgr.DeSerialize<T>(bytes);
        }
    }
}
