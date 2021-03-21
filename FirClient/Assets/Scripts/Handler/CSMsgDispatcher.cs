using FirClient.Handler;
using FirCommon.Define;
using System.Collections.Generic;

namespace FirClient.Manager
{
    internal class CSMsgDispatcher : BaseDispatcher
    {
        Dictionary<string, BaseHandler> mHandlers = new Dictionary<string, BaseHandler>()
        {
            { Protocal.Default, new DefaultHandler() },
        };

        public override void OnMessage(string protoName, byte[] bytes)
        {
            if (mHandlers.TryGetValue(protoName, out BaseHandler handler))
            {
                if (handler != null)
                {
                    handler.OnMessage(bytes);
                }
            }
        }
    }
}