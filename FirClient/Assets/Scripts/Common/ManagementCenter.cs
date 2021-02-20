using UnityEngine;
using FirClient.Manager;
using FirCommon.Data;

/// <summary>
/// Interface Manager Object 
/// </summary>
public class ManagementCenter {
    /// <summary>
    /// 游戏管理器对象
    /// </summary>
    private static GameObject _managerObject = null;
    public static GameObject managerObject {
        get {
            if (_managerObject == null)
                _managerObject = GameObject.FindWithTag("GameManager");
            return _managerObject;
        }
    }

    private static Main _main = null;
    public static Main main
    {
        get {
            if (_main == null)
            {
                _main = managerObject.GetComponent<Main>();
            }
            return _main;
        }
    }

    public static BaseManager GetManager(string managerName)
    {
        return BaseBehaviour.GetManager(managerName);
    }

    public static object GetExtManager(string managerName)
    {
        return BaseBehaviour.GetExtManager(managerName);
    }

    public static T GetManager<T>() where T : class
    {
        if (typeof(T) == typeof(ConfigManager))
        {
            return BaseBehaviour.configMgr as T;
        }
        else if (typeof(T) == typeof(TableManager))
        {
            return BaseBehaviour.tableMgr as T;
        }
        return BaseBehaviour.GetManager<T>();
    }
}