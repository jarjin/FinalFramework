using UnityEngine;
using System;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;
using FirClient.Network;
using FirClient.Define;
using LuaInterface;
using System.IO;
using ProtoBuf;

namespace FirClient.Manager
{
    public partial class NetworkManager : BaseManager
    {
        private static readonly object msgLock = new object();
        private static readonly NetDataWriter writer = new NetDataWriter();
        private static readonly Queue<PacketData> mPacketPool = new Queue<PacketData>();
        private static readonly Dictionary<byte, BaseDispatcher> mDispatchers = new Dictionary<byte, BaseDispatcher>();

        public NetManager mClient { get; private set; }
        private Action connectOK;

        [NoToLua]
        public override void Initialize()
        {
            InitHandler();
            
            var listener = new ClientListener(this);
            mClient = new NetManager(listener);
            //client.SimulateLatency = true;
            mClient.UpdateTime = 15;
            if (!mClient.Start())
            {
                Console.WriteLine("Client start failed");
            }
            isOnUpdate = true;
        }

        /// <summary>
        /// ע����Ϣ������
        /// </summary>
        void InitHandler()
        {
            mDispatchers.Add((byte)ProtoType.CSProtoMsg, new CSMsgDispatcher());
            mDispatchers.Add((byte)ProtoType.LuaProtoMsg, new LuaMsgDispatcher());
        }


        public override void OnUpdate(float deltaTime)
        {
            if (mClient != null)
            {
                this.OnProcessPack();
                this.OnSocketUpdate();
            }
        }

        /// <summary>
        /// Socket����
        /// </summary>
        private void OnSocketUpdate()
        {
            mClient.PollEvents();
        }
        
        /// <summary>
        /// ��������
        /// </summary>
        private void OnProcessPack()
        {
            var peer = mClient.FirstPeer;
            if (peer != null && peer.ConnectionState == ConnectionState.Connected)
            {
                lock (msgLock)
                {
                    while (mPacketPool.Count > 0)
                    {
                        SendPacketData(mPacketPool.Dequeue());
                    }
                }
            }
        }

        /// <summary>
        /// Connect to the server.
        /// </summary>
        [NoToLua]
        public void Connect(string addr, int port, Action connectOK)
        {
            this.connectOK = connectOK;
            mClient.Connect(addr, port, AppConst.AppName);
            Debug.LogWarning("Connect Server Address:" + addr + " Port:" + port);
        }

        [NoToLua]
        public void OnConnected(NetPeer peer)
        {
            if (connectOK != null)
            {
                connectOK.Invoke();
            }
            Debug.LogWarning("Server Connected!!");
        }

        public void SendData<T>(string protoName, T t) 
        {
            var bytes = Serialize<T>(t);
            if (bytes != null)
            {
                SendDataInternal(ProtoType.CSProtoMsg, protoName, bytes);
            }
        }

        public void SendData(string protoName, LuaByteBuffer luaBuffer) 
        {
            if (luaBuffer.buffer != null)
            {
                SendDataInternal(ProtoType.LuaProtoMsg, protoName, luaBuffer.buffer);
            }
        }

        private void SendDataInternal(ProtoType protocal, string protoName, byte[] buffer)
        {
            if (mClient != null)
            {
                lock (msgLock)
                {
                    var writer = new NetDataWriter();
                    writer.Put(protoName);
                    writer.Put(buffer.Length);
                    writer.Put(buffer);

                    var packet = objMgr.Get<PacketData>();
                    packet.protocal = protocal;
                    packet.writer = writer;
                    mPacketPool.Enqueue(packet);
                }
            }
        }

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        [NoToLua]
        public void OnReceived(NetPeer peer, NetDataReader reader)
        {
            BaseDispatcher dispatcher = null;
            var key = reader.GetByte();
            if (mDispatchers.TryGetValue(key, out dispatcher))
            {
                if (dispatcher != null)
                {
                    dispatcher.OnMessage(peer, reader);
                }
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void SendPacketData(PacketData pack)
        {
            if (pack != null)
            {
                writer.Reset();
                writer.Put((ushort)pack.protocal);
                if (pack.writer != null)
                {
                    writer.Put(pack.writer.Data);
                }
                var peer = mClient.FirstPeer;
                peer.Send(writer, DeliveryMethod.ReliableOrdered);
                objMgr.Release<PacketData>(pack);
            }
        }

        private byte[] Serialize<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                return ms.ToArray();
            }
        }

        [NoToLua]
        public override void OnDispose()
        {
            if (mClient != null)
            {
                mClient.Stop();
                mClient = null;
            }
            Debug.Log("~NetworkManager was destroy");
        }
    }
}