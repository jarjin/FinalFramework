using FirClient.Component.FSM;

namespace FirClient.Logic.FSM.TurnBaseState
{
    public class TIdleState : FsmState
    {
        private NpcFSM npcFsm;

        public override void Enter()
        {
            base.Enter();
            npcFsm = (NpcFSM)Machine;
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
