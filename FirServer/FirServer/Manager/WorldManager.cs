using FirServer.Define;
using FirServer.Interface;
using log4net;

namespace FirServer.Manager
{
    public class WorldManager : BaseManager
    {
        private static readonly ILog logger = LogManager.GetLogger(AppConst.LogRepos?.Name, typeof(WorldManager));

        private Dictionary<string, IWorld> _worlds = new Dictionary<string, IWorld>();

        public override void Initialize()
        {
            var config = configMgr?.GetGlobalConfig();
            if (config?._gameConfigList != null)
            {
                foreach (var item in config._gameConfigList)
                {
                    if (item.Value != null && !string.IsNullOrEmpty(item.Value._libName) && !string.IsNullOrEmpty(item.Value._mainClass))
                    {
                        var assembly = assemblyMgr?.GetAssembly(item.Value._libName);
                        var objType = assembly?._assembly.GetType(item.Value._mainClass);
                        if (objType != null)
                        {
                            var world = Activator.CreateInstance(objType) as IWorld;
                            if (world != null)
                            {
                                world.Initialize();
                                _worlds.Add(item.Value._libName, world);
                            }
                        }
                    }
                }
            }
        }

        public IWorld? GetWorld(string libName)
        {
            _worlds.TryGetValue(libName, out IWorld? world);
            return world;
        }

        public override void OnDispose()
        {
        }
    }
}
