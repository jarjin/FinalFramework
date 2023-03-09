using Google.Protobuf;
using System;
using System.IO;

namespace FirCommon.Utility
{
    public static class ProtoUtil
    {
        public static ByteString Serialize(this IMessage message)
        {
            ByteString data = null;
            if (message != null)
            {
                using (var stream = new MemoryStream())
                {
                    MessageExtensions.WriteTo(message, stream);
                    data = ByteString.CopyFrom(stream.ToArray());
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
