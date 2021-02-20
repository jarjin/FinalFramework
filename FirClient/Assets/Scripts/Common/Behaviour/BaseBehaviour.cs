using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirClient.Manager;
using FirClient.Component;
using FirClient.Utility;
using LuaInterface;
using FirCommon.Data;

public abstract class BaseBehaviour
{
    static Dictionary<string, BaseManager> Managers = new Dictionary<string, BaseManager>();
    static Dictionary<string, BaseObject> ExtManagers = new Dictionary<string, BaseObject>();

    private Canvas _uiCanvas;
    protected Canvas uiCanvas
    {
        get
        {
            if (_uiCanvas == null)
            {
                _uiCanvas = GameObject.Find("/MainGame/UICanvas").GetComponent<Canvas>();
            }
            return _uiCanvas;
        }
    }
    
    private GameObject _battleScene;
    protected GameObject battleScene
    {
        get
        {
            if (_battleScene == null)
            {
                _battleScene = GameObject.Find("/MainGame/BattleScene");
            }
            return _battleScene;
        }
    }

    private Camera _battleCamera;
    protected Camera battleCamera
    {
        get
        {
            if (_battleCamera == null)
            {
                _battleCamera = Camera.main;
            }
            return _battleCamera;
        }
    }

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public T Instantiate<T>(T original) where T : UnityEngine.Object
    {
        return GameObject.Instantiate<T>(original);
    }

    public static void Destroy(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            GameObject.Destroy(obj);
        }
    }

    public static void Destroy(UnityEngine.Object obj, float t)
    {
        if (obj != null)
        {
            GameObject.Destroy(obj, t);
        }
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return ManagementCenter.main.StartCoroutine(routine);
    }

    [NoToLua]
    public static void Initialize()
    {
        InitGameSettings();
        InitManager();
        InitExtManager();
        InitComponent();
    }

    /// <summary>
    /// 初始化游戏设置
    /// </summary>
    private static void InitGameSettings()
    {
        var settings = Util.LoadGameSettings();
        if (settings != null)
        {
            AppConst.LogMode = settings.logMode;
            AppConst.DebugMode = settings.debugMode;
            AppConst.GameFrameRate = settings.GameFrameRate;
            AppConst.UpdateMode = settings.updateMode;
            AppConst.NetworkMode = settings.networkMode;
            AppConst.LuaByteMode = settings.luaByteMode;
            AppConst.ShowFps = settings.showFps;
        }
    }

    /// <summary>
    /// 初始化组件
    /// </summary>
    private static void InitComponent()
    {
        var mainGame = GameObject.Find("/MainGame");
        if (mainGame != null)
        {
            mainGame.AddComponent<CObjectFollow>();
            if (AppConst.ShowFps)
            {
                mainGame.AddComponent<CFPSDisplay>();
            }
        }
    }

    /// <summary>
    /// 初始化扩展管理器
    /// </summary>
    private static void InitExtManager()
    {
        ExtManagers.Add("TimerManager", timerMgr);
        ExtManagers.Add("ConfigManager", configMgr);
        ExtManagers.Add("TableManager", tableMgr);
    }

    public static object GetExtManager(string componentName)
    {
        if (!ExtManagers.ContainsKey(componentName))
        {
            return null;
        }
        return ExtManagers[componentName];
    }

    /// /////////////////////////////////////////MANAGERS//////////////////////////////////////////
    /// <summary>
    /// 初始化管理器
    /// </summary>
    static void InitManager()
    {
        AddManager<GameManager>();
        AddManager<NPCManager>();
        AddManager<SoundManager>();
        AddManager<NetworkManager>();
        AddManager<ResourceManager>();
        AddManager<ObjectManager>();
        AddManager<HandlerManager>();
        AddManager<ExtractManager>();
        AddManager<EffectManager>();
        AddManager<BulletManager>();
        AddManager<BattleViewManager>();
        AddManager<LuaManager>();
        AddManager<UpdateManager>();
        AddManager<ShaderManager>();
        AddManager<FontManager>();
    }

    static T AddManager<T>() where T : BaseManager, new()
    {
        var type = typeof(T);
        var obj = new T();
        Managers.Add(type.Name, obj);
        return obj;
    }

    public static T GetManager<T>() where T : class
    {
        var type = typeof(T);
        if (!Managers.ContainsKey(type.Name))
        {
            return null;
        }
        return Managers[type.Name] as T;
    }

    public static BaseManager GetManager(string managerName)
    {
        if (!Managers.ContainsKey(managerName))
        {
            return null;
        }
        return Managers[managerName];
    }

    private static ObjectManager _objMgr;
    protected static ObjectManager objMgr
    {
        get
        {
            if (_objMgr == null)
            {
                _objMgr = GetManager<ObjectManager>();
            }
            return _objMgr;
        }
    }

    private static SoundManager _soundMgr;
    protected static SoundManager soundMgr
    {
        get
        {
            if (_soundMgr == null)
            {
                _soundMgr = GetManager<SoundManager>();
            }
            return _soundMgr;
        }
    }

    private static NPCManager _npcMgr;
    protected static NPCManager npcMgr
    {
        get
        {
            if (_npcMgr == null)
            {
                _npcMgr = GetManager<NPCManager>();
            }
            return _npcMgr;
        }
    }

    private static HandlerManager _handlerMgr;
    protected static HandlerManager handlerMgr
    {
        get
        {
            if (_handlerMgr == null)
            {
                _handlerMgr = GetManager<HandlerManager>();
            }
            return _handlerMgr;
        }
    }

    private static ConfigManager _configMgr;
    public static ConfigManager configMgr
    {
        get
        {
            if (_configMgr == null)
            {
                _configMgr = ConfigManager.Create();
            }
            return _configMgr;
        }
    }

    private static BulletManager _bulletMgr;
    protected static BulletManager bulletMgr
    {
        get
        {
            if (_bulletMgr == null)
            {
                _bulletMgr = GetManager<BulletManager>();
            }
            return _bulletMgr;
        }
    }

    private static EffectManager _effectMgr;
    protected static EffectManager effectMgr
    {
        get
        {
            if (_effectMgr == null)
            {
                _effectMgr = GetManager<EffectManager>();
            }
            return _effectMgr;
        }
    }

    private static CTimer _timerMgr;
    protected static CTimer timerMgr
    {
        get
        {
            if (_timerMgr == null)
            {
                _timerMgr = CTimer.Create();
            }
            return _timerMgr;
        }
    }

    private static ResourceManager _resMgr;
    protected static ResourceManager resMgr
    {
        get
        {
            if (_resMgr == null)
            {
                _resMgr = GetManager<ResourceManager>();
            }
            return _resMgr;
        }
    }

    private static NetworkManager _networkMgr;
    protected static NetworkManager networkMgr
    {
        get
        {
            if (_networkMgr == null)
            {
                _networkMgr = GetManager<NetworkManager>();
            }
            return _networkMgr;
        }
    }

    private static BattleViewManager _battleViewMgr;
    public static BattleViewManager battleViewMgr
    {
        get
        {
            if (_battleViewMgr == null)
            {
                _battleViewMgr = GetManager<BattleViewManager>();
            }
            return _battleViewMgr;
        }
    }

    private static LuaManager _luaMgr;
    public static LuaManager luaMgr {
        get {
            if (_luaMgr == null)
            {
                _luaMgr = GetManager<LuaManager>();
            }
            return _luaMgr;
        }
    }

    private static GameManager _gameMgr;
    public static GameManager gameMgr 
    {
        get {
            if (_gameMgr == null) 
            {
                _gameMgr = GetManager<GameManager>();
            }
            return _gameMgr;
        }
    }

    private static TableManager _tableMgr;
    public static TableManager tableMgr {
        get {
            if (_tableMgr == null)
            {
                _tableMgr = TableManager.Create();
            }
            return _tableMgr;
        }
    }

    private static UpdateManager _updateMgr;
    public static UpdateManager updateMgr {
        get {
            if (_updateMgr == null)
            {
                _updateMgr = GetManager<UpdateManager>();
            }
            return _updateMgr;
        }
    }

    private static ExtractManager _extractMgr;
    public static ExtractManager extractMgr {
        get {
            if (_extractMgr == null)
            {
                _extractMgr = GetManager<ExtractManager>();
            }
            return _extractMgr;
        }
    }

    private static ShaderManager _shaderMgr;
    public static ShaderManager shaderMgr {
        get {
            if (_shaderMgr == null)
            {
                _shaderMgr = GetManager<ShaderManager>();
            }
            return _shaderMgr;
        }
    }

    private static FontManager _fontMgr;
    public static FontManager fontMgr {
        get {
            if (_fontMgr == null)
            {
                _fontMgr = GetManager<FontManager>();
            }
            return _fontMgr;
        }
    }

    /// ///////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 控制器更新
    /// </summary>
    /// <param name="deltaTime"></param>
    public static void OnUpdate(float deltaTime)
    {
        ///驱动所有的管理器
        foreach (var mgr in Managers)
        {
            if (mgr.Value != null && mgr.Value.isOnUpdate)
            {
                mgr.Value.OnUpdate(deltaTime);
            }
        }

        ///驱动所有的组件
        foreach (var com in ExtManagers)
        {
            if (com.Value != null && com.Value.isOnUpdate)
            {
                com.Value.OnUpdate(deltaTime);
            }
        }
    }
}
