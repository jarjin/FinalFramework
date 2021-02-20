using LiteNetLib;
using LiteNetLib.Utils;

namespace FirClient.Manager
{
    public abstract class BaseDispatcher : BaseBehaviour
    {
        public abstract void OnMessage(NetPeer peer, NetDataReader reader);

        protected void ParseProtoBytes(NetDataReader reader, ref string protoName, ref byte[] bytes)
        {
            protoName = reader.GetString();
            var count = reader.GetInt();
            bytes = new byte[count];
            reader.GetBytes(bytes, count);
        }
    }
}