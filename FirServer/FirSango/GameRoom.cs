using System;
using System.Collections.Generic;
using FirServer;
using FirServer.Define;
using GameLibs.FirSango.Interface;
using log4net;

namespace GameLibs.FirSango
{
    public class GameRoom : GameBehaviour, IRoom
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(GameRoom));

        private uint roomId = 0;
        private uint maxCount = 0;
        private Dictionary<long, User> users = new Dictionary<long, User>();

        public void Initialize(string name, uint roomId)
        {
            this.roomId = roomId;
            logger.Info("Game Room " + roomId + "("+ name + ")" + " Created!");
        }

        public bool OnEnter(User user)
        {
            if (users.Count >= maxCount)
            {
                return false;
            }
            if (user != null)
            {
                user.paramObject = this;
                users.Add(user.uid, user);
                return true;
            }
            return false;
        }

        public bool OnLeave(User user)
        {
            if (user != null)
            {
                user.paramObject = null;
                return users.Remove(user.uid);
            }
            return false;
        }

        public Dictionary<long, User> GetUsers()
        {
            return users;
        }

        public uint GetRoomId()
        {
            return roomId;
        }

        public bool OnPK(User user, uint targetId)
        {
            throw new NotImplementedException();
        }

        public void OnDispose()
        {
            Console.WriteLine("Game Room " + roomId + " OnDispose...");
        }
    }
}
