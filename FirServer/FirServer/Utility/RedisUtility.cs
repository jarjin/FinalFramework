using System;
using FirServer.Defines;

namespace FirServer.Utility
{
    public class RedisUtility
    {
        // private static ConnectionMultiplexer redis;
        // private static IDatabase db = null;

        public static void Initialize()
        {
            // if (AppConst.RedisMode)
            // {
            //     ConfigurationOptions option = new ConfigurationOptions
            //     {
            //         Password = AppConst.Redis_Pass,
            //         AbortOnConnectFail = false,
            //         EndPoints = { { AppConst.Redis_Host, AppConst.Redis_Port } }
            //     };
            //     redis = ConnectionMultiplexer.Connect(option);
            //     db = redis.GetDatabase();
            // }
        }

        public static bool StringSet(string key, string value, int seconds)
        {
            // if (db == null)
            // {
            //     return false;
            // }
            // return db.StringSet(key, value, TimeSpan.FromSeconds(seconds));
            return true;
        }

        public static string StringGet(string key)
        {
            // if (db == null)
            // {
            //     return null;
            // }
            // return db.StringGet(key);
            return "";
        }

        public static bool KeyDelete(string key)
        {
            // if (db == null)
            // {
            //     return false;
            // }
            // return db.KeyDelete(key);
            return true;
        }

        public static bool KeyExist(string key)
        {
            // if (db == null)
            // {
            //     return false;
            // }
            // return db.KeyExists(key);
            return true;
        }

        public static void Close()
        {
            //redis.Close();
        }
    }
}
