using FirServer.Define;
using log4net;

namespace FirServer.Manager
{
    /// <summary>
    /// 基于管理器集中式管理，缺了输出类显示，根据习惯来用即可。
    /// </summary>
    public class LoggerManager : BaseManager
    {
        private static readonly ILog logger = LogManager.GetLogger(AppConst.LogRepos?.Name, typeof(LoggerManager));

        public override void Initialize()
        {
        }

        public void Log(string str) 
        {
            logger.Info(str);
        }

        public void LogWarn(string str) 
        {
            logger.Warn(str);
        }

        public void LogError(string str) 
        {
            logger.Error(str);
        }

        public override void OnDispose()
        {
        }
    }
}
