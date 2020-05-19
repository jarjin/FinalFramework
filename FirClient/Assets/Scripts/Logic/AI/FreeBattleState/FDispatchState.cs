using FirClient.Component.FSM;

namespace FirClient.Logic.AI.FreeBattleState
{
    public class FDispatchState : FsmState
    {
        private BattleFSM battleFsm;

        public override void Initialize()
        {
            base.Initialize();
            battleFsm = (BattleFSM)Machine;
        }

        public override void Enter()
        {
            base.Enter();
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