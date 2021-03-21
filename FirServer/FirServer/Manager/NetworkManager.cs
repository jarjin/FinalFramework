using FirCommon.Data;
using FirCommon.Define;
using FirCommon.Utility;
using Google.Protobuf;
using LiteNetLib;
using LiteNetLib.Utils;
using log4net;

namespace FirServer.Manager
{
    public class NetworkManager : BaseManager
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(NetworkManager));

        public override void Initialize()
        {
        }

        internal void OnConnected(NetPeer peer)
        {
            var handler = handlerMgr.GetHandler(Protocal.Connected);
            if (handler != null)
            {
                var clientPeer = clientPeerMgr.AddClientPeer(peer);
                handler.OnMessage(clientPeer, null);
            }
        }

        public void SendData(ClientPeer clientPeer, ProtoType protoType, string protoName, IMessage msg)
        {
            var buffer = msg.Serialize();
            var writer = new NetDataWriter();
            writer.Put((byte)protoType);
            writer.Put(protoName);
            writer.Put(buffer.Length);
            writer.Put(buffer);
            clientPeer.peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        public void OnRecvData(NetPeer peer, NetPacketReader reader)
        {
            handlerMgr.OnRecvData(peer, reader); 
        }

        public void OnDisconnect(NetPeer peer)
        {
            var handler = handlerMgr.GetHandler(Protocal.Disconnect);
            if (handler != null)
            {
                var clientPeer = clientPeerMgr.GetClientPeer(peer);
                handler.OnMessage(clientPeer, null);
                clientPeerMgr.RemoveClientPeer(peer);
            }
            logger.Error("ConnectId:>" + peer.Id + " Disconnected!");
        }
    }
}
