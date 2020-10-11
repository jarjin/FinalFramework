using log4net;
using FirServer.Define;
using LiteNetLib;
using LiteNetLib.Utils;
using FirServer.Handler;
using FirServer;
using GameLibs.FirSango.Defines;
using GameLibs.FirSango.Model;
using Tutorial;
using System.IO;
using Google.Protobuf;

namespace GameLibs.FirSango.Handlers
{
    public class LoginHandler : BaseHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(LoginHandler));

        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            ///解析使用
            var person = Person.Parser.ParseFrom(bytes);
            logger.Info(person.Name);
            logger.Info(person.Email);

            ///封装发送
            using (MemoryStream stream = new MemoryStream())
            {
                // Save the person to a stream
                person.WriteTo(stream);
                bytes = stream.ToArray();
                netMgr.SendData(peer, ProtoType.CSProtoMsg, "Person", bytes);
            }

            var username = string.Empty;
            var password = string.Empty;

            var uid = 0L;
            var dw = new NetDataWriter();
            dw.Put(GameProtocal.Login);

            var userModel = modelMgr.GetModel(ModelNames.User) as UserModel;
            if (userModel != null)
            {
                uid = userModel.ExistUser(username, password);
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
