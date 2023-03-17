﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FirClient_View_RoleViewWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FirClient.View.RoleView), typeof(FirClient.View.NPCView));
		L.RegFunction("ShowNpc", new LuaCSFunction(ShowNpc));
		L.RegFunction("PlayRoleAnim", new LuaCSFunction(PlayRoleAnim));
		L.RegFunction("LookAt", new LuaCSFunction(LookAt));
		L.RegFunction("SetFaceDir", new LuaCSFunction(SetFaceDir));
		L.RegFunction("NpcSkillAttack", new LuaCSFunction(NpcSkillAttack));
		L.RegFunction("OnNpcDeath", new LuaCSFunction(OnNpcDeath));
		L.RegFunction("New", new LuaCSFunction(_CreateFirClient_View_RoleView));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFirClient_View_RoleView(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				FirClient.View.RoleView obj = new FirClient.View.RoleView();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FirClient.View.RoleView.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowNpc(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FirClient.View.RoleView obj = (FirClient.View.RoleView)ToLua.CheckObject<FirClient.View.RoleView>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				obj.ShowNpc(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FirClient.View.RoleView obj = (FirClient.View.RoleView)ToLua.CheckObject<FirClient.View.RoleView>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				System.Action arg1 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 3);
				obj.ShowNpc(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FirClient.View.RoleView.ShowNpc");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayRoleAnim(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FirClient.View.RoleView obj = (FirClient.View.RoleView)ToLua.CheckObject<FirClient.View.RoleView>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				obj.PlayRoleAnim(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FirClient.View.RoleView obj = (FirClient.View.RoleView)ToLua.CheckObject<FirClient.View.RoleView>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				obj.PlayRoleAnim(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FirClient.View.RoleView.PlayRoleAnim");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LookAt(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FirClient.View.RoleView obj = (FirClient.View.RoleView)ToLua.CheckObject<FirClient.View.RoleView>(L, 1);
			UnityEngine.Vector3Int arg0 = StackTraits<UnityEngine.Vector3Int>.Check(L, 2);
			obj.LookAt(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetFaceDir(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FirClient.View.RoleView obj = (FirClient.View.RoleView)ToLua.CheckObject<FirClient.View.RoleView>(L, 1);
			FirClient.Data.FaceDir arg0 = (FirClient.Data.FaceDir)ToLua.CheckObject(L, 2, TypeTraits<FirClient.Data.FaceDir>.type);
			obj.SetFaceDir(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int NpcSkillAttack(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			FirClient.View.RoleView obj = (FirClient.View.RoleView)ToLua.CheckObject<FirClient.View.RoleView>(L, 1);
			FirClient.View.RoleView arg0 = (FirClient.View.RoleView)ToLua.CheckObject<FirClient.View.RoleView>(L, 2);
			FirClient.Data.NpcSkillAttackEvent arg1 = (FirClient.Data.NpcSkillAttackEvent)ToLua.CheckObject<FirClient.Data.NpcSkillAttackEvent>(L, 3);
			System.Action arg2 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 4);
			obj.NpcSkillAttack(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnNpcDeath(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FirClient.View.RoleView obj = (FirClient.View.RoleView)ToLua.CheckObject<FirClient.View.RoleView>(L, 1);
			System.Action arg0 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 2);
			obj.OnNpcDeath(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

