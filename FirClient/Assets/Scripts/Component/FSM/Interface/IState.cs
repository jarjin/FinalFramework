using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Component.FSM
{
    public interface IState
    {
        /// <summary>
        /// A refernce to the name of this instance
        /// </summary>
        string StateName { get; set; }

        /// <summary>
        /// A reference to the state machine that this instance belongs to
        /// </summary>
        IFSM Machine { get; set; }

        /// <summary>
        /// Retrieves the state machine that this state belongs to
        /// </summary>
        /// <typeparam name="T">type of state</typeparam>
        /// <returns></returns>
        T GetMachine<T>() where T : IFSM;

        /// <summary>
        /// Initializes this instance
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called once when a state is activated
        /// </summary>
        void Enter();

        /// <summary>
        /// Called every frame execute
        /// </summary>
        void Execute();

        /// <summary>
        /// Called once when a state is deactivated
        /// </summary>
        void Exit();
    }
}
