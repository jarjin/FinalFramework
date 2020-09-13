using System.Collections.Generic;
using FirServer;
using FirServer.Define;
using FirServer.Handler;
using FirServer.Model;
using LiteNetLib;
using LiteNetLib.Utils;
using log4net;

namespace GameLibs.FirSango.Handlers
{
    class ReqUserInfoHandler : BaseHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(ReqUserInfoHandler));
        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            var uid = 1u;     //uid
            var dw = new NetDataWriter();
            var userModel = modelMgr.GetModel(ModelNames.User) as UserModel;
            if (userModel != null)
            {
                var list = new List<string>();
                list.Add("long:uid=" + uid);
                var row = userModel.GetRow(list);
                if (row != null)
                {
                    dw.Put((ushort)ResultCode.Success);
                    dw.Put(row["username"].ToString());
                }
                else 
                {
                    dw.Put((ushort)ResultCode.Failed);
                }
            }
            else
            {
                dw.Put((ushort)ResultCode.Failed);
            }
            peer.Send(dw, DeliveryMethod.ReliableOrdered);
            logger.Info("OnMessage: " + uid);
        }
    }
}
