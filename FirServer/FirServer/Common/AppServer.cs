using System.Collections.Generic;
using System.IO;
using FirServer.Common;
using FirServer.Interface;
using FirServer.Managers;
using LiteNetLib;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace FirServer
{
    public class AppServer : BaseBehaviour
    {
        public static ILoggerRepository repository { get; set; }
        private static ILog logger = null;
        private static readonly Dictionary<string, IManager> mManagers = new Dictionary<string, IManager>();

        public bool IsRunning { get; private set; }

        public NetManager mServer { get; private set; }
        private ServerListener mListener = null;
        private NetworkManager mNetMgr = null;

        protected void Initialize(int port)
        {
            appServer = this;
            IsRunning = false;

            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            logger = LogManager.GetLogger(repository.Name, typeof(AppServer));

            ManagerCenter.Initialize();
            mNetMgr = ManagerCenter.GetManager<NetworkManager>();

            StartServer(port);
        }

        /// <summary>
        /// 初始化服务器
        /// </summary>
        public void StartServer(int port)
        {
            mListener = new ServerListener();
            mServer = new NetManager(mListener);
            mServer.Start(port);
            mServer.UpdateTime = 15;
            IsRunning = true;
            logger.Warn("MasterServer Started!!");
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        public void StopServer()
        {
            if (mServer != null)
            {
                mServer.Stop();
                mServer = null;
            }
            IsRunning = false;
            logger.Warn("MasterServer Stoped!!");
        }


        public void OnUpdate()
        {
            if (mServer != null)
            {
                mServer.PollEvents();
            }
            ///更新管理器
            foreach (var de in mManagers)
            {
                if (de.Value != null)
                {
                    //de.Value.OnFrameUpdate();
                }
            }
        }
    }
}
