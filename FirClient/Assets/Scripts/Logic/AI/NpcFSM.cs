using FirClient.Logic.AI.TurnBaseState;
using FirClient.Logic.AI.FreeBattleState;
using FirClient.Data;

namespace FirClient.Logic.AI
{
    public class NpcFSM : GameFSM
    {
        public void Initialize(long npcId)
        {
            SetVar<long>("mynpcId", npcId);
            SetVar<long>("targetId", 0);
            base.Initialize();
        }

        public override void AddStates()
        {
            //SetUpdateFrequency(0.1f);
            switch (LogicConst.BattleType)
            {
                case BattleType.TurnBase:
                    InitTurnBaseState();
                break;
                case BattleType.FreeBattle:
                    InitFreeBattleState();
                break;
            }
        }

        private void InitFreeBattleState()
        {
            AddState<FIdleState>();
            AddState<FSearchState>();
            AddState<FAttackState>();
            AddState<FDeathState>();

            SetInitialState<FIdleState>();
        }

        private void InitTurnBaseState()
        {
            AddState<TIdleState>();
            AddState<TSearchState>();
            AddState<TAttackState>();
            AddState<TDeathState>();

            SetInitialState<TIdleState>();
        }

        public void OnUpdate(float deltaTime)
        {
            OnExecute(deltaTime);
        }
    }
}