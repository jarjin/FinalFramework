using FirServer;
using FirServer.Handler;
using LiteNetLib;
using log4net;

namespace GameLibs.FirSango.Handlers
{
    public class ConnectedHandler : BaseHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(ConnectedHandler));

        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            logger.Info(peer.EndPoint + " OnConnected!!");
        }
    }
}
