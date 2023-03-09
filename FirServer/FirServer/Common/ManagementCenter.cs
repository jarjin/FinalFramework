using FirServer.Define;
using FirServer.Interface;
using FirServer.Manager;
using FirServer.Service;
using log4net;

namespace FirServer.Common
{
    public class ManagementCenter
    {
        private static readonly Dictionary<string, IManager> mManagers = new Dictionary<string, IManager>();
        private static readonly ILog logger = LogManager.GetLogger(AppConst.LogRepos?.Name, typeof(ManagementCenter));

        internal static void Initialize()
        {
            AddManager<ConfigManager>();
            AddManager<LoggerManager>();
            AddManager<AssemblyManager>();
            AddManager<HandlerManager>();
            AddManager<NetworkManager>();
            AddManager<WorldManager>();

            var mgrCount = mManagers.Count;
            var currMgrs = new List<IManager>(mManagers.Values);
            for (int i = 0; i < mgrCount; i++)
            {
                currMgrs[i]?.Initialize();
            }
            logger.Info("Initialize Success!!!");
        }

        /// <summary>
        /// 注册RPC处理器
        /// </summary>
        internal static void RegRpcService(WebApplication _app)
        {
            _app?.MapGrpcService<GatewayService>();
        }

        ///添加管理器
        public static T? AddManager<T>() where T : IManager, new()
        {
            var name = typeof(T).ToString();
            if (mManagers.ContainsKey(name))
            {
                return default;
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
        public static T? GetManager<T>() where T : IManager
        {
            var name = typeof(T).ToString();
            if (!mManagers.TryGetValue(name, out IManager? manager))
            {
                return default;
            }
            return (T)manager;
        }

        /// 获取管理器
        public static IManager? GetManager(string name)
        {
            mManagers.TryGetValue(name, out IManager? manager);
            return manager;
        }

        internal static void OnUpdate()
        {
            throw new NotImplementedException();
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
