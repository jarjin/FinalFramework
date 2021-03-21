using FirCommon.Data;
using FirServer.Interface;

namespace FirServer.Handler
{
    public abstract class BaseHandler : BaseBehaviour, IHandler
    {
        public abstract void OnMessage(ClientPeer peer, byte[] bytes);
    }
}
