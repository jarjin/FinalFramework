using FirServer;
using FirServer.Handler;
using LiteNetLib;
using log4net;

namespace GameLibs.FirSango.Handlers
{

    public class DisconnectHandler : BaseHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(DisconnectHandler));

        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            logger.Warn("connid:>" + peer.Id);
        }
    }
}
