using FirClient.Logic.FSM.TurnBaseState;
using FirClient.Logic.FSM.FreeBattleState;
using UnityEngine;
using FirClient.Data;

namespace FirClient.Logic.FSM
{
    public class BattleFSM : GameFSM
    {
        private float updateTime = 0;

        public BattleFSM()
        {
            GLogger.Cyan("Current FSM Type:" + LogicConst.BattleType);
        }

        public override void AddStates()
        {
            SetUpdateFrequency(1f);
            switch(LogicConst.BattleType)
            {
                case BattleType.TurnBase:
                    InitTurnBaseState();
                break;
                case BattleType.FreeBattle:
                    InitFreeBattleState();
                break;
            }
        }

        /// <summary>
        /// 初始化回合制状态
        /// </summary>
        void InitTurnBaseState()
        {
            ///初始化变量
            SetGlobalVar<long>("tokenNpcId", 0);
            SetGlobalVar<bool>("isTakeNewToken", false);

            AddState<TDispatchState>();
            SetInitialState<TDispatchState>();
        }

        /// <summary>
        /// 初始化自由战斗状态
        /// </summary>
        void InitFreeBattleState()
        {
            AddState<FDispatchState>();
            SetInitialState<FDispatchState>();
        }

        /// <summary>
        /// 设置驱动开关
        /// </summary>
        public void SetEnable(bool enable)
        {
            if (enable)
            {
                AutoUpdateNpcData(0);
            }
            bRunning = enable;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!bRunning) return;
            OnExecute(deltaTime);

            updateTime += deltaTime;
            if (updateTime >= 1)
            {
                updateTime = 0;
                AutoUpdateNpcData();
            }
        }

        /// <summary>
        /// 自动更新角色属性
        /// </summary>
        /// <param name="value"></param>
        void AutoUpdateNpcData(int value = -1)
        {
            lock(mLock)
            {
                var npcDatas = npcDataMgr.NpcDatas;
                foreach (var de in npcDatas)
                {
                    if (value == 0)
                    {
                        de.Value.mp = 0;
                    }
                    else
                    {
                        UpdateNpcHP(de.Value);
                        UpdateNpcMP(de.Value);
                    }
                }
            }
        }

        ///自我恢复血量
        void UpdateNpcHP(NPCData npcData)
        {
            npcData.hp += npcData.hpInc;
            if (npcData.hp > npcData.hpMax)
            {
                npcData.hp = npcData.hpMax;
            }
        }

        ///自我恢复魔法值
        void UpdateNpcMP(NPCData npcData)
        {
            npcData.mp += npcData.mpInc;
            if (npcData.mp > npcData.mpMax)
            {
                npcData.mp = npcData.mpMax;
            }
        }

        public void OnDispose()
        {
            updateTime = 0;
            bRunning = false;
        }
    }
}