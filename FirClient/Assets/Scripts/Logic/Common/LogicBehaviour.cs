using FirClient.Component;
using FirClient.Logic.FSM;
using FirClient.Logic.Manager;
using FirCommon.Data;
using System.Collections.Generic;

namespace FirClient.Logic
{
    public class LogicBehaviour : ILogicObject
    {
        static Dictionary<string, LogicBehaviour> logicManagers = new Dictionary<string, LogicBehaviour>();

        private static LogicManager _logicMgr;
        public static LogicManager logicMgr
        {
            get
            {
                if (_logicMgr == null)
                {
                    _logicMgr = GetManager<LogicManager>();
                }
                return _logicMgr;
            }
        }


        private static TableManager _tableMgr;
        protected static TableManager tableMgr
        {
            get
            {
                if (_tableMgr == null)
                {
                    _tableMgr = TableManager.Create();
                }
                return _tableMgr;
            }
        }

        private static ConfigManager _configMgr;
        protected static ConfigManager configMgr
        {
            get
            {
                if (_configMgr == null)
                {
                    _configMgr = ConfigManager.Create();
                }
                return _configMgr;
            }
        }

        private static EventManager _eventMgr;
        public static EventManager eventMgr
        {
            get
            {
                if (_eventMgr == null)
                {
                    _eventMgr = GetManager<EventManager>();
                }
                return _eventMgr;
            }
        }

        private static NPCDataManager _npcDataMgr;
        public static NPCDataManager npcDataMgr
        {
            get
            {
                if (_npcDataMgr == null)
                {
                    _npcDataMgr = GetManager<NPCDataManager>();
                }
                return _npcDataMgr;
            }
        }

        private static CTimer _timerMgr;
        public static CTimer timerMgr
        {
            get
            {
                if (_timerMgr == null)
                {
                    _timerMgr = CTimer.Create();
                }
                return _timerMgr;
            }
        }

        private static BattleLogicManager _battleLogicMgr;
        public static BattleLogicManager battleLogicMgr
        {
            get
            {
                if (_battleLogicMgr == null)
                {
                    _battleLogicMgr = GetManager<BattleLogicManager>();
                }
                return _battleLogicMgr;
            }
        }

        private static EventMappingManager _evMappingMgr;
        public static EventMappingManager evMappingMgr
        {
            get
            {
                if (_evMappingMgr == null)
                {
                    _evMappingMgr = GetManager<EventMappingManager>();
                }
                return _evMappingMgr;
            }
        }

        private static BattleHandlerManager _battleHandlerMgr;
        public static BattleHandlerManager battleHandlerMgr
        {
            get
            {
                if (_battleHandlerMgr == null)
                {
                    _battleHandlerMgr = GetManager<BattleHandlerManager>();
                }
                return _battleHandlerMgr;
            }
        }

        private static EmbattlePosManager _embattlePosMgr;
        public static EmbattlePosManager embattlePosMgr
        {
            get
            {
                if (_embattlePosMgr == null)
                {
                    _embattlePosMgr = GetManager<EmbattlePosManager>();
                }
                return _embattlePosMgr;
            }
        }

        /// <summary>
        /// 战斗有限状态机
        /// </summary>
        private static BattleFSM _battleFsm;
        protected static BattleFSM battleFsm
        {
            get
            {
                return _battleFsm;
            }
            set
            {
                _battleFsm = value;
            }
        }

        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 添加管理器
        /// </summary>
        protected static T AddManager<T>() where T : LogicBehaviour, new()
        {
            var type = typeof(T);
            var obj = new T();
            logicManagers.Add(type.Name, obj);
            return obj;
        }

        /// <summary>
        /// 添加管理器
        /// </summary>
        protected static void AddManager<T>(LogicBehaviour obj) where T : LogicBehaviour
        {
            var type = typeof(T);
            logicManagers.Add(type.Name, obj);
        }

        /// <summary>
        /// 获取管理器
        /// </summary>
        public static T GetManager<T>() where T : class
        {
            var type = typeof(T);
            if (!logicManagers.ContainsKey(type.Name))
            {
                return null;
            }
            return logicManagers[type.Name] as T;
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }

        public virtual void OnDispose()
        {
        }
    }
}
