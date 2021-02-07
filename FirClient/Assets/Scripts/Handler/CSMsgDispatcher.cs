using FirClient.Define;
using FirClient.Handler;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;

namespace FirClient.Manager
{
    internal class CSMsgDispatcher : BaseDispatcher
    {
        Dictionary<string, BaseHandler> mHandlers = new Dictionary<string, BaseHandler>()
        {
            { Protocal.Default, new DefaultHandler() },
        };

        public override void OnMessage(NetPeer peer, NetDataReader reader)
        {
            string protoName = string.Empty;
            byte[] bytes = null;
            ParseProtoBytes(reader, ref protoName, ref bytes);

            BaseHandler handler = null;
            if (mHandlers.TryGetValue(protoName, out handler))
            {
                if (handler != null)
                {
                    handler.OnMessage(bytes);
                }
            }
        }
    }
}