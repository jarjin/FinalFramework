using System;
using System.Linq;
using UnityEngine;

namespace TableTool
{
    enum TableType
    {
        Lua = 0,
        CSharp = 1,
        Server = 2,
    }

    public enum TableFormat
    {
        None = -1,
        CSharp = 0,
        Lua = 1,
    }

    public class TableData : ICloneable
    {
        public string fileName;
        public string md5value;
        public TableFormat format;
        public bool withServer = false;

        public TableData() { }

        public TableData(string data)
        {
            var strs = data.Split('|');
            fileName = strs[0].Trim();
            md5value = strs[1].Trim();
            format = (TableFormat)(int.Parse(strs[2]));
            withServer = strs[3].Trim() == "1" ? true : false;
        }

        public TableData Clone()
        {
            return (TableData)this.MemberwiseClone();
        }

        public override string ToString()
        {
            var ws = withServer ? 1 : 0;
            return string.Format("{0}|{1}|{2}|{3}", fileName, md5value, (int)format, ws);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }

    class TablePath
    {
        public string path;
        public string dllpath;

        public TablePath(string p, string n)
        {
            path = p;
            dllpath = n;
        }
    }

    class TableCompileInfo
    {
        public string tableName;
        public string tablePath;
        public TableType tableType;
        public string sheetName;
        public string tableCode;
    }

    public class ClassInfo
    {
        public string namespaceName;
        public string typeName;
    }

    static class Common
    {
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

        public static Vector2 ToVec2(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return new Vector2(0, 0);
            string[] strs = input.Split(splitChar);
            return new Vector2(float.Parse(strs[0]), float.Parse(strs[1]));
        }

        public static string ToLuaVec2(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string[] strs = input.Split(splitChar);
            return string.Format("Vector2.New({0}, {1})", strs[0], strs[1]);
        }

        public static Vector3 ToVec3(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return new Vector3(0, 0, 0);
            string[] strs = input.Split(splitChar);
            return new Vector3(float.Parse(strs[0]), float.Parse(strs[1]), float.Parse(strs[2]));
        }

        public static string ToLuaVec3(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string[] strs = input.Split(splitChar);
            return string.Format("Vector3.New({0}, {1}, {2})", strs[0], strs[1], strs[2]);
        }

        public static Color ToColor(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return new Color(0, 0, 0, 0);
            string[] strs = input.Split(splitChar);
            return new Color(float.Parse(strs[0]), float.Parse(strs[1]), float.Parse(strs[2]), float.Parse(strs[3]));
        }

        public static string ToLuaColor(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string[] strs = input.Split(splitChar);
            return string.Format("Color.New({0}, {1}, {2}, {3})", strs[0], strs[1], strs[2], strs[3]);
        }

        public static Color32 ToColor32(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return new Color32(0, 0, 0, 0);
            string[] strs = input.Split(splitChar);
            return new Color32(byte.Parse(strs[0]), byte.Parse(strs[1]), byte.Parse(strs[2]), byte.Parse(strs[3]));
        }

        public static string ToLuaColor32(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string[] strs = input.Split(splitChar);
            return string.Format("Color32.New({0}, {1}, {2}, {3})", strs[0], strs[1], strs[2], strs[3]);
        }

        public static int[] ToIntArray(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string[] strs = input.Split(splitChar);
            int[] c = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                c[i] = int.Parse(strs[i]);
            }
            return c;
        }

        public static uint[] ToUIntArray(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string[] strs = input.Split(splitChar);
            uint[] c = new uint[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                c[i] = uint.Parse(strs[i]);
            }
            return c;
        }

        public static float[] ToFloatArray(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string[] strs = input.Split(splitChar);
            float[] c = new float[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                c[i] = float.Parse(strs[i]);
            }
            return c;
        }

        public static long[] ToLongArray(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string[] strs = input.Split(splitChar);
            long[] c = new long[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                c[i] = long.Parse(strs[i]);
            }
            return c;
        }

        public static string ToLuaArray(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return "{}";
            string[] strs = input.Split(splitChar);
            string c = "{";
            for (int i = 0; i < strs.Length; i++)
            {
                c += strs[i] + ", ";
            }
            c = c.TrimEnd(',', ' ') + "}";
            return c;
        }

        public static string ToLuaStringArray(this string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input)) return "{}";
            string[] strs = input.Split(splitChar);
            string c = "{";
            for (int i = 0; i < strs.Length; i++)
            {
                c += "'" + strs[i] + "', ";
            }
            c = c.TrimEnd(',', ' ') + "}";
            return c;
        }
    }
}
