using FirServer;
using FirServer.Define;
using FirServer.Handler;
using GameLibs.FirSango.Defines;
using GameLibs.FirSango.Model;
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
                var doc = userModel.GetDoc<UserInfo>(u => u.uid == uid);
                if (doc != null)
                {
                    dw.Put((ushort)ResultCode.Success);
                    dw.Put(doc.username);
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
