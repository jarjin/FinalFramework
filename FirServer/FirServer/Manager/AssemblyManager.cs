using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace FirServer.Manager
{
    public class AssemblyManager : BaseManager
    {
        private Dictionary<string, Assembly> assemblys = new Dictionary<string, Assembly>();

        public override void Initialize()
        {
            var config = configMgr.GetGlobalConfig();
            var games = config.gameList;
            foreach (var item in games)
            {
                var libName = item.Value.libName + ".dll";
                var assembly = LoadAssembly(libName);
                if (assembly != null)
                {
                    assemblys.Add(item.Value.libName, assembly);
                }
            }
        }

        private Assembly LoadAssembly(string assemblyName)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string assemblyPath = Path.GetDirectoryName(path);
            string currDir = assemblyPath + "/" + assemblyName;
            if (!File.Exists(currDir))
            {
                return null;
            }
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(currDir);
        }

        public Assembly GetAssembly(string libName)
        {
            if (assemblys.ContainsKey(libName))
            {
                return assemblys[libName];
            }
            return null;
        }

        private void UnloadAssembly(string assemblyName)
        {
        }
    }
}