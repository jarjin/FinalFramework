using FirCommon.Utility;
using FirServer.Define;
using log4net;
using System.Security;

namespace FirServer.Manager
{
    public class ConfigManager : BaseManager
    {
        private static readonly ILog logger = LogManager.GetLogger(AppConst.LogRepos?.Name, typeof(ConfigManager));

        private GlobalConfig _globalConfig = new GlobalConfig();

        public override void Initialize()
        {
            LoadGlobalConfig();
        }

        void LoadGlobalConfig()
        {
            var xml = XmlHelper.LoadXml(AppConst.GameCfgPath);
            if (xml != null)
            {
                var count = xml?.Children?.Count;
                for(int i = 0; i < count; i++)
                {
                    var node = xml?.Children?[i] as SecurityElement;
                    if (node != null)
                    {
                        switch (node.Tag)
                        {
                            case AppConst.XmlGlobalNode: ParseGlobal(node); break;
                            case AppConst.XmlGameNode: ParseGames(node); break;
                        }
                    }
                }
            }
        }

        public GlobalConfig GetGlobalConfig()
        {
            return _globalConfig;
        }

        /// <summary>
        /// 分析全局属性
        /// </summary>
        /// <param name="node"></param>
        void ParseGlobal(SecurityElement node)
        {
            if (node != null)
            {
                _globalConfig._name = node?.Attributes?["name"]?.ToString();
                _globalConfig._percent = node?.Attributes?["percent"]?.ToString();
                _globalConfig._takecashPoundage = node?.Attributes?["takecashPoundage"]?.ToString();
                _globalConfig._failAmount = float.Parse(node?.Attributes?["failAmount"]?.ToString());
            }
        }

        GameInfoPercent? GetPercent(string str)
        {
            GameInfoPercent? obj = null;
            if (!string.IsNullOrEmpty(str))
            {
                obj = new GameInfoPercent();
                var strs = str.Split(':');
                obj._baseValue = float.Parse(strs[0]);
                obj._maxValue = float.Parse(strs[1]);
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
                for(int i = 0; i < nodes?.Count; i++)
                {
                    var obj = nodes[i] as SecurityElement;
                    var game = new GameConfigInfo();
                    game._id = uint.Parse(obj?.Attribute("id"));
                    game._name = obj.Attribute("name");
                    game._libName = obj.Attribute("libname");
                    game._mainClass = obj.Attribute("mainclass");
                    game._version = obj.Attribute("version");
                    _globalConfig._gameConfigList.Add(game._id, game);
                }
            }
        }

        public override void OnDispose()
        {
        }
    }
}
