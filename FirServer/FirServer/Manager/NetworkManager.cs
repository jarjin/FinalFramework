using FirServer.Define;
using FirServer.Interface;
using LiteNetLib;
using LiteNetLib.Utils;
using log4net;
using ProtoBuf;
using System.IO;

namespace FirServer.Manager
{
    public class NetworkManager : BaseBehaviour, IManager
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(NetworkManager));

        public void Initialize()
        {
            netMgr = this;
        }

        internal void OnConnected(NetPeer peer)
        {
            logger.Info(peer.EndPoint + " OnConnected!!");
        }

        public void SendData<T>(NetPeer peer, ProtoType protoType, string protoName, T t)
        {
            var bytes = Serialize<T>(t);
            if (bytes != null)
            {
                SendDataInternal(peer, protoType, protoName, bytes);
            }
        }

        private void SendDataInternal(NetPeer peer, ProtoType protoType, string protoName, byte[] buffer)
        {
            var writer = new NetDataWriter();
            writer.Put((ushort)protoType);
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

        public byte[] Serialize<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                return ms.ToArray();
            }
        }

        public void OnDispose()
        {
        }
    }
}
