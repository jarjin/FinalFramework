using System;
using System.Collections.Generic;

namespace FirClient.Component.FSM
{
    public interface IFSM
    {
        /// <summary>
        /// A reference to the state name
        /// </summary>
        string MachineName { get; set; }

        /// <summary>
        /// A refernce to the states dictionary
        /// </summary>
        Dictionary<Type, IState> States { get; set; }

        /// <summary>
        /// A reference to the current state
        /// </summary>
        IState CurrentState { get; set; }

        /// <summary>
        /// A reference to the initial state
        /// </summary>
        IState InitialState { get; set; }

        /// <summary>
        /// A reference to the next state
        /// </summary>
        IState NextState { get; set; }

        /// <summary>
        /// A reference to the previous state
        /// </summary>
        IState PreviousState { get; set; }

        /// <summary>
        /// Checks whether this instance contains a state of a particular type
        /// </summary>
        /// <typeparam name="T">state type</typeparam>
        /// <returns></returns>
        bool ContainsState<T>() where T : IState;

        /// <summary>
        /// Checks whether this state is the executing state
        /// </summary>
        /// <typeparam name="T">state type</typeparam>
        bool IsCurrentState<T>() where T : IState;

        /// <summary>
        /// Checks whether this state is the previous state
        /// </summary>
        /// <typeparam name="T">state type</typeparam>
        bool IsPreviousState<T>() where T : IState;

        /// <summary>
        /// Retrieves the executing state
        /// </summary>
        /// <typeparam name="T">state type</typeparam>
        T GetCurrentState<T>() where T : IState;

        /// <summary>
        /// Retrieves the initial state
        /// </summary>
        /// <typeparam name="T">state type</typeparam>
        T GetInitialState<T>() where T : IState;

        /// <summary>
        /// Retrieves the previously executed state
        /// </summary>
        /// <typeparam name="T">state type</typeparam>
        T GetPreviousState<T>() where T : IState;

        /// <summary>
        /// Retrieves a particular state
        /// </summary>
        /// <typeparam name="T">state type</typeparam>
        T GetState<T>() where T : IState;

        /// <summary>
        /// Retrieves the list of all the states
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetStates<T>() where T : IState;

        /// <summary>
        /// Adds a state to the state machine
        /// </summary>
        /// <typeparam name="T">type of state</typeparam>
        void AddState<T>() where T : IState, new();

        /// <summary>
        /// Changes the executing state to the specified one
        /// </summary>
        /// <typeparam name="T">type of state</typeparam>
        void ChangeState<T>() where T : IState;

        /// <summary>
        /// Removes a particular state
        /// </summary>
        /// <typeparam name="T">type of state</typeparam>
        void RemoveState<T>() where T : IState;

        /// <summary>
        /// Sets the current state
        /// </summary>
        /// <typeparam name="T">type of state</typeparam>
        void SetCurrentState<T>() where T : IState;

        /// <summary>
        /// Sets the initial state
        /// </summary>
        /// <typeparam name="T">type of state</typeparam>
        void SetInitialState<T>() where T : IState;

        /// <summary>
        /// Adds states to this state machine
        /// </summary>
        void AddStates();

        /// <summary>
        /// Initializes this instance
        /// </summary>
        void Initialize();

        /// <summary>
        /// Sets the initial state as the current state
        /// </summary>
        void SetInitialStateToCurrentState();

        /// <summary>
        /// Triggers state transition to the previous state
        /// </summary>
        /// <typeparam name="T">type of state</typeparam>
        void GoToPreviousState();

        /// <summary>
        /// This is the implementation of the execute method which is called 
        /// on every frame
        /// </summary>
        void OnExecute(float deltaTime);

        /// <summary>
        /// Removes all the states saved in this fsm
        /// </summary>
        void RemoveAllStates();
    }
}