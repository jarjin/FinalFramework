using FirClient.Component.FSM;
using UnityEngine;

namespace FirClient.Logic.AI.TurnBaseState
{
    public class TDeathState : FsmState
    {
        public override void Enter()
        {
            base.Enter();
            Debug.LogError("DeathState.Enter");
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