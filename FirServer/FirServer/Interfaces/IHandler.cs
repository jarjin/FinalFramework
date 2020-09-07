using LiteNetLib;

namespace FirServer.Interface
{
    public interface IHandler : IObject
    {
        void OnMessage(NetPeer peer, byte[] bytes);
    }
}
