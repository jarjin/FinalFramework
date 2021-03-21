using System.Net;
using System.Net.Sockets;
using FirClient.Manager;
using LiteNetLib;
using UnityEngine;

namespace FirClient.Network
{
    public class ClientListener : INetEventListener
    {
        private NetworkManager netMgr;
        public ClientListener(NetworkManager manager)
        {
            netMgr = manager;
        }

        public void OnPeerConnected(NetPeer peer)
        {
            if (netMgr != null)
            {
                netMgr.OnConnected(peer);
            }
            Debug.LogWarning("[CLIENT] OnPeerConnected: " + peer.Id);
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            if (netMgr != null)
            {
                netMgr.OnReceived(peer, reader);
            }
            reader.Recycle();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Debug.LogError("[CLIENT] error! " + socketError);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            if (messageType == UnconnectedMessageType.BasicMessage)
            {
                Debug.Log("[CLIENT] Received discovery response. Connecting to: " + remoteEndPoint);
                if (messageType == UnconnectedMessageType.BasicMessage && netMgr.mClient.ConnectedPeersCount == 0 && reader.GetInt() == 1)
                {
                    netMgr.mClient.Connect(remoteEndPoint, AppConst.AppName);
                }
            }
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            if (netMgr != null)
            {
                netMgr.OnDisconnected(peer, disconnectInfo.Reason.ToString());
            }
            Debug.Log("[CLIENT] We disconnected because " + disconnectInfo.Reason);
        }
    }

}
