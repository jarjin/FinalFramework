using System.IO;
using Google.Protobuf;
using Newtonsoft.Json;

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

        public static byte[] SerializeByteArray(this IMessage message)
        {
            byte[] data = null;
            if (message != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    MessageExtensions.WriteTo(message, stream);
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

        /// <summary>
        /// 序列化二进制
        /// </summary>
        public static void Serialize(string binraryPath, object instance)
        {
            var json = JsonConvert.SerializeObject(instance);
            File.WriteAllText(binraryPath, json);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        public static T Deserialize<T>(string fullPath) where T : class
        {
            var json = File.ReadAllText(fullPath);
            if (json == null) { return default(T); }
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
