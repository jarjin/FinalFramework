using FirCommon.Data;
using FirServer;
using FirServer.Common;
using GameLibs.FirSango.Managers;

public class GameBehaviour : BaseBehaviour
{
    /// <summary>
    /// TableManager在客户端+服务器都是特殊的存在，
    /// 不依赖于框架的实现，属于特定的管理器。
    /// </summary>
    protected static TableManager tableMgr 
    {
        get { return TableManager.Create(); }
    }

    /// <summary>
    /// 房间管理器
    /// </summary>
    private static RoomManager _roomMgr = null;
    protected static RoomManager roomMgr
    {
        get
        {
            if (_roomMgr == null)
            {
                _roomMgr = ManagementCenter.AddManager<RoomManager>();
            }
            return _roomMgr;
        }
    }
}
