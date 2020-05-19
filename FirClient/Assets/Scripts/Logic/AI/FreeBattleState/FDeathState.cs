using FirClient.Component.FSM;
using FirClient.Data;

namespace FirClient.Logic.AI.FreeBattleState
{
    public class FDeathState : FsmState
    {
        private NPCData myNpcData;
        private FsmVar<long> mynpcId;

        public override void Enter()
        {
            base.Enter();
            myNpcData = npcDataMgr.GetNpcData(mynpcId.value);
            myNpcData.npcState = NpcState.Death;
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