using System.IO;
using Google.Protobuf;

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

        /// <summary>
        /// 序列化二进制
        /// </summary>
        public static void Serialize<T>(string binraryPath, T instance) where T : class
        {
            using (var ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize<T>(ms, instance);
                File.WriteAllBytes(binraryPath, ms.ToArray());
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        public static T Deserialize<T>(string fullPath) where T : class
        {
            var bytes = File.ReadAllBytes(fullPath);
            if (bytes == null) { return default(T); }

            using (var ms = new MemoryStream(bytes))
            {
                return ProtoBuf.Serializer.Deserialize<T>(ms);
            }
        }
    }
}
