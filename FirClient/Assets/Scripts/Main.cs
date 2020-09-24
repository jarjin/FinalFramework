using UnityEngine;
using FirClient.Manager;
using FirClient.Define;

public class Main : GameBehaviour
{
    /// <summary>
    /// 初始化游戏管理器
    /// </summary>
    protected override void OnAwake()
    {
        AppConst.AppState = AppState.IsPlaying;
        base.OnAwake();
        this.Initialize();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void Initialize()
    {
        BaseBehaviour.Initialize();
        DontDestroyOnLoad(gameObject);  //防止销毁自己

        var gameMgr = ManagementCenter.GetManager<GameManager>();
        if (gameMgr != null)
        {
            gameMgr.Initialize();   //初始化游戏管理器 
        }
    }

    /// <summary>
    /// 每一帧更新
    /// </summary>
    protected override void OnUpdate()
    {
        base.OnUpdate();
        BaseBehaviour.OnUpdate(Time.deltaTime);
    }

    /// <summary>
    /// 析构函数
    /// </summary>
    protected override void OnDestroyMe()
    {
        base.OnDestroyMe();
        Debug.Log("~Main was destroyed");
    }

    private void OnApplicationQuit()
    {
        AppConst.AppState = AppState.Exiting;
    }
}