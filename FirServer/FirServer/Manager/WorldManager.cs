using System;
using System.Collections.Generic;
using FirServer.Interface;

namespace FirServer.Manager
{
    public class WorldManager : BaseManager
    {
        Dictionary<string, IWorld> worlds = new Dictionary<string, IWorld>();

        public override void Initialize()
        {
            var config = configMgr.GetGlobalConfig();
            var gameList = config.gameList;
            foreach (var item in gameList)
            {
                var assembly = assemblyMgr.GetAssembly(item.Value.libName);
                var objType = assembly.GetType(item.Value.mainClass);
                if (objType != null) 
                {
                    var world = Activator.CreateInstance(objType) as IWorld;
                    if (world != null) 
                    {
                        world.Initialize();
                        worlds.Add(item.Value.libName, world);
                    }
                }
            }
        }
    }
}