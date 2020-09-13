using System;

namespace FirServer.Define
{
    public static class AppConst
    {
        public const string AppName = "FirSango";
        public const int FrameCount = 33;
        public const bool DebugMode = true;
        public const bool RedisMode = true;
        public const bool MySQLMode = true;

        #if DEBUG
            public const string MySQL_Host = "127.0.0.1";
            public const string MySQL_DB = "gamedb";
            public const string MySQL_User = "root";
            public const string MySQL_Pass = "P@ssw0rd";

            public const string Redis_Host = "127.0.0.1";
            public const int Redis_Port = 6379;
            public const string Redis_Pass = "";
        #else
            public const string MySQL_Host = "rm-uf65lfa08zlfqb1fx1o.mysql.rds.aliyuncs.com";
            public const string MySQL_DB = "gamedb";
            public const string MySQL_User = "dbuser";
            public const string MySQL_Pass = "xHWWTMJZp4&t6waZw";

            public const string Redis_Host = "127.0.0.1";
            public const int Redis_Port = 6379;
            public const string Redis_Pass = "jar510@Lee";
        #endif




        public const string adminUser = "fbadmin";
        public const string adminPass = "fbpass888";
        public const string loginKey = "fb-api-login-user";
        public const string loginPass = "FB-admin-888-cooking";
        public const string appid = "wx51aa7a5c4aca6a3e";
        public const string secret = "95babc90b54d249c70e9653819a50f07";
    }
}
