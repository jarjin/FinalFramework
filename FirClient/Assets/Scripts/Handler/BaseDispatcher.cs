using LiteNetLib;
using LiteNetLib.Utils;

namespace FirClient.Manager
{
    public abstract class BaseDispatcher : BaseBehaviour
    {
        public abstract void OnMessage(NetPeer peer, NetDataReader reader);

        protected void ParseProtoBytes(NetDataReader reader, ref string name, ref byte[] bytes)
        {
            var protoName = reader.GetString();
            var count = reader.GetInt();
            reader.GetBytes(bytes, count);
        }
    }
}