using FirCommon.Data;
using FirServer.Define;
using FirServer.Interface;
using GameLibs.Defines;
using GameLibs.Handler;
using GameLibs.Model;
using log4net;

namespace GameLibs
{
    public class GameWorld : GameBehaviour, IWorld
    {
        private static readonly ILog logger = LogManager.GetLogger(AppConst.LogRepos?.Name, typeof(GameWorld));

        public GameWorld() { }

        public void Initialize()
        {
            InitManager();
            TestTable();
            //TestDBServer();     //需要安装Mongodb
            HandlerMap.RegHandlers();
            logger.Info("GameWorld Initialized!!!");
        }

        ///初始化管理器
        void InitManager()
        {
            tableMgr?.Initialize();

            modelMgr?.AddModel(ModelNames.User, new UserModel());
            modelMgr?.AddModel(ModelNames.Battle, new BattleModel());

            roomMgr?.Initialize();
        }

        void TestTable()
        {
            //Test Table
            GlobalConfigTableItem? item = tableMgr?.globalConfigTable?.GetItemByKey("CommonWhite");
            logger?.Info(string.Format("id={0} value={1}", item?.id, item?.value));
        }

        public void OnDispose()
        {
        }
    }
}