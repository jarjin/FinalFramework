using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using FirServer.Define;

namespace FirServer.Manager
{
    public class UserManager : BaseManager
    {
        private static Dictionary<long, User> users = new Dictionary<long, User>();
        public override void Initialize()
        {
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
    }
}
