using System.Collections.Generic;
using log4net;
using FirServer.Manager;
using FirServer.Interface;

namespace FirServer.Common
{
    public class ManagementCenter
    {
        private static readonly Dictionary<string, IManager> mManagers = new Dictionary<string, IManager>();
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(ManagementCenter));

        /// <summary>
        /// 初始化管理器
        /// </summary>
        public static void Initialize()
        {
            AddManager<ConfigManager>();
            AddManager<DataManager>();
            AddManager<TimerManager>();
            AddManager<ModelManager>();
            AddManager<UserManager>();
            AddManager<AssemblyManager>();
            AddManager<HandlerManager>();
            AddManager<NetworkManager>();
            AddManager<WorldManager>();
            AddManager<ClientPeerManager>();

            var mgrCount = mManagers.Count;
            var currMgrs = new List<IManager>(mManagers.Values);
            for (int i = 0; i < mgrCount; i++)
            {
                currMgrs[i]?.Initialize();
            }
            logger.Info("Initialize Success!!!");
        }

        ///添加管理器
        public static T AddManager<T>() where T : IManager, new()
        {
            var name = typeof(T).ToString();
            if (mManagers.ContainsKey(name))
            {
                return default(T);
            }
            var obj = new T();
            mManagers.Add(name, obj);
            return (T)obj;
        }

        ///添加管理器
        public static void AddWithInitManager<T>() where T : IManager, new()
        {
            var manager = AddManager<T>();
            if (manager != null)
            {
                manager.Initialize();
            }
        }

        /// 获取管理器
        public static T GetManager<T>() where T : IManager
        {
            var name = typeof(T).ToString();
            IManager manager = null;
            if (mManagers.TryGetValue(name, out manager))
            {
                return (T)manager;
            }
            return default(T);
        }

        /// 获取管理器
        public static IManager GetManager(string name)
        {
            IManager manager = null;
            mManagers.TryGetValue(name, out manager);
            return manager;
        }

        public static void OnDispose()
        {
            foreach (var de in mManagers)
            {
                if (de.Value != null)
                {
                    de.Value.OnDispose();
                }
            }
        }
    }
}
