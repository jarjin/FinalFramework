﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FirClient_Define_ResultCodeWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(FirClient.Define.ResultCode));
		L.RegVar("Success", new LuaCSFunction(get_Success), null);
		L.RegVar("Failed", new LuaCSFunction(get_Failed), null);
		L.RegVar("ExistUser", new LuaCSFunction(get_ExistUser), null);
		L.RegFunction("IntToEnum", new LuaCSFunction(IntToEnum));
		L.EndEnum();
		TypeTraits<FirClient.Define.ResultCode>.Check = CheckType;
		StackTraits<FirClient.Define.ResultCode>.Push = Push;
	}

	static void Push(IntPtr L, FirClient.Define.ResultCode arg)
	{
		ToLua.Push(L, arg);
	}

	static Type TypeOf_FirClient_Define_ResultCode = typeof(FirClient.Define.ResultCode);

	static bool CheckType(IntPtr L, int pos)
	{
		return TypeChecker.CheckEnumType(TypeOf_FirClient_Define_ResultCode, L, pos);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Success(IntPtr L)
	{
		ToLua.Push(L, FirClient.Define.ResultCode.Success);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Failed(IntPtr L)
	{
		ToLua.Push(L, FirClient.Define.ResultCode.Failed);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ExistUser(IntPtr L)
	{
		ToLua.Push(L, FirClient.Define.ResultCode.ExistUser);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tointeger(L, 1);
		FirClient.Define.ResultCode o = (FirClient.Define.ResultCode)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}

