
using System.Collections.Generic;
using FirServer.Define;
using FirServer.Interface;

namespace GameLibs.FirSango.Interface
{
    public interface IRoom : IObject
    {
        void Initialize(string name, uint roomId);
        bool OnEnter(User user);
        bool OnLeave(User user);
        bool OnPK(User user, uint targetId);
        Dictionary<long, User> GetUsers();
        uint GetRoomId();
        void OnDispose();
    }
}