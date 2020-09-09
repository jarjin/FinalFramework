using FirClient.Component.FSM;
using FirClient.Data;
using UnityEngine;

namespace FirClient.Logic.FSM.FreeBattleState
{
    public class FIdleState : FsmState
    {
        private NpcFSM npcFsm;
        private FsmVar<long> mynpcId;
        private NPCData myNpcData;

        public override void Enter()
        {
            base.Enter();
            npcFsm = (NpcFSM)Machine;

            mynpcId = npcFsm.GetVar<long>("mynpcId");
            myNpcData = npcDataMgr.GetNpcData(mynpcId.value);
            myNpcData.npcState = NpcState.Idle;
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
