using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
     public static class StringHelper
    {
        public static string NotNull(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return str;
        }

        public static string[] SplitTrim(this string str, string splitor)
        {
            if (string.IsNullOrEmpty(str))
                return new string[0];

            return str.Split(new string[] { splitor }, StringSplitOptions.RemoveEmptyEntries);
        }

        // public static Dictionary<T, V> ToDictionary<T, V>(this string str, string splitor1, string splitor2)
        // {
        //     var result = new Dictionary<T, V>();
        //     var data = str.SplitTrim(splitor1);
        //
        //     for (int i = 0; i < data.Length; i++)
        //     {
        //         var pair = data[i].SplitTrim(splitor2);
        //         if (pair != null && pair.Length == 2)
        //             result.Renew((T)Convert.ChangeType(pair[0], typeof(T)), (V)Convert.ChangeType(pair[1], typeof(V)));
        //     }
        //
        //     return result;
        // }

        public static byte[] ToBase64(this string str)
        {
            string corrected = str.Replace('-', '+').Replace('_', '/');
            return Convert.FromBase64String(corrected);
        }

        #region Has

        public static bool Has(this string str, int id)
        {
            return str.Has(id, 1);
        }

        /// <param name="bit">占用位数</param>
        public static bool Has(this string str, int id, int bit)
        {
            var index = (id - 1) * bit;

            if (string.IsNullOrEmpty(str))
                return false;

            if (str.Length <= index)
                return false;

            // 没找到"-"，即代表有数据
            return (str.IndexOf('-', index, bit) < 0);
        }

        #endregion

        #region Get

        public static string Get(this string str, int id)
        {
            return str.Get(id, 1);
        }

        public static string Get(this string str, int id, int bit)
        {
            var index = (id - 1) * bit;

            if (str.Length < (index + bit))
                return string.Empty;

            return str.Substring(index, bit);
        }

        #endregion

        #region Set

        public static string Set(this string str, int id)
        {
            return str.Set(id, "1");
        }

        public static string Set(this string str, int id, string data)
        {
            if (string.IsNullOrEmpty(str))
                str = "";

            var index = (id - 1) * data.Length;

            // 如果不存在，则扩展
            if (str.Length <= index)
            {
                str = str.PadRight(index, '-');
                str = str.Insert(index, data);
            }
            else
            {
                str = str.Remove(index, data.Length);
                str = str.Insert(index, data);
            }

            return str;
        }

        #endregion

        public static bool Any(this string list, string[] keywords)
        {
            var source = list.SplitTrim(",");

            foreach (var key in keywords)
            {
                if (source.Any(s => s == key))
                    return true;
            }

            return false;
        }

        public static string Cut(this StringBuilder sb, string cutStr = ",")
        {
            string value = sb.ToString();

            if (!string.IsNullOrEmpty(value))
            {
                if (value.EndsWith(cutStr))
                    value = value.Substring(0, value.Length - 1);
            }

            return value;
        }

        public static string SetPadLeft(this string str, int totalWidth, char c = ' ')
        {
            var length = str.GetLength();
            var number = MathHelper.DivideMore(Math.Max(0, totalWidth - length), 2);

            return "".PadLeft(number, c) + str;
        }

        public static string SetPadRight(this string str, int totalWidth, char c = ' ')
        {
            var length = str.GetLength();
            var number = MathHelper.DivideMore(Math.Max(0, totalWidth - length), 2);

            return str + "".PadRight(number, c);
        }

        public static int GetLength(this string str)
        {
            return Encoding.GetEncoding(54936).GetByteCount(str);
        }

        /// <summary>
        /// 切合字符串
        /// </summary>
        /// <param name="str">"uc|asdfadfuc|dfasfdf"
        /// <param name="c">"|"</param>
        /// <returns>"asdfadfuc|dfasfdf"</returns>
        /// <remarks>
        ///     "aaa"=>"aaa"
        ///     "fdafd|"=>""
        ///     "|"=>""
        /// </remarks>
        public static string Cut(this string str, string c)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            var idx = str.IndexOf(c);
            if (idx < 0)
                return str;

            if (idx >= str.Length)
                return string.Empty;

            return str.Substring(idx + 1, str.Length - idx - 1);
        }

        #region 十六进制转换

        /// <summary>
        /// 获取十六进制字符串
        /// </summary>
        public static string Hex(string s)
        {
            var b = Encoding.UTF8.GetBytes(s);//按照指定编码将string编程字节数组
            var sb = new StringBuilder();

            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                sb.Append("%").Append(Convert.ToString(b[i], 16));
            }

            return sb.ToString();
        }

        public static string UnHex(string hs)
        {
            //以%分割字符串，并去掉空字符
            var chars = hs.SplitTrim("%");
            var b = new byte[chars.Length];

            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }

            //按照指定编码将字节数组变为字符串
            return Encoding.UTF8.GetString(b);
        }

        #endregion

        /// <summary>
        /// 判断字符串是否符合预期顺序
        /// </summary>
        public static bool IsMatchSequace(this List<string> strList, List<string> requireSequance)
        {
            int i = 0, j = 0, lenR = requireSequance.Count, lenS = strList.Count;
            bool complete = false;

            for (; i < lenR; )
            {
                if (i == lenR || complete)
                    break;

                var rStr = requireSequance[i];

                for (; j < lenS; j++)
                {
                    var sStr = strList[j];

                    if (rStr == sStr)
                    {
                        i++;
                        break;
                    }

                    if (j == lenS - 1)
                        complete = true;
                }
            }

            return i == lenR;
        }

        ///// <summary>
        ///// 获取正则表达式匹配到的字符串
        ///// 匹配失败则返回string.Empty
        ///// </summary>
        //public static string GetStringFromRegexMatch(string value, string pattern, int groupIndex = 1)
        //{
        //    var match = Regex.Match(value, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //    return (match.Success) ? match.Groups[groupIndex].Value : string.Empty;
        //}

        public static bool Contains(this string str, string toCheck, StringComparison comparison)
        {
            return str.IndexOf(toCheck, comparison) >= 0;
        }

        public static string TakeStr(this string str, int len)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (str.Length <= len)
                return str;

            return str.Substring(0, len);
        }
    }
}