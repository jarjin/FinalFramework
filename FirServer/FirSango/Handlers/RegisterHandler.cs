using log4net;
using FirServer.Define;
using LiteNetLib;
using FirServer.Handler;
using FirServer;
using GameLibs.FirSango.Defines;
using GameLibs.FirSango.Model;
using System;
using PbUser;

namespace GameLibs.FirSango.Handlers
{
    public class RegisterHandler : BaseHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(RegisterHandler));

        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            var person = Req_Register.Parser.ParseFrom(bytes);

            var user = new UserInfo()
            {
                username = person.Name,
                password = person.Pass,
                money = 10000L,
                lasttime = DateTime.Now.ToShortDateString()
            };
            long uid = 0L;
            var userModel = modelMgr.GetModel(ModelNames.User) as UserModel;
            if (userModel != null)
            {
                uid = userModel.AddUser(user);
            }
            var resData = new Res_Register();
            if (uid > 0)
            {
                resData.Result = PbCommon.ResultCode.Success;
                resData.Userid = uid.ToString();
            }
            netMgr.SendData(peer, ProtoType.LuaProtoMsg, GameProtocal.Register, resData);

            logger.Info("OnMessage: " + uid);
        }
    }
}
