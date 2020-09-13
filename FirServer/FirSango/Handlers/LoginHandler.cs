using System.Collections.Generic;
using FirSanguo;
using log4net;
using FirServer.Define;
using FirServer.Model;
using LiteNetLib;
using LiteNetLib.Utils;
using FirServer.Handler;
using FirServer;
using GameLibs.FirSango.Defines;

namespace GameLibs.FirSango.Handlers
{
    public class LoginHandler : BaseHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(LoginHandler));

        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            ///解析使用
            var proto = DeSerialize<AcountLoginAsk>(bytes);
            logger.Info(proto.username);
            logger.Info(proto.password);

            ///封装发送
            var writer = new NetDataWriter();
            var reply = new AccountLoginReply();
            netMgr.SendData<AccountLoginReply>(peer, ProtoType.CSProtoMsg, "AccountLoginReply", reply);

            var username = string.Empty;
            var password = string.Empty;

            var uid = 0L;
            var dw = new NetDataWriter();
            dw.Put(GameProtocal.Login);

            var userModel = modelMgr.GetModel(ModelNames.User) as UserModel;
            if (userModel != null)
            {
                var list = new List<string>();
                list.Add("str:username=" + username);
                list.Add("str:password=" + password);
                uid = userModel.ExistUser(list);
            }
            var result = uid == 0 ? (ushort)ResultCode.Failed : (ushort)ResultCode.Success;
            dw.Put(result);
            if (uid > 0L) 
            {
                dw.Put(uid);
            }
            peer.Send(dw, DeliveryMethod.ReliableOrdered);
            logger.Info(username + " " + password);
        }

        private void InitUser(long uid, NetPeer peer, UserModel model)
        {
            // if (socket == null)
            // {
            //     throw new Exception("InitUser null!~");
            // }
            // var socketid = appServer.connManager.GetId(socket);
            // if (socketid > 0)
            // {
            //     var user = UserMgr.AddUser(socketid, socket);
            //     user.uid = uid; 
            //     user.name = model.GetUserName(uid.ToString());
            // }
        }
    }
}
