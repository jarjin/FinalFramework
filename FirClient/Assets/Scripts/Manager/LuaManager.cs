using UnityEngine;
using LuaInterface;
using FirClient.Behaviour;
using FirClient.Utility;
using System;

namespace FirClient.Manager {
    public class LuaManager : BaseManager {
        private LuaState lua;
        private LuaLoader loader;
        private LuaLooper loop = null;
        private GameObject managerObject = null;
        private string luaSrcPath = string.Empty;

        public LuaState Lua
        {
            get { return lua; }
        }

        public override void Initialize()
        {
            managerObject = ManagementCenter.managerObject;
            
            luaSrcPath = Util.DataPath + "scripts";
            loader = new LuaLoader(luaSrcPath);
            lua = new LuaState();
            this.OpenLibs();
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, ManagementCenter.main);

            this.InitStart();   //初始化开始
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        void InitStart() 
        {
            this.InitLuaPath();
            this.lua.Start();       //启动LUA
            this.StartLooper();
            this.DoFile("Main");    //加载游戏
        }

        void StartLooper() 
        {
            loop = managerObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson() 
        {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        protected void OpenLuaSocket()
        {
            LuaConst.openLuaSocket = true;

            lua.BeginPreLoad();
            lua.RegFunction("socket.core", LuaOpen_Socket_Core);
            lua.RegFunction("mime.core", LuaOpen_Mime_Core);
            lua.EndPreLoad();
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Socket_Core(IntPtr L)
        {
            return LuaDLL.luaopen_socket_core(L);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Mime_Core(IntPtr L)
        {
            return LuaDLL.luaopen_mime_core(L);
        }

        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs() 
        {
            lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);

            this.OpenCJson();
            this.OpenLuaSocket();
        }

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        void InitLuaPath() 
        {
            if (AppConst.DebugMode) 
            {
                string luaPath = Application.dataPath;
                lua.AddSearchPath(luaPath + "/Lua");
                lua.AddSearchPath(luaPath + "/ToLua/Lua");
            } 
            else 
            {
                lua.AddSearchPath(luaSrcPath);
            }
        }

        public void DoFile(string filename) 
        {
            lua.DoFile(filename);
        }

        // Update is called once per frame
        public object[] CallFunction(string funcName, params object[] args) 
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) {
                return func.LazyCall(args);
            }
            return null;
        }

        public void LuaGC() 
        {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close() 
        {
            loop.Destroy();
            loop = null;

            lua.Dispose();
            lua = null;
            loader = null;
        }


        public override void OnDispose()
        {
            this.Close();
        }
    }
}