using FirClient.Logic;

namespace FirClient.Component.FSM
{
    public class FsmState : LogicBehaviour, IState
    {
        public string StateName { get; set; }
        public IFSM Machine { get; set; }

        public virtual void Initialize()
        {
        }

        public virtual void Enter()
        {
        }

        public virtual void Execute()
        {
        }

        public virtual T GetMachine<T>() where T : IFSM
        {
            return (T)Machine;
        }

        public virtual bool TryChangeState<T>() where T : IState
        {
            var bHaveAttack = Machine.ContainsState<T>();
            if (bHaveAttack)
            {
                Machine.ChangeState<T>();
                return true;
            }
            return false;
        }

        public virtual void Exit()
        {
        }
    }
}