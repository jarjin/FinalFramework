﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class AppConstWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(AppConst), typeof(System.Object));
		L.RegFunction("New", new LuaCSFunction(_CreateAppConst));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.RegVar("DebugMode", new LuaCSFunction(get_DebugMode), new LuaCSFunction(set_DebugMode));
		L.RegVar("LogMode", new LuaCSFunction(get_LogMode), new LuaCSFunction(set_LogMode));
		L.RegVar("UpdateMode", new LuaCSFunction(get_UpdateMode), new LuaCSFunction(set_UpdateMode));
		L.RegVar("NetworkMode", new LuaCSFunction(get_NetworkMode), new LuaCSFunction(set_NetworkMode));
		L.RegVar("LuaByteMode", new LuaCSFunction(get_LuaByteMode), new LuaCSFunction(set_LuaByteMode));
		L.RegVar("ShowFps", new LuaCSFunction(get_ShowFps), new LuaCSFunction(set_ShowFps));
		L.RegVar("AppState", new LuaCSFunction(get_AppState), new LuaCSFunction(set_AppState));
		L.RegVar("GameFrameRate", new LuaCSFunction(get_GameFrameRate), new LuaCSFunction(set_GameFrameRate));
		L.RegConstant("BatchProcCount", 5);
		L.RegConstant("NetMessagePoolMax", 100);
		L.RegConstant("DefaultSortLayer", 0);
		L.RegConstant("RoleSortLayer", 3);
		L.RegConstant("BattleTempSortingOrder", 100);
		L.RegVar("AppName", new LuaCSFunction(get_AppName), null);
		L.RegVar("AppPrefix", new LuaCSFunction(get_AppPrefix), null);
		L.RegVar("ExtName", new LuaCSFunction(get_ExtName), null);
		L.RegVar("LuaTempDir", new LuaCSFunction(get_LuaTempDir), null);
		L.RegVar("ABDir", new LuaCSFunction(get_ABDir), null);
		L.RegVar("ResIndexFile", new LuaCSFunction(get_ResIndexFile), null);
		L.RegVar("GameSettingName", new LuaCSFunction(get_GameSettingName), null);
		L.RegVar("ResUrl", new LuaCSFunction(get_ResUrl), null);
		L.RegVar("PatchUrl", new LuaCSFunction(get_PatchUrl), null);
		L.RegVar("SocketAddress", new LuaCSFunction(get_SocketAddress), null);
		L.RegConstant("SocketPort", 15940);
		L.RegVar("ExtCmdName", new LuaCSFunction(get_ExtCmdName), null);
		L.RegVar("ProtoNameKey", new LuaCSFunction(get_ProtoNameKey), null);
		L.RegVar("ByteArrayKey", new LuaCSFunction(get_ByteArrayKey), null);
		L.RegVar("MsgTypeKey", new LuaCSFunction(get_MsgTypeKey), null);
		L.RegVar("TablePath", new LuaCSFunction(get_TablePath), new LuaCSFunction(set_TablePath));
		L.RegVar("DataPrefixs", new LuaCSFunction(get_DataPrefixs), new LuaCSFunction(set_DataPrefixs));
		L.RegVar("AssetPaths", new LuaCSFunction(get_AssetPaths), new LuaCSFunction(set_AssetPaths));
		L.RegVar("WaitForSeconds_01", new LuaCSFunction(get_WaitForSeconds_01), null);
		L.RegVar("WaitForEndOfFrame", new LuaCSFunction(get_WaitForEndOfFrame), null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateAppConst(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				AppConst obj = new AppConst();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: AppConst.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DebugMode(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, AppConst.DebugMode);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LogMode(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, AppConst.LogMode);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UpdateMode(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, AppConst.UpdateMode);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NetworkMode(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, AppConst.NetworkMode);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LuaByteMode(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, AppConst.LuaByteMode);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ShowFps(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, AppConst.ShowFps);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AppState(IntPtr L)
	{
		try
		{
			ToLua.Push(L, AppConst.AppState);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GameFrameRate(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushinteger(L, AppConst.GameFrameRate);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AppName(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.AppName);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AppPrefix(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.AppPrefix);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ExtName(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.ExtName);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LuaTempDir(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.LuaTempDir);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ABDir(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.ABDir);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ResIndexFile(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.ResIndexFile);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GameSettingName(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.GameSettingName);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ResUrl(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.ResUrl);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PatchUrl(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.PatchUrl);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SocketAddress(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.SocketAddress);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ExtCmdName(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.ExtCmdName);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ProtoNameKey(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.ProtoNameKey);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ByteArrayKey(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.ByteArrayKey);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MsgTypeKey(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.MsgTypeKey);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TablePath(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, AppConst.TablePath);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DataPrefixs(IntPtr L)
	{
		try
		{
			ToLua.Push(L, AppConst.DataPrefixs);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AssetPaths(IntPtr L)
	{
		try
		{
			ToLua.Push(L, AppConst.AssetPaths);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_WaitForSeconds_01(IntPtr L)
	{
		try
		{
			ToLua.PushSealed(L, AppConst.WaitForSeconds_01);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_WaitForEndOfFrame(IntPtr L)
	{
		try
		{
			ToLua.PushSealed(L, AppConst.WaitForEndOfFrame);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_DebugMode(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			AppConst.DebugMode = arg0;
			AppConst.DebugMode = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_LogMode(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			AppConst.LogMode = arg0;
			AppConst.LogMode = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_UpdateMode(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			AppConst.UpdateMode = arg0;
			AppConst.UpdateMode = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_NetworkMode(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			AppConst.NetworkMode = arg0;
			AppConst.NetworkMode = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_LuaByteMode(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			AppConst.LuaByteMode = arg0;
			AppConst.LuaByteMode = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ShowFps(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			AppConst.ShowFps = arg0;
			AppConst.ShowFps = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_AppState(IntPtr L)
	{
		try
		{
			FirClient.Define.AppState arg0 = (FirClient.Define.AppState)ToLua.CheckObject(L, 2, TypeTraits<FirClient.Define.AppState>.type);
			AppConst.AppState = arg0;
			AppConst.AppState = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_GameFrameRate(IntPtr L)
	{
		try
		{
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			AppConst.GameFrameRate = arg0;
			AppConst.GameFrameRate = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_TablePath(IntPtr L)
	{
		try
		{
			string arg0 = ToLua.CheckString(L, 2);
			AppConst.TablePath = arg0;
			AppConst.TablePath = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_DataPrefixs(IntPtr L)
	{
		try
		{
			string[] arg0 = ToLua.CheckStringArray(L, 2);
			AppConst.DataPrefixs = arg0;
			AppConst.DataPrefixs = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_AssetPaths(IntPtr L)
	{
		try
		{
			string[] arg0 = ToLua.CheckStringArray(L, 2);
			AppConst.AssetPaths = arg0;
			AppConst.AssetPaths = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

