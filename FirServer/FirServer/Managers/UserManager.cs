using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using FirServer.Defines;
using FirServer.Interface;

namespace FirServer.Managers
{
    public class UserManager : BaseBehaviour, IManager
    {
        private static Dictionary<long, User> users = new Dictionary<long, User>();
        public void Initialize()
        {
            userMgr = this;
            users.Clear();
        }

        public User AddUser(long socketid, WebSocket socket)
        {
            User user = null;
            if (!users.ContainsKey(socketid))
            {
                user = new User(socket);
                users.Add(socketid, user);
            }
            else
            {
                throw new Exception("AddUser uid:>" + socketid);
            }
            return user;
        }

        public User GetUser(long socketid)
        {
            User user = null;
            users.TryGetValue(socketid, out user);
            return user;
        }

        public void RemoveUser(long socketid)
        {
            if (users.ContainsKey(socketid))
            {
                users.Remove(socketid);
            }
        }

        public void OnDispose()
        {
            userMgr = null;
        }
    }
}
