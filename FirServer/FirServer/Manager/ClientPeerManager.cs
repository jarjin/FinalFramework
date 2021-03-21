using FirCommon.Data;
using LiteNetLib;
using System.Collections.Generic;

namespace FirServer.Manager
{
    public class ClientPeerManager : BaseManager
    {
        Dictionary<NetPeer, ClientPeer> mClientPeers = new Dictionary<NetPeer, ClientPeer>();

        public override void Initialize()
        {
        }

        public ClientPeer GetClientPeer(NetPeer peer)
        {
            mClientPeers.TryGetValue(peer, out ClientPeer clientPeer);
            if (clientPeer == null)
            {
                clientPeer = new ClientPeer(peer);
                mClientPeers.Add(peer, clientPeer);
            }
            return clientPeer;
        }

        public void RemoveClientPeer(NetPeer peer)
        {
            mClientPeers.Remove(peer);
        }

        public void RemoveClientPeer(ClientPeer clientPeer)
        {
            if (clientPeer != null)
            {
                RemoveClientPeer(clientPeer.peer);
            }
        }
    }
}
