
using System.Collections.Generic;
using System.Net.WebSockets;
using FirServer.Defines;
using FirServer.Interface;

namespace GameLibs.FirSango.Interface
{
    public interface IRoom : IObject
    {
        void Initialize(string name, uint roomId);
        bool OnEnter(User user);
        bool OnLeave(User user);
        bool OnGenZhu(User user, uint count);
        bool OnJiaZhu(User user, uint count);
        bool OnKanPai(User user, uint targetId);
        bool OnQiPai(User user);
        bool OnPK(User user, uint targetId);
        Dictionary<long, User> GetUsers();
        uint GetRoomId();
        void OnDispose();
    }
}