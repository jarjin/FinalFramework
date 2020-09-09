using FirClient.Component.FSM;
using FirClient.Data;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Logic.FSM.TurnBaseState
{
    public class TSearchState : FsmState
    {
        private NpcFSM npcFsm;
        private FsmVar<long> mynpcId;
        private FsmVar<long> targetId;
        private FsmVar<long> tokenNpcId;

        public override void Initialize()
        {
            base.Initialize();
            npcFsm = (NpcFSM)Machine;
        }

        public override void Enter()
        {
            base.Enter();
            mynpcId = npcFsm.GetVar<long>("mynpcId");
            targetId = npcFsm.GetVar<long>("targetId");
            tokenNpcId = npcFsm.GetGlobalVar<long>("tokenNpcId");
        }

        public override void Execute()
        {
            base.Execute();

            if (mynpcId == null || tokenNpcId == null)
            {
                return;
            }
            if (mynpcId.value == tokenNpcId.value)
            {
                SelectTarget();     //当前拥有TOKEN
            }
        }

        /// <summary>
        /// 选择对象
        /// </summary>
        private void SelectTarget()
        {
            List<NPCData> npcDatas = null;
            var myData = npcDataMgr.GetNpcData(mynpcId.value);
            if (myData.npcType == NpcType.Hero)
            {
                npcDatas = npcDataMgr.GetNpcDatas(NpcType.Enemy);
            }
            else
            {
                npcDatas = npcDataMgr.GetNpcDatas(NpcType.Hero);
            }
            if (npcDatas != null)
            {
                var index = Random.Range(0, npcDatas.Count);
                if (npcDatas[index] != null)
                {
                    targetId.value = npcDatas[index].npcid;
                    npcFsm.ChangeState<TAttackState>();
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}