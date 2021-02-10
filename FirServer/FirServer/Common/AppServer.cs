using System.Collections.Generic;
using System.IO;
using FirServer.Common;
using FirServer.Interface;
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

        private ServerListener mListener = null;

        protected void Initialize(int port)
        {
            IsRunning = false;

            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            logger = LogManager.GetLogger(repository.Name, typeof(AppServer));

            ManagementCenter.Initialize();

            StartServer(port);
        }

        public void StartServer(int port)
        {
            mListener = new ServerListener();
            mListener.StartServer(port);
            IsRunning = true;
            logger.Warn("MasterServer Started!!");
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        public void StopServer()
        {
            mListener.StopServer();
            IsRunning = false;
            logger.Warn("MasterServer Stoped!!");
        }


        public void OnUpdate()
        {
            mListener.OnUpdate();

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
