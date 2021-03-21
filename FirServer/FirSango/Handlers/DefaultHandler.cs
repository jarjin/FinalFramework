using FirCommon.Data;
using FirServer;
using FirServer.Handler;
using log4net;
using System;

namespace GameLibs.FirSango.Handlers
{
    public class DefaultHandler : BaseHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(DefaultHandler));

        public override void OnMessage(ClientPeer peer, byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
