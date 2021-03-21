namespace FirClient.Manager
{
    public abstract class BaseDispatcher : BaseBehaviour
    {
        public abstract void OnMessage(string protoName, byte[] bytes);
    }
}