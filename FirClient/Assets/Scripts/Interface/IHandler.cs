public interface IHandler
{
    void OnMessage(NetPeer peer, NetDataReader reader);
}