using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Utility
{
    public static class AppUtil
    {
        public static string CurrDirectory
        {
            get
            {
                var CurrDir = string.Empty;
#if DEBUG_MODE
                CurrDir = Directory.GetParent(@"../../../../").FullName;
#else
                CurrDir = Environment.CurrentDirectory;
#endif
                return CurrDir.Replace('\\', '/') + "/";
            }
        }

        public static int Random(int min, int max)
        {
            var ran = new System.Random();
            return ran.Next(min, max);
        }

        /// <summary>
        /// 产生UID
        /// </summary>
        /// <returns></returns>
        public static long NewGuidId()
        {
            var buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
        
        ///等待一帧
        public static async Task Waitforms(int ms) 
        {
            await Task.Run(() =>                                          
            {
                Thread.Sleep(ms); 
            });
        }

        //操作符转换
        public static string TransferOperand(this ExpressionType type)
        {
            string operand = string.Empty;
            switch (type)
            {
                case ExpressionType.AndAlso:
                    operand = "AND";
                    break;
                case ExpressionType.OrElse:
                    operand = "OR";
                    break;
                case ExpressionType.Equal:
                    operand = "=";
                    break;
                case ExpressionType.NotEqual:
                    operand = "<>";
                    break;
                case ExpressionType.LessThan:
                    operand = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    operand = "<=";
                    break;
                case ExpressionType.GreaterThan:
                    operand = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    operand = ">=";
                    break;
            }
            return operand;
        }
    }
}
