using LiteNetLib.Utils;
using UnityEngine;

namespace FirClient.Extensions
{
    public static class LiteNetLibExtensions
    {
        /// <summary>
        /// NetDataWriter
        /// </summary>
        public static void Put(this NetDataWriter dw, Vector2 v)
        {
            dw.Put(v.x);
            dw.Put(v.y);
        }

        public static void Put(this NetDataWriter dw, Vector3 v)
        {
            dw.Put(v.x);
            dw.Put(v.y);
            dw.Put(v.z);
        }

        public static void Put(this NetDataWriter dw, Quaternion v)
        {
            dw.Put(v.x);
            dw.Put(v.y);
            dw.Put(v.z);
            dw.Put(v.w);
        }

        public static Vector2 GetVector2(this NetDataReader reader)
        {
            var x = reader.GetFloat();
            var y = reader.GetFloat();
            return new Vector2(x, y);
        }

        public static Vector3 GetVector3(this NetDataReader reader)
        {
            var x = reader.GetFloat();
            var y = reader.GetFloat();
            var z = reader.GetFloat();
            return new Vector3(x, y, z);
        }

        public static Quaternion GetQuaternion(this NetDataReader reader)
        {
            var x = reader.GetFloat();
            var y = reader.GetFloat();
            var z = reader.GetFloat();
            var w = reader.GetFloat();
            return new Quaternion(x, y, z, w);
        }
    }
}
