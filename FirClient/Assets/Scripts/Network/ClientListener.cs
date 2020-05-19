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

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Debug.LogError("[CLIENT] disconnected: " + disconnectInfo.Reason);
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            if (netMgr != null)
            {
                netMgr.OnReceived(peer, reader);
            }
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
                if (netMgr.mClient != null && netMgr.mClient.PeersCount == 0)
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
            throw new System.NotImplementedException();
        }
    }

}
