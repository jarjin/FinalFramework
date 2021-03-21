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
        public LuaFunction connFunc;
        public LuaFunction disconnFunc;
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

        public void Connect(string addr, int port, LuaTable luaClass, LuaFunction connectOK, LuaFunction disconnFunc)
        {
            connParams.luaClass = luaClass;
            connParams.connFunc = connectOK;
            connParams.disconnFunc = disconnFunc;

            mClient.Connect(addr, port, AppConst.AppName);
            Debug.LogWarning("Connect Server Address:" + addr + " Port:" + port);
        }

        [NoToLua]
        public void OnConnected(NetPeer peer, string disReason = null)
        {
            if (connParams.connFunc != null)
            {
                var self = connParams.luaClass;
                connParams.connFunc.Call(self, disReason);

                //connParams.luaClass.Dispose();
                //connParams.luaClass = null;

                connParams.connFunc.Dispose();
                connParams.connFunc = null;
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
            var key = reader.GetByte();
            if (mDispatchers.TryGetValue(key, out BaseDispatcher dispatcher))
            {
                if (dispatcher != null)
                {
                    var protoName = reader.GetString();
                    var count = reader.GetInt();
                    var bytes = new byte[count];
                    reader.GetBytes(bytes, count);
                    dispatcher.OnMessage(protoName, bytes);
                }
            }
        }

        [NoToLua]
        public void OnDisconnected(NetPeer peer, string disReason)
        {
            if (connParams.connFunc != null)
            {
                var self = connParams.luaClass;
                connParams.disconnFunc.Call(self, disReason);

                connParams.luaClass.Dispose();
                connParams.luaClass = null;

                connParams.disconnFunc.Dispose();
                connParams.disconnFunc = null;
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