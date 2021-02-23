using Google.Protobuf;
using System.IO;

namespace FirCommon.Utility
{
    public static class ProtoUtil
    {
        public static byte[] Serialize(this IMessage message)
        {
            byte[] data = null;
            if (message != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Google.Protobuf.MessageExtensions.WriteTo(message, stream);
                    data = stream.ToArray();
                }
            }
            return data;
        }

        //static T DeSerialize<T>(byte[] bytes)
        //{
        //    using (MemoryStream ms = new MemoryStream(bytes))
        //    {
        //        T t = Serializer.Deserialize<T>(ms);
        //        return t;
        //    }
        //}
    }
}
