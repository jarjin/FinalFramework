using LuaInterface;

namespace FirClient.Manager
{
    public abstract class BaseManager : BaseBehaviour
    {
        public bool isOnUpdate = false;

        [NoToLua]
        public abstract void Initialize();

        [NoToLua]
        public abstract void OnUpdate(float deltaTime);

        [NoToLua]
        public abstract void OnDispose();
    }
}

