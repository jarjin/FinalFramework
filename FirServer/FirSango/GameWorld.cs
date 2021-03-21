using FirCommon.Define;
using FirCommon.Utility;
using FirServer;
using FirServer.Interface;
using GameLibs.FirSango.Defines;
using GameLibs.FirSango.Handlers;
using GameLibs.FirSango.Model;
using log4net;
using System;

namespace GameLibs.FirSango
{
    public class GameWorld : GameBehaviour, IWorld
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(GameBehaviour));
        private uint gameId = 0;
        
        public void Initialize()
        {
            InitManager();
            RegHandler();
            TestTable();
            //TestDBServer();     //需要安装Mongodb
        }

        ///初始化管理器
        void InitManager()
        {
            tableMgr.Initialize();

            modelMgr.AddModel(ModelNames.User, new UserModel());
            modelMgr.AddModel(ModelNames.Battle, new BattleModel());

            roomMgr.Initialize();
        }

        void TestTable()
        {
            //Test Table
            var item = tableMgr.globalConfigTable.GetItemByKey("CommonWhite");
            logger.Info(string.Format("id={0} value={1}", item.id, item.value));
        }

        private async void TestDBServer()
        {
            ///Open DB
            dataMgr.Connect(GameConst.DB_URL);
            //dataMgr.DropDB(GameConst.DB_NAME);
            dataMgr.OpenDB(GameConst.DB_NAME);

            //Test Insert to DB
            var user = new UserInfo()
            {
                username = "张三",
                password = "123456",
                money = 10000L,
                lasttime = DateTime.Now.ToShortDateString()
            };
            var userModel = modelMgr.GetModel(ModelNames.User) as UserModel;
            var uid = userModel?.AddUser(user);
            logger.Info("UID:" + uid.Value);

            //Test Read from DB
            var userName = userModel?.GetUserName(uid.Value);
            logger.Info("UserName:" + userName);
            await AppUtil.Waitforms(1000);

            //Update filed to DB
            logger.Info("UID:" + uid.Value);
            userModel?.SetUserName(uid.Value, "李四");
            var newName = userModel?.GetUserName(uid.Value);
            logger.Info("NewName:" + newName);
        }

        ///注册处理器
        void RegHandler() 
        {
            handlerMgr.AddHandler(Protocal.Default, new DefaultHandler());
            handlerMgr.AddHandler(Protocal.Connected, new ConnectedHandler());
            handlerMgr.AddHandler(Protocal.Disconnect, new DisconnectHandler());
            handlerMgr.AddHandler(Protocal.ReqLogin, new LoginHandler());
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