using FirServer.Define;
using FirServer.Interface;

namespace GameLibs.Interface
{
    public interface IRoom : IObject
    {
        void Initialize(string name, uint roomId);
        bool OnEnter(MsgChannel user);
        bool OnLeave(MsgChannel user);
        bool OnPK(MsgChannel user, uint targetId);
        Dictionary<string, MsgChannel> GetUsers();
        uint GetRoomId();
        void OnDispose();
    }
}