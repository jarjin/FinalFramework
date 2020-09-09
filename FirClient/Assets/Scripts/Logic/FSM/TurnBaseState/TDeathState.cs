using FirClient.Component.FSM;
using FirClient.Data;
using UnityEngine;

namespace FirClient.Logic.FSM.TurnBaseState
{
    public class TDeathState : FsmState
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
            GLogger.Red(mynpcId.value + " enter Death...");

            DoNpcDeath();
        }

        private void DoNpcDeath()
        {
            myNpcData.fsm.RemoveAllStates();
            myNpcData.fsm = null;
            npcDataMgr.RemoveNpcData(mynpcId.value);

            var npcType = myNpcData.npcType;
            battleLogicMgr.NpcDeath(mynpcId.value);

            var emType = npcType == NpcType.Hero ? EmbattleType.Left : EmbattleType.Right;
            embattlePosMgr.PushbackItem(emType, myNpcData.position);
        }

        public override void Execute()
        {
            base.Execute();
            Debug.Log("DeathState.Execute");
        }

        public override void Exit()
        {
            base.Exit();
            Debug.LogError("DeathState.Exit");
        }
    }
}