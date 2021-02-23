using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FirCommon.Utility
{
    public static class AppUtil
    {
        public static string CurrDirectory
        {
            get
            {
                var CurrDir = Environment.CurrentDirectory;
                if (CurrDir.Contains("netcoreapp"))
                {
                    CurrDir = Directory.GetParent(@"../../../../").FullName;
                }
                CurrDir = CurrDir.Replace('\\', '/') + "/";
                return CurrDir;
            }
        }

        public static int Random(int min, int max)
        {
            return new Random().Next(min, max);
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
    }
}
