using FirClient.Utility;
using LuaInterface;

namespace FirClient.Manager
{
    internal class LuaMsgDispatcher : BaseDispatcher
    {
        public override void OnMessage(string protoName, byte[] bytes)
        {
            if (bytes != null)
            {
                var buffer = new LuaByteBuffer(bytes);
                Util.CallLuaMethod("OnReceived", protoName, buffer);
            }
        }
    }
}