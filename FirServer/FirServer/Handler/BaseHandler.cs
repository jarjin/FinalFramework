using FirServer.Interface;
using LiteNetLib;
using System.IO;

namespace FirServer.Handler
{
    public abstract class BaseHandler : BaseBehaviour, IHandler
    {
        public abstract void OnMessage(NetPeer peer, byte[] bytes);

        public T DeSerialize<T>(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }
    }
}
