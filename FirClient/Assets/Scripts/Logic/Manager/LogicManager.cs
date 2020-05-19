using FirClient.Define;
using FirClient.Logic.AI;
using FirClient.Extensions;

namespace FirClient.Logic.Manager
{
    public class LogicManager : LogicBehaviour
    {
        public LogicManager(params object[] vars)
        {
            LogicConst.Viewport_Margin = vars[0].ToFloat();     //视口边距
        }

        public override void Initialize()
        {
            base.Initialize();

            AddManager<EventManager>();
            AddManager<NPCDataManager>();
            AddManager<BattleLogicManager>();
            AddManager<EventMappingManager>();
            AddManager<BattleTurnManager>();
            AddManager<EmbattlePosManager>();
            AddManager<LogicManager>(this);

            battleLogicMgr.Initialize();
            Messenger.AddListener<float>(EventNames.EvLogicUpdate, OnUpdate);
        }

        public void InitBattleFsm()
        {
            this.CloseBattleFsm();
            battleFsm = new BattleFSM();
            battleFsm.Initialize();
        }

        public void CloseBattleFsm()
        {
            if (battleFsm != null)
            {
                battleFsm.RemoveAllStates();
                battleFsm = null;
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            npcDataMgr.OnUpdate(deltaTime);

            if (battleFsm != null)
            {
                battleFsm.OnUpdate(deltaTime);
            }
        }

        public override void OnDispose()
        {
            Messenger.RemoveListener<float>(EventNames.EvLogicUpdate, timerMgr.OnUpdate);
        }
    }
}

