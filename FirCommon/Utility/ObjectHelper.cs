using System;
using System.Collections.Generic;
using System.Linq;

namespace FirCommon.Utility
{
    public static class ObjectHelper
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
    }
}
