using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace FirCommon.Utility
{
    public static class JsonHelper
    {
        public static string ToJson<T>(this T obj)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, obj);

                byte[] dataBytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(dataBytes, 0, (int)stream.Length);
                return Encoding.UTF8.GetString(dataBytes);
            }
        }

        public static T ToObject<T>(this string input) 
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
            {
                var obj = new DataContractJsonSerializer(typeof(T));
                return (T)obj.ReadObject(stream);
            }
        }
    }
}