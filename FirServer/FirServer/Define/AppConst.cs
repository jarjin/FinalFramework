using log4net.Repository;

namespace FirServer.Define
{
    public static class AppConst
    {
        public const string LibName = "Testlib";
        public static ILoggerRepository? LogRepos;
        public const string RepositoryName = "NETCoreRepository";
        public const string Log4jConfig = "log4net.config";

        public const string GameCfgPath = "config/game.xml";
        public const string XmlGlobalNode = "global";
        public const string XmlGameNode = "game";
    }
}
