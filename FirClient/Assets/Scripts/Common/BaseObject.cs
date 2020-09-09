using FirClient.Utility;

public abstract class BaseObject
{
    public bool isOnUpdate = false;

    protected string DataPath
    {
        get
        {
            return Util.DataPath;
        }
    }

    public abstract void Initialize();
    public abstract void OnUpdate(float deltaTime);
    public abstract void OnDispose();
}
