using LiteNetLib;

namespace FirServer.Handlers
{
    class DefaultHandler : BaseHandler
    {
        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            //netMgr.DeSerialize<T>(bytes);
        }
    }
}
