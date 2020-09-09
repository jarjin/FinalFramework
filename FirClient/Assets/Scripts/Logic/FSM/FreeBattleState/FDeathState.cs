using FirClient.Component.FSM;
using FirClient.Data;
using UnityEngine;

namespace FirClient.Logic.FSM.FreeBattleState
{
    public class FDeathState : FsmState
    {
        private NPCData myNpcData;
        private FsmVar<long> mynpcId;
        private NpcFSM npcFsm;

        public override void Initialize()
        {
            base.Initialize();
            npcFsm = (NpcFSM)Machine;
        }

        public override void Enter()
        {
            base.Enter();
            mynpcId = npcFsm.GetVar<long>("mynpcId");
            myNpcData = npcDataMgr.GetNpcData(mynpcId.value);
            myNpcData.npcState = NpcState.Death;
            GLogger.Red(mynpcId.value + " enter Death...");

            DoNpcDeath();
        }

        void DoNpcDeath()
        {
            myNpcData.fsm.RemoveAllStates();
            myNpcData.fsm = null;
            npcDataMgr.RemoveNpcData(mynpcId.value);
            battleLogicMgr.NpcDeath(mynpcId.value);
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