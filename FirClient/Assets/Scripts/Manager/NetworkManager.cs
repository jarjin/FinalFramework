using UnityEngine;
using System.Collections.Generic;
using FirClient.Network;
using LuaInterface;
using Google.Protobuf;
using FirClient.Define;
using FirClient.Extensions;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using FirCommon.Utility;
using Network.pb_common;

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
        private ClientListener mClient;
        private ConnectParam connParams;

        [NoToLua]
        public override void Initialize()
        {
            InitHandler();

            mClient = new ClientListener(this);
            mClient.Start();

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
                mClient.Update();
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
        public void OnConnected(BaseEvent evt)
        {
            bool isConnected = (bool)evt.Params["success"];
            if (connParams.connFunc != null)
            {
                var self = connParams.luaClass;
                connParams.connFunc.Call(self, isConnected);

                //connParams.luaClass.Dispose();
                //connParams.luaClass = null;

                connParams.connFunc.Dispose();
                connParams.connFunc = null;
            }
        }

        [NoToLua]
        public void OnLogin(BaseEvent evt)
        {
            // Send login request
            // sfs.Send(new Sfs2X.Requests.LoginRequest(""));
            var john = new Person
            {
                Id = 1234,
                Name = "John Doe",
                Email = "jdoe@example.com",
                Phones = { new Person.Types.PhoneNumber { Number = "555-4321", Type = Person.Types.PhoneType.Home } }
            };
            SendData(Protocal.ReqLogin, john);
        }

        [NoToLua]
        public void OnLoginError(BaseEvent evt)
        {
            Debug.LogError("[CLIENT]Login error: " + (string)evt.Params["errorMessage"]);
        }

        [NoToLua]
        public void SendData(string protoName, byte[] bytes) 
        {
            SendDataInternal(protoName, bytes);
        }

        public void SendData(string protoName, IMessage msg)
        {
            var buffer = msg.SerializeByteArray();
            if (buffer != null)
            {
                SendDataInternal(protoName, buffer);
            }
        }

        public void SendData(string protoName, LuaByteBuffer luaBuffer) 
        {
            if (luaBuffer.buffer != null)
            {
                SendDataInternal(protoName, luaBuffer.buffer);
            }
        }

        private void SendDataInternal(string protoName, byte[] buffer)
        {
            if (mClient != null)
            {
                mClient.Send(protoName, buffer);
            }
        }

        [NoToLua]
        public void OnReceived(ISFSObject responseParams)
        {
            var key = responseParams.GetByte(AppConst.MsgTypeKey);
            if (mDispatchers.TryGetValue(key, out BaseDispatcher dispatcher))
            {
                if (dispatcher != null)
                {
                    var protoName = responseParams.GetUtfString(AppConst.ProtoNameKey);
                    var byteArray = responseParams.GetByteArray(AppConst.ByteArrayKey);

                    if (!string.IsNullOrEmpty(protoName) && byteArray != null)
                    {
                        dispatcher.OnMessage(protoName, byteArray.Bytes);
                    }
                }
            }
            //Debug.Log("Result: " + responseParams.GetInt("res"));
        }

        [NoToLua]
        public void OnDisconnected(string disReason)
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
                mClient.Disconnect();
                mClient = null;
            }
            Debug.Log("~NetworkManager was destroy");
        }
    }
}