using ProtoBuf;
using System.IO;

namespace FirClient.Handler
{
    public abstract class BaseHandler : BaseBehaviour
    {
        public abstract void OnMessage(byte[] bytes);

        protected T DeSerialize<T>(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }
    }
}

