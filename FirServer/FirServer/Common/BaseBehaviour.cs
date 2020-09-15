using System.Collections.Generic;
using System.Net.WebSockets;
using FirServer.Define;
using FirServer.Manager;
using LitJson;

namespace FirServer
{
    public class BaseBehaviour
    {
        protected static AppServer appServer { get; set; }
        protected static DataManager dataMgr { get; set; }
        protected static TimerManager timerMgr { get; set; }
        protected static ModelManager modelMgr { get; set; }
        protected static ConfigManager configMgr { get; set; }
        protected static UserManager userMgr { get; set; }
        protected static AssemblyManager assemblyMgr { get; set; }
        protected static NetworkManager netMgr { get; set; }
        protected static HandlerManager handlerMgr { get; set; }

        ///
        protected User GetUserBySocket(WebSocket socket)
        {
            return null;
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public void BroadcastMessage(List<User> users, ushort protocal, JsonData json)
        {
            var message = new Message()
            {
                CommandId = protocal,
                MessageType = MessageType.Json,
                Data = JsonMapper.ToJson(json)
            };
            foreach (var user in users)
            {
                //await appServer.SendMessageAsync(user.socket, message);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public void SendJsonMessage(WebSocket socket, ushort protocal, JsonData json)
        {
            var message = new Message()
            {
                CommandId = protocal,
                MessageType = MessageType.Json,
                Data = JsonMapper.ToJson(json)
            };
            //await appServer.SendMessageAsync(socket, message);
        }
    }
}
