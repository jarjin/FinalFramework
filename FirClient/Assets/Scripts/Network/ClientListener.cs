using FirClient.Manager;
using Sfs2X.Core;
using Sfs2X.Util;
using Sfs2X;
using UnityEngine;
using Network.pb_common;
using Sfs2X.Entities.Data;
using System.IO;
using Google.Protobuf;
using FirCommon.Utility;

namespace FirClient.Network
{
    public class ClientListener : BaseBehaviour
    {
        private SmartFox sfs;
        private NetworkManager netMgr;
        public ClientListener(NetworkManager manager)
        {
            netMgr = manager;
        }

        public void Start()
        {
            // Create SmartFox client instance
            sfs = new SmartFox();

            // Add event listeners
            sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
            sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
            sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
            sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
            sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        }

        public void Connect(string host, int port, string zone)
        {
            // Set connection parameters
            var cfg = new ConfigData();
            cfg.Host = host;
            cfg.Port = port;
            cfg.Zone = zone;

            // Connect to SFS2X
            sfs.Connect(cfg);
        }

        public void Stop()
        {
        }

        public void Update()
        {
            sfs?.ProcessEvents();
        }

        private void OnConnection(BaseEvent evt)
        {
            netMgr.OnConnected(evt);
        }

        private void OnConnectionLost(BaseEvent evt)
        {
            Debug.Log("Disconnected");
        }



        private void OnLogin(BaseEvent evt)
        {
            Debug.Log("Logged in as: " + sfs.MySelf.Name);

            Person john = new Person
            {
                Id = 1234,
                Name = "John Doe",
                Email = "jdoe@example.com",
                Phones = { new Person.Types.PhoneNumber { Number = "555-4321", Type = Person.Types.PhoneType.Home } }
            };

            byte[] bytes = ProtoUtil.SerializeByteArray(john);

            // Send test request to Extension
            var param = SFSObject.NewInstance();
            param.PutUtfString(AppConst.ProtoNameKey, Protocal.ReqLogin);
            param.PutByteArray(AppConst.ByteArrayKey, new ByteArray(bytes));

            sfs.Send(new Sfs2X.Requests.ExtensionRequest(AppConst.ExtCmdName, param));
        }

        private void OnLoginError(BaseEvent evt)
        {
            Debug.LogError("Login error: " + (string)evt.Params["errorMessage"]);
        }

        private void OnExtensionResponse(BaseEvent evt)
        {
            // Retrieve response object
            ISFSObject responseParams = (SFSObject)evt.Params["params"];

            Debug.Log("Result: " + responseParams.GetInt("res"));
        }


        //public void OnPeerConnected(NetPeer peer)
        //{
        //    if (netMgr != null)
        //    {
        //        netMgr.OnConnected(peer);
        //    }
        //    Debug.LogWarning("[CLIENT] OnPeerConnected: " + peer.Id);
        //}

        //public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        //{
        //    if (netMgr != null)
        //    {
        //        netMgr.OnReceived(peer, reader);
        //    }
        //    reader.Recycle();
        //}

        //public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        //{
        //    Debug.LogError("[CLIENT] error! " + socketError);
        //}

        //public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        //{
        //    if (messageType == UnconnectedMessageType.BasicMessage)
        //    {
        //        Debug.Log("[CLIENT] Received discovery response. Connecting to: " + remoteEndPoint);
        //        if (messageType == UnconnectedMessageType.BasicMessage && netMgr.mClient.ConnectedPeersCount == 0 && reader.GetInt() == 1)
        //        {
        //            netMgr.mClient.Connect(remoteEndPoint, AppConst.AppName);
        //        }
        //    }
        //}

        //public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        //{
        //}

        //public void OnConnectionRequest(ConnectionRequest request)
        //{
        //}

        //public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        //{
        //    if (netMgr != null)
        //    {
        //        netMgr.OnDisconnected(peer, disconnectInfo.Reason.ToString());
        //    }
        //    Debug.Log("[CLIENT] We disconnected because " + disconnectInfo.Reason);
        //}
    }

}
