namespace FirServer.Interface
{
    public interface IManager : IObject
    {
        void Initialize();
        void OnDispose();
    }
}
