using System.Collections.Generic;
using System.IO;
using ProtoBuf;


namespace FirServer.Utility
{
    public static class SerializationHelper
    {
        #region ProtoBuf

        /// <summary>
        /// 更新值
        /// </summary>
        public static void AddItem<T>(this Dictionary<string, byte[]> updateTable, string key, T t)
        {
            updateTable.Add(key, Serialize(t));
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        public static byte[] Serialize<T>(T t)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        public static T Deserialize<T>(this byte[] data)
        {
            if (data == null)
                return default(T);

            using (var ms = new MemoryStream(data))
            {
                try
                {
                    return Serializer.Deserialize<T>(ms);
                }
                catch
                {
                    //var sb = new StringBuilder();
                    //data.ToList().ForEach(i => sb.Append(i).Append(","));
                    //var str = sb.ToString();
                    //LogHelper.WriteErrorLog(string.Format("反序列化{0}失败: {1}", typeof(T).Name, str));
                    throw;
                }
            }
        }
        
        public static T DeepClone<T>(this T source)
            where T : new()
        {
            if (source == null)
                return source;

            var data = Serialize(source);
            return data.Deserialize<T>();
        }

        #endregion
    }
}