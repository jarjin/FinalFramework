using FirCommon.Utility;
using FirServer.Define;
using log4net;

namespace FirServer.Manager
{
    public class AssemblyManager : BaseManager
    {
        private static readonly ILog logger = LogManager.GetLogger(AppConst.LogRepos?.Name, typeof(AssemblyManager));
        private Dictionary<string, AssemblyInfo> _assemblys = new Dictionary<string, AssemblyInfo>();

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            var config = configMgr?.GetGlobalConfig();
            if (config?._gameConfigList != null )
            {
                foreach (var item in config._gameConfigList)
                {
                    if (item.Value == null) continue;

                    var libName = item.Value._libName + ".dll";
                    var assembly = LoadAssembly(libName, item.Value._version);
                    if (assembly != null && item.Value != null && !string.IsNullOrEmpty(item.Value._libName))
                    {
                        _assemblys?.Add(item.Value._libName, assembly);
                    }
                }
            }
        }

        AssemblyInfo? LoadAssembly(string assemblyName, string? _version)
        {
            string assemblyPath = AppUtil.CurrDirectory;
            string currDir = assemblyPath + assemblyName;
            if (!File.Exists(currDir))
            {
                logger.Error(string.Format("Game DLL file ({0}) not exist!!!", currDir));
                return null;
            }
            try
            {
                var context = new GameAssemblyLoadContext();
                var assembly = context.LoadFromAssemblyPath(currDir);
                logger.Warn(string.Format("GameDLL LoadOK, Path:{0} Version:{1}", currDir, _version));
                return new AssemblyInfo(context, assembly);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return null;
        }

        public AssemblyInfo? GetAssembly(string libName)
        {
            _assemblys.TryGetValue(libName, out AssemblyInfo? assembly);
            return assembly;
        }

        private void UnloadAssembly(string assemblyName)
        {
            var assembly = GetAssembly(assemblyName);
            try
            {
                assembly?._context?.Unload();
            }
            finally
            {
                assembly = null;
                _assemblys.Remove(assemblyName);
            }
            logger.Warn(string.Format("Assembly {0} was Unloaded!!", assemblyName));
        }

        public override void OnDispose()
        {
        }
    }
}
