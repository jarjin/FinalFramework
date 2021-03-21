using System.IO;

namespace FirClient.Handler
{
    public abstract class BaseHandler : BaseBehaviour
    {
        public abstract void OnMessage(byte[] bytes);
    }
}

