using FirClient.Manager;
using Sfs2X.Core;
using Sfs2X.Util;
using Sfs2X;
using UnityEngine;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

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
            sfs.AddEventListener(SFSEvent.LOGIN, netMgr.OnLogin);
            sfs.AddEventListener(SFSEvent.LOGIN_ERROR, netMgr.OnLoginError);
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

        public void Update()
        {
            sfs?.ProcessEvents();
        }

        public void Disconnect()
        {
            sfs.Disconnect();
        }

        private void OnConnection(BaseEvent evt)
        {
            netMgr.OnConnected(evt);
        }

        public void Send(string protoName, byte[] bytes)
        {
            var param = SFSObject.NewInstance();
            param.PutUtfString(AppConst.ProtoNameKey, protoName);
            param.PutByteArray(AppConst.ByteArrayKey, new ByteArray(bytes));

            sfs.Send(new ExtensionRequest(AppConst.ExtCmdName, param));
        }

        private void OnExtensionResponse(BaseEvent evt)
        {
            // Retrieve response object
            var responseParams = (SFSObject)evt.Params["params"];
            if (responseParams != null)
            {
                netMgr.OnReceived(responseParams);
            }
        }

        private void OnConnectionLost(BaseEvent evt)
        {
            Debug.Log("[CLIENT] We disconnected because " + evt.Type);
        }
    }
}
