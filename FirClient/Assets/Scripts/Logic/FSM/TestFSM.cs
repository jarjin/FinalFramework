using FirClient.Logic.FSM.TurnBaseState;
using System.Collections;
using UnityEngine;

namespace FirClient.Component.FSM
{
    public class TestFSM : MonoBehaviour
    {
        BaseFSM fsm = new BaseFSM();
        IEnumerator Start()
        {
            fsm.SetVar<bool>("aaa", true);
            fsm.SetVar<uint>("bbb", 100);

            fsm.SetUpdateFrequency(0.1f);

            fsm.AddState<TIdleState>();
            fsm.AddState<TAttackState>();
            fsm.AddState<TDeathState>();

            fsm.SetInitialState<TIdleState>();
            fsm.Initialize();

            var boolVar = fsm.GetVar<bool>("aaa");
            Debug.LogError(boolVar);    //

            var uintVar = fsm.GetVar<uint>("bbb");
            Debug.LogError(uintVar);

            yield return new WaitForSeconds(1);
            fsm.ChangeState<TAttackState>();

            yield return new WaitForSeconds(1);
            fsm.ChangeState<TDeathState>();

            yield return new WaitForSeconds(1);
            fsm.RemoveAllStates();
        }

        private void Update()
        {
            fsm.OnExecute(Time.deltaTime);
        }
    }
}