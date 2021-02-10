using log4net;
using FirServer.Define;
using LiteNetLib;
using FirServer.Handler;
using FirServer;
using GameLibs.FirSango.Defines;
using GameLibs.FirSango.Model;
using PbUser;

namespace GameLibs.FirSango.Handlers
{
    public class LoginHandler : BaseHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(LoginHandler));

        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            var person = ReqLogin.Parser.ParseFrom(bytes);

            var resData = new ResLogin();
            resData.Result = PbCommon.ResultCode.Failed;
            var userModel = modelMgr.GetModel(ModelNames.User) as UserModel;
            if (userModel != null)
            {
                var uid = 10000L;
                //var uid = userModel.ExistUser(person.Name, person.Pass);
                resData.Result = PbCommon.ResultCode.Success;
                resData.Userinfo = new PbCommon.UserInfo()
                {
                    Name = person.Name,
                    Money = 10000,
                    Userid = uid.ToString(),
                };
            }
            netMgr.SendData(peer, ProtoType.LuaProtoMsg, GameProtocal.Login, resData);

            logger.Info(person.Name + " " + person.Pass);
        }
    }
}
