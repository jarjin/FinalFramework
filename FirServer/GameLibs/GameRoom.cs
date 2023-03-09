using FirServer.Define;
using GameLibs.Interface;
using log4net;

namespace GameLibs
{
    public class GameRoom : GameBehaviour, IRoom
    {
        private static readonly ILog logger = LogManager.GetLogger(AppConst.LogRepos?.Name, typeof(GameRoom));

        private uint roomId = 0;
        private uint maxCount = 0;
        private Dictionary<string, MsgChannel> users = new Dictionary<string, MsgChannel>();

        public void Initialize(string name, uint roomId)
        {
            this.roomId = roomId;
            logger.Info("Game Room " + roomId + "("+ name + ")" + " Created!");
        }

        public bool OnEnter(MsgChannel user)
        {
            if (users.Count >= maxCount)
            {
                return false;
            }
            if (user != null && user.Name != null)
            {
                user.paramObject = this;
                users.Add(user.Name, user);
                return true;
            }
            return false;
        }

        public bool OnLeave(MsgChannel user)
        {
            if (user != null && user.Name != null)
            {
                user.paramObject = null;
                return users.Remove(user.Name);
            }
            return false;
        }

        public Dictionary<string, MsgChannel> GetUsers()
        {
            return users;
        }

        public uint GetRoomId()
        {
            return roomId;
        }

        public bool OnPK(MsgChannel user, uint targetId)
        {
            throw new NotImplementedException();
        }

        public void OnDispose()
        {
            Console.WriteLine("Game Room " + roomId + " OnDispose...");
        }
    }
}
