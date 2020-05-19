namespace FirServer.Interface
{
    public interface IWorld : IObject
    {
        void Initialize();
        void OnDispose();
    }
}