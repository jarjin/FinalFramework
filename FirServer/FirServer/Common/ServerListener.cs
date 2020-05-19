using System.Net;
using System.Net.Sockets;
using FirServer.Defines;
using LiteNetLib;
using log4net;

namespace FirServer
{
    public class ServerListener : BaseBehaviour, INetEventListener
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(ServerListener));

        public void OnPeerConnected(NetPeer peer)
        {
            netMgr.OnConnected(peer);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            netMgr.OnDisconnect(peer);
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            logger.Error("OnNetworkError------>>>" + socketError);
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            netMgr.OnRecvData(peer, reader);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            if (messageType == UnconnectedMessageType.BasicMessage)
            {
                logger.Warn("[SERVER] Received discovery request. Send discovery response");
            }
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            request.AcceptIfKey(AppConst.AppName);
            logger.Info("OnConnectionRequest--->>" + request.RemoteEndPoint);
        }
    }
}
