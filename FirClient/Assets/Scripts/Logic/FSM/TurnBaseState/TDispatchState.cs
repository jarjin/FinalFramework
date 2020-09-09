using FirClient.Component.FSM;
using FirClient.Data;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Logic.FSM.TurnBaseState
{
    public class TDispatchState : FsmState
    {
        private BattleFSM battleFsm;

        private FsmVar<long> tokenNpcId;
        private FsmVar<bool> isTakeNewToken;

        private List<long> takedTokenNpcIds;

        public override void Initialize()
        {
            base.Initialize();
            battleFsm = (BattleFSM)Machine;
            takedTokenNpcIds = new List<long>();
        }

        public override void Enter()
        {
            base.Enter();
            tokenNpcId = battleFsm.GetGlobalVar<long>("tokenNpcId");
            isTakeNewToken = battleFsm.GetGlobalVar<bool>("isTakeNewToken");
        }

        public override void Execute()
        {
            base.Execute();
            if (isTakeNewToken != null && isTakeNewToken.value)
            {
                isTakeNewToken.value = false;
                DispatchNpcToken();
            }
        }

        /// <summary>
        /// 派发NPC的TOKEN
        /// </summary>
        private void DispatchNpcToken()
        {
            var enemyDatas = npcDataMgr.GetNpcDatas(NpcType.Enemy);
            if (enemyDatas.Count == 0)
            {
                battleHandlerMgr.MoveNextTurn();
                return;
            }
            var heroDatas = npcDataMgr.GetNpcDatas(NpcType.Hero);
            if (heroDatas.Count == 0)
            {
                battleLogicMgr.BattleEnd();  //挑战失败
                return;
            }
            long newTokenNpcId = 0;
            newTokenNpcId = GetNewTokenNpcId(heroDatas);
            if (newTokenNpcId == 0)
            {
                newTokenNpcId = GetNewTokenNpcId(enemyDatas);
            }
            if (newTokenNpcId == 0)
            {
                takedTokenNpcIds.Clear();
                DispatchNpcToken();
                return;
            }
            if (tokenNpcId != null)
            {
                tokenNpcId.value = newTokenNpcId;
            }
            GLogger.Purple("DispatchNpcToken----------------->>>>" + newTokenNpcId);
        }

        /// <summary>
        /// 获取新的TokenNpc
        /// </summary>
        private long GetNewTokenNpcId(List<NPCData> npcDatas)
        {
            for (int i = 0; i < npcDatas.Count; i++)
            {
                var currNpcId = npcDatas[i].npcid;
                if (!takedTokenNpcIds.Contains(currNpcId))
                {
                    takedTokenNpcIds.Add(currNpcId);
                    return currNpcId;
                }
            }
            return 0;
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}