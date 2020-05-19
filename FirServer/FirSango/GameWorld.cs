using FirServer;
using FirServer.Interface;
using GameLibs.FirSango.Defines;
using GameLibs.FirSango.Handlers;
using log4net;
using Utility;

namespace GameLibs.FirSango
{
    public class GameWorld : GameBehaviour, IWorld
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(GameBehaviour));
        private uint gameId = 0;
        
        public void Initialize()
        {
            this.RegHandler();     //注册处理器
            this.InitManager();
        }

        ///初始化管理器
        async void InitManager() 
        {
            await AppUtil.Waitforms(1);

            //tableMgr.Initialize();
            //var item = tableMgr.globalConfigTable.GetItemByKey("CommonWhite");
            //logger.Info(string.Format("id={0} value={1}", item.id, item.value));

            roomMgr.Initialize();
        }

        ///注册处理器
        void RegHandler() 
        {
            handlerMgr.AddHandler(GameProtocal.Login, new LoginHandler());
            handlerMgr.AddHandler(GameProtocal.Register, new RegisterHandler());
            handlerMgr.AddHandler(GameProtocal.Logout, new LogoutHandler());
            handlerMgr.AddHandler(GameProtocal.ReqUserInfo, new ReqUserInfoHandler());
        }

        public uint GetGameId() 
        {
            return gameId;
        }

        public void SetGameId(uint id) 
        {
            gameId = id;
        }

        public void OnDispose()
        {
        }
    }
}