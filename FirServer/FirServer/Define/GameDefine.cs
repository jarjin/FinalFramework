using System.Collections.Generic;
using System.Net.WebSockets;
using FirServer.Interface;

namespace FirServer.Define
{
    public class GlobalConfig
    {
        public string name { get; set; }
        public Percent percent { get; set; }
        public Percent takecashPoundage { get; set; }
        public float failAmount { get; set; }
        public Dictionary<uint, Game> gameList = new Dictionary<uint, Game>();
    }

    public class Percent
    {
        public float baseValue;
        public float maxValue;
    }

    public class BulletData
    {
        public ushort id;
        public int type;
        public int damage;
    }

    public class Game
    {
        public uint id { get; set; }
        public string name { get; set; }
        public string libName { get; set; }
        public string mainClass { get; set; }
    }

    public class User
    {
        public long uid { get; set; }
        public string name { get; set; }
        public WebSocket socket { get; private set; }
        public IObject paramObject;

        public User(WebSocket socket)
        {
            this.socket = socket;
        }
    }

    public enum MessageType
    {
        Text = 0,
        Json = 1,
        Binrary = 2
    }

    public class Message
    {
        public ushort CommandId { get; set; }
        public MessageType MessageType { get; set; }
        public string Data { get; set; }
    }
    
    public class DatabaseConfig
    {
        public string Tag;
        public string IP;
        public int Port;
        public string Username;
        public string Password;
        public string Database;
        public string CharSet;
        public int DefaultCommandTimeout;
        // public string SSLMode;
        public DatabaseConfig Clone()
        {
            var newConfig = new DatabaseConfig
            {
                Tag = this.Tag,
                IP = this.IP,
                Port = this.Port,
                Username = this.Username,
                Password = this.Password,
                Database = this.Database,
                CharSet = this.CharSet,
                DefaultCommandTimeout = this.DefaultCommandTimeout,
            };

            return newConfig;
        }

        public string ToLog()
        {
            return $"{Tag},{IP},{Port},{Username},{Password},{Database},{CharSet},{DefaultCommandTimeout}";
        }
    }

    public static class DatabaseConfigHelper
    {
        public static DatabaseConfig Get(this List<DatabaseConfig> configList, string tag)
        {
            return configList.Find(c => c.Tag == tag);
        }
    }
}

