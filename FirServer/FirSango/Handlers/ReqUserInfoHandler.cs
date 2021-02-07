using FirServer;
using FirServer.Define;
using FirServer.Handler;
using GameLibs.FirSango.Defines;
using GameLibs.FirSango.Model;
using LiteNetLib;
using log4net;
using PbUser;

namespace GameLibs.FirSango.Handlers
{
    class ReqUserInfoHandler : BaseHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(ReqUserInfoHandler));
        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            var data = Req_UserInfo.Parser.ParseFrom(bytes);
            var resData = new Res_UserInfo();
            resData.Result = PbCommon.ResultCode.Failed;

            var userModel = modelMgr.GetModel(ModelNames.User) as UserModel;
            if (userModel != null)
            {
                var uid = long.Parse(data.Userid);
                var doc = userModel.GetDoc<UserInfo>(u => u.uid == uid);
                if (doc != null)
                {
                    resData.Result = PbCommon.ResultCode.Success;
                    resData.UserInfo = new PbCommon.UserInfo();
                    resData.UserInfo.Userid = data.Userid;
                    resData.UserInfo.Name = doc.username;
                    resData.UserInfo.Money = doc.money;
                }
            }
            netMgr.SendData(peer, ProtoType.LuaProtoMsg, GameProtocal.UserInfo, resData);
            logger.Info("OnMessage: " + data.Userid);
        }
    }
}
