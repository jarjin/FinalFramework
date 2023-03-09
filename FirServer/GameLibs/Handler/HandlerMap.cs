using FirServer.Common;
using FirServer.Manager;
using GameLibs.Define;

namespace GameLibs.Handler
{
    /// <summary>
    /// 在这里声明可以使用框架基类功能
    /// </summary>
    public static class HandlerMap
    {
        public static void RegHandlers()
        {
            var handleMgr = ManagementCenter.GetManager<HandlerManager>();
            if (handleMgr == null)
            {
                throw new ArgumentNullException(nameof(handleMgr));
            }
            handleMgr.AddHandler(HandlerNames.RegLogin, new LoginHandler());    //注册管理器
        }
    }
}
