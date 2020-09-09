
namespace FirClient.Component.FSM
{
    public class BaseState : IState
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

        public virtual void Exit()
        {
        }
    }
}