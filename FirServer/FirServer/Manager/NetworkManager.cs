using FirCommon.Define;
using FirCommon.Utility;
using FirServer.Define;
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
                handler.OnMessage(peer, null);
            }
        }

        public void SendData(NetPeer peer, ProtoType protoType, string protoName, IMessage msg)
        {
            var buffer = msg.Serialize();
            SendDataInternal(peer, protoType, protoName, buffer);
        }

        private void SendDataInternal(NetPeer peer, ProtoType protoType, string protoName, byte[] buffer)
        {
            var writer = new NetDataWriter();
            writer.Put((byte)protoType);
            writer.Put(protoName);
            writer.Put(buffer.Length);
            writer.Put(buffer);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
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
                handler.OnMessage(peer, null);
            }
            logger.Error("ConnectId:>" + peer.Id + " Disconnected!");
        }
    }
}
