using FirCommon.Data;

namespace FirServer.Interface
{
    public interface IHandler : IObject
    {
        void OnMessage(ClientPeer peer, byte[] bytes);
    }
}
