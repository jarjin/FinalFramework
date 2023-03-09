using FirServer.Interface;
using FirServer.Manager;

namespace FirServer.Common
{
    public class BaseBehaviour : IObject
    {
        private static ConfigManager? _configMgr = null;
        protected static ConfigManager? configMgr
        {
            get
            {
                if (_configMgr == null)
                {
                    _configMgr = ManagementCenter.GetManager<ConfigManager>();
                }
                return _configMgr;
            }
        }

        private static AssemblyManager? _assemblyMgr = null;
        protected static AssemblyManager? assemblyMgr
        {
            get
            {
                if (_assemblyMgr == null)
                {
                    _assemblyMgr = ManagementCenter.GetManager<AssemblyManager>();
                }
                return _assemblyMgr;
            }
        }

        private static NetworkManager? _netMgr = null;
        protected static NetworkManager? netMgr
        {
            get
            {
                if (_netMgr == null)
                {
                    _netMgr = ManagementCenter.GetManager<NetworkManager>();
                }
                return _netMgr;
            }
        }

        private static HandlerManager? _handlerMgr = null;
        protected static HandlerManager? handlerMgr
        {
            get
            {
                if (_handlerMgr == null)
                {
                    _handlerMgr = ManagementCenter.GetManager<HandlerManager>();
                }
                return _handlerMgr;
            }
        }

        private static WorldManager? _worldMgr = null;
        protected static WorldManager? worldMgr
        {
            get
            {
                if (_worldMgr == null)
                {
                    _worldMgr = ManagementCenter.GetManager<WorldManager>();
                }
                return _worldMgr;
            }
        }

        private static LoggerManager? _loggerMgr = null;
        protected static LoggerManager? loggerMgr
        {
            get
            {
                if (_loggerMgr == null)
                {
                    _loggerMgr = ManagementCenter.GetManager<LoggerManager>();
                }
                return _loggerMgr;
            }
        }

        private static DataManager? _dataMgr = null;
        protected static DataManager? dataMgr
        {
            get
            {
                if (_dataMgr == null)
                {
                    _dataMgr = ManagementCenter.GetManager<DataManager>();
                }
                return _dataMgr;
            }
        }

        private static TimerManager? _timerMgr = null;
        protected static TimerManager? timerMgr
        {
            get
            {
                if (_timerMgr == null)
                {
                    _timerMgr = ManagementCenter.GetManager<TimerManager>();
                }
                return _timerMgr;
            }
        }

        private static ModelManager? _modelMgr = null;
        protected static ModelManager? modelMgr
        {
            get
            {
                if (_modelMgr == null)
                {
                    _modelMgr = ManagementCenter.GetManager<ModelManager>();
                }
                return _modelMgr;
            }
        }
    }
}
