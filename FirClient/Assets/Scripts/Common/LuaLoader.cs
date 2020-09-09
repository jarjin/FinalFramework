using UnityEngine;
using System.Collections;
using System.IO;
using LuaInterface;
using FirClient.Utility;

namespace FirClient.Behaviour {
    /// <summary>
    /// 集成自LuaFileUtils，重写里面的ReadFile，
    /// </summary>
    public class LuaLoader : LuaFileUtils {
        private string luaSrcPath = string.Empty;

        // Use this for initialization
        public LuaLoader(string luaPath) 
        {
            beZip = false;
            instance = this;
            luaSrcPath = luaPath;
        }

        /// <summary>
        /// 当LuaVM加载Lua文件的时候，这里就会被调用，
        /// 用户可以自定义加载行为，只要返回byte[]即可。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override byte[] ReadFile(string fileName) 
        {
            return base.ReadFile(fileName);
        }
    }
}