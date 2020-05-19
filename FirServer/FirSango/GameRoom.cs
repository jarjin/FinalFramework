using System;
using System.Collections.Generic;
using FirServer.Defines;
using GameLibs.FirSango.Interface;

namespace GameLibs.FirSango
{
    public class GameRoom : GameBehaviour, IRoom
    {
        private uint roomId = 0;
        private uint maxCount = 0;
        private bool isPlaying = false;
        private Dictionary<long, User> users = new Dictionary<long, User>();

        public void Initialize(string name, uint roomId)
        {
            this.roomId = roomId;
            Console.WriteLine(name + " Game Room " + roomId + " Created!");
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

        public bool OnGenZhu(User user, uint count)
        {
            throw new NotImplementedException();
        }

        public bool OnJiaZhu(User user, uint count)
        {
            throw new NotImplementedException();
        }

        public bool OnKanPai(User user, uint targetId)
        {
            throw new NotImplementedException();
        }

        public bool OnQiPai(User user)
        {
            throw new NotImplementedException();
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
