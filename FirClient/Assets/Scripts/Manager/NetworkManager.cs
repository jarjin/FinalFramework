using UnityEngine;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;
using FirClient.Network;
using LuaInterface;
using Google.Protobuf;
using FirClient.Define;
using FirClient.Extensions;

namespace FirClient.Manager
{
    struct ConnectParam
    {
        public LuaTable luaClass;
        public LuaFunction callFunc;
    }

    public partial class NetworkManager : BaseManager
    {
        static readonly Dictionary<byte, BaseDispatcher> mDispatchers = new Dictionary<byte, BaseDispatcher>();

        public NetManager mClient { get; private set; }
        private ConnectParam connParams;

        [NoToLua]
        public override void Initialize()
        {
            InitHandler();
            
            var listener = new ClientListener(this);
            mClient = new NetManager(listener);
            mClient.UnconnectedMessagesEnabled = true;
            mClient.UpdateTime = 15;
            mClient.DisconnectTimeout = 30 * 1000;
            if (!mClient.Start())
            {
                Debug.LogError("Client start failed");
                return;
            }
            isOnUpdate = true;
        }

        void InitHandler()
        {
            mDispatchers.Add((byte)ProtoType.CSProtoMsg, new CSMsgDispatcher());
            mDispatchers.Add((byte)ProtoType.LuaProtoMsg, new LuaMsgDispatcher());
        }


        public override void OnUpdate(float deltaTime)
        {
            if (mClient != null)
            {
                mClient.PollEvents();
            }
        }

        public void Connect(string addr, int port, LuaTable luaClass, LuaFunction connectOK)
        {
            connParams.luaClass = luaClass;
            connParams.callFunc = connectOK;

            mClient.Connect(addr, port, AppConst.AppName);
            Debug.LogWarning("Connect Server Address:" + addr + " Port:" + port);
        }

        [NoToLua]
        public void OnConnected(NetPeer peer, string disReason = null)
        {
            if (connParams.callFunc != null)
            {
                var self = connParams.luaClass;
                connParams.callFunc.Call(self, disReason);

                connParams.luaClass.Dispose();
                connParams.luaClass = null;

                connParams.callFunc.Dispose();
                connParams.callFunc = null;
            }
        }

        [NoToLua]
        public void SendData(string protoName, byte[] bytes) 
        {
            SendDataInternal(ProtoType.CSProtoMsg, protoName, bytes);
        }

        public void SendData(string protoName, IMessage msg)
        {
            var buffer = msg.Serialize();
            if (buffer != null)
            {
                SendDataInternal(ProtoType.CSProtoMsg, protoName, buffer);
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
                var writer = new NetDataWriter();
                writer.Put((byte)protocal);
                writer.Put(protoName);
                writer.Put(buffer.Length);
                writer.Put(buffer);
                mClient.FirstPeer.Send(writer, DeliveryMethod.ReliableOrdered);
            }
        }

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