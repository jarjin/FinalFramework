using UnityEngine;
using System;
using System.Text;

namespace UnityEngine
{
    public static partial class StringBuilderExtensionMethods
    {
        /*到.net4.0 stringbuild 会有Clear 函数，到时可以删掉这个函数*/
        public static void Clear(this StringBuilder sb)
        {
            sb.Length = 0;
        }

        public static void AppendLineEx(this StringBuilder sb, string str = "")
        {
            sb.Append(str).Append("\r\n");
        }        
    }
}


