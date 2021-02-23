using System.Security;
using FirCommon.Utility;
using FirServer.Define;

namespace FirServer.Manager
{
    public class ConfigManager : BaseManager
    {
        private static GlobalConfig globalConfig = new GlobalConfig();
        private static DatabaseConfig _databaseConfig = new DatabaseConfig();

        public override void Initialize()
        {
            LoadGlobalConfig();
        }

        void LoadGlobalConfig()
        {
            var xml = XmlHelper.LoadXml("config/game.xml");
            if (xml != null)
            {
                var count = xml.Children.Count;
                for(int i = 0; i < count; i++)
                {
                    var node = xml.Children[i] as SecurityElement;
                    if (node != null)
                    {
                        switch (node.Tag)
                        {
                            case "global": ParseGlobal(node); break;
                            case "games": ParseGames(node); break;
                            case "gamedata" : ParseGameDatabase(node); break;
                        }
                    }
                }
            }
        }

        public GlobalConfig GetGlobalConfig()
        {
            return globalConfig;
        }

        public DatabaseConfig GetDatabaseConfig()
        {
            return _databaseConfig;
        }

        /// <summary>
        /// 分析全局属性
        /// </summary>
        /// <param name="node"></param>
        void ParseGlobal(SecurityElement node)
        {
            if (node != null)
            {
                globalConfig.name = node.Attributes["name"].ToString();
                globalConfig.percent = GetPercent(node.Attributes["percent"].ToString());
                globalConfig.takecashPoundage = GetPercent(node.Attributes["takecashPoundage"].ToString());
                globalConfig.failAmount = float.Parse(node.Attributes["failAmount"].ToString());
            }
        }

        Percent GetPercent(string str)
        {
            Percent obj = null;
            if (!string.IsNullOrEmpty(str))
            {
                obj = new Percent();
                var strs = str.Split(':');
                obj.baseValue = float.Parse(strs[0]);
                obj.maxValue = float.Parse(strs[1]);
            }
            return obj;
        }

        /// <summary>
        /// 分析大厅属性
        /// </summary>
        /// <param name="node"></param>
        private void ParseGames(SecurityElement node)
        {
            if (node != null)
            {
                var nodes = node.Children;
                for(int i = 0; i < nodes.Count; i++)
                {
                    var obj = nodes[i] as SecurityElement;
                    var game = new Game();
                    game.id = uint.Parse(obj.Attribute("id"));
                    game.name = obj.Attribute("name");
                    game.libName = obj.Attribute("libname");
                    game.mainClass = obj.Attribute("mainclass");
                    globalConfig.gameList.Add(game.id, game);
                }
            }
        }
        
        private void ParseGameDatabase(SecurityElement node)
        {
            if (node != null)
            {
                var nodes = node.Children;
                for(var i = 0; i < nodes.Count; i++)
                {
                    var obj = nodes[i] as SecurityElement;
                    var database = new DatabaseConfig();
                    database.Tag = obj.Attribute("tag");
                    database.IP = obj.Attribute("ip");
                    database.Port = int.Parse(obj.Attribute("port"));
                    database.Database = obj.Attribute("database");
                    database.Username = obj.Attribute("username");
                    database.Password = obj.Attribute("password");
                    database.CharSet = obj.Attribute("charset");
                    database.DefaultCommandTimeout = int.Parse(obj.Attribute("defaultcommandtimeout"));
                    _databaseConfig = database;
                }
            }
        }
    }
}
