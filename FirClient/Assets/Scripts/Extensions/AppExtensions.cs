using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace FirClient.Extensions
{
    public static class AppExtensions
    {
        public static int ToInt(this object o)
        {
            return Convert.ToInt32(o);
        }

        public static uint ToUint(this object o)
        {
            return Convert.ToUInt32(o);
        }

        public static float ToFloat(this object o)
        {
            return Convert.ToSingle(o);
        }

        public static long ToLong(this object o)
        {
            return Convert.ToInt64(o);
        }

        public static List<T> ToList<T>(this string o, char splitChar)
        {
            if (string.IsNullOrEmpty(o)) return null;
            var list = new List<T>();
            var strs = o.Split(splitChar);
            foreach (var str in strs)
            {
                list.Add((T)Convert.ChangeType(str, typeof(T)));
            }
            return list;
        }

        public static bool Between(this sbyte target, sbyte min, sbyte max)
        {
            return target >= min && target <= max;
        }

        public static bool Between(this byte target, byte min, byte max)
        {
            return target >= min && target <= max;
        }

        public static bool Between(this short target, short min, short max)
        {
            return target >= min && target <= max;
        }

        public static bool Between(this ushort target, ushort min, ushort max)
        {
            return target >= min && target <= max;
        }

        public static bool Between(this int target, int min, int max)
        {
            return target >= min && target <= max;
        }

        public static bool Between(this uint target, uint min, uint max)
        {
            return target >= min && target <= max;
        }

        public static bool Between(this long target, long min, long max)
        {
            return target >= min && target <= max;
        }

        public static bool Between(this ulong target, ulong min, ulong max)
        {
            return target >= min && target <= max;
        }

        public static bool Between(this float target, float min, float max)
        {
            return target >= min && target <= max;
        }

        public static bool Between(this double target, double min, double max)
        {
            return target >= min && target <= max;
        }

        public static bool Near(this float target, float other, float distance)
        {
            return target.Between(other - distance, other + distance);
        }

        public static bool Near(this object target, object other, float distance)
        {
            return target == other;
        }

        public static string FirstCharToLower(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;
        }

        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            string str = input.First().ToString().ToUpper() + input.Substring(1);
            return str;
        }

        public static Vector2? ToVec2(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string[] strs = input.Split(splitChar);
            return new Vector2(float.Parse(strs[0]), float.Parse(strs[1]));
        }

        public static Vector3? ToVec3(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string[] strs = input.Split(splitChar);
            return new Vector3(float.Parse(strs[0]), float.Parse(strs[1]), float.Parse(strs[2]));
        }

        public static Vector3Int? ToVec3Int(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string[] strs = input.Split(splitChar);
            return new Vector3Int(int.Parse(strs[0]), int.Parse(strs[1]), int.Parse(strs[2]));
        }

        public static Vector3Int ToVec3Int(this Vector3 input)
        {
            return new Vector3Int(input.x.ToInt(), input.y.ToInt(), input.z.ToInt());
        }

        public static string ToIntStr(this Vector3 vec, string splitChar)
        {
            return (int)vec.x + splitChar + (int)vec.y + splitChar + (int)vec.z;
        }

        public static string ToStr(this Vector3 vec, string splitChar)
        {
            return vec.x + splitChar + vec.y + splitChar + vec.z;
        }

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
    }
}
