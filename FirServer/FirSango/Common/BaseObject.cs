using FirCommon.Utility;
using System;

public abstract class BaseObject
{
    public bool isOnUpdate = false;

    protected string DataPath
    {
        get
        {
            return AppUtil.CurrDirectory;
        }
    }

    public abstract void Initialize();
    public abstract void OnUpdate(float deltaTime);
    public abstract void OnDispose();
}
