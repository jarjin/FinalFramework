using System.Net;
using System.Net.Sockets;
using FirServer.Define;
using LiteNetLib;
using log4net;

namespace FirServer
{
    public class ServerListener : BaseBehaviour, INetEventListener, INetLogger
    {
        private NetManager mServer = null;
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(ServerListener));

        public void StartServer(int port)
        {
            mServer = new NetManager(this);
            mServer.Start(port);
            mServer.UpdateTime = 15;
        }

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

        internal void OnUpdate()
        {
            if (mServer != null)
            {
                mServer.PollEvents();
            }
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            if (mServer.GetPeersCount(ConnectionState.Connected) < 10)
            {
                request.AcceptIfKey(AppConst.AppName);
            }
            else
            {
                request.Reject();       //拒绝掉链接
            }
            logger.Info("OnConnectionRequest--->>" + request.RemoteEndPoint);
        }

        public void WriteNet(NetLogLevel level, string str, params object[] args)
        {
            logger.InfoFormat(str, args);
        }

        internal void StopServer()
        {
            if (mServer != null)
            {
                mServer.Stop();
                mServer = null;
            }
        }
    }
}
