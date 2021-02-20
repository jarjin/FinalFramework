using FirClient.Utility;
using LiteNetLib;
using LiteNetLib.Utils;
using LuaInterface;

namespace FirClient.Manager
{
    internal class LuaMsgDispatcher : BaseDispatcher
    {
        public override void OnMessage(NetPeer peer, NetDataReader reader)
        {
            byte[] bytes = null;
            string protoName = null;
            ParseProtoBytes(reader, ref protoName, ref bytes);
            if (bytes != null)
            {
                var buffer = new LuaByteBuffer(bytes);
                Util.CallLuaMethod("OnReceived", protoName, buffer);
            }
        }
    }
}