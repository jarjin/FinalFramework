using System;
using System.Collections.Generic;
using System.Linq;

namespace FirClient.Component.FSM
{
    public class BaseFSM : IFSM
    {
        private float updateFrequency;
        private float _updateFrequencyTime;

        public virtual string MachineName { get; set; }
        public virtual IState CurrentState { get; set; }
        public virtual IState InitialState { get; set; }
        public virtual IState NextState { get; set; }
        public virtual IState PreviousState { get; set; }
        Dictionary<Type, IState> _states = new Dictionary<Type, IState>();

        /// <summary>
        /// Property to set or get the states list
        /// </summary>
        public virtual Dictionary<Type, IState> States
        {
            get { return _states; }
            set { _states = value; }
        }
        /// <summary>
        /// 状态机变量
        /// </summary>
        private FsmVariable fsmVariable = new FsmVariable();
        private static FsmVariable _globalVar = null;
        private static FsmVariable GlobalVar
        {
            get
            {
                if (_globalVar == null)
                    _globalVar = new FsmVariable();
                return _globalVar;
            }
        }
        /// <summary>
        /// 设置变量
        /// </summary>
        public void SetVar<T>(string key, T value)
        {
            var v = new FsmVar<T>(value);
            fsmVariable.SetVar<FsmVar<T>>(key, v);
        }

        /// <summary>
        /// 获取变量
        /// </summary>
        public FsmVar<T> GetVar<T>(string varName) where T : new()
        {
            return fsmVariable.GetVar<FsmVar<T>>(varName);
        }

        /// <summary>
        /// 移除变量
        /// </summary>
        public void RemoveVar(string varName) 
        {
            fsmVariable.RemoveVar(varName);
        }

        /// <summary>
        /// 清除所有变量
        /// </summary>
        public void ClearVars()
        {
            fsmVariable.ClearVars();
        }

        /// <summary>
        /// 设置全局变量
        /// </summary>
        public void SetGlobalVar<T>(string key, T value) 
        {
            var v = new FsmVar<T>(value);
            GlobalVar.SetVar<FsmVar<T>>(key, v);
        }

        /// <summary>
        /// 获取全局变量
        /// </summary>
        public FsmVar<T> GetGlobalVar<T>(string varName) where T : new()
        {
            return GlobalVar.GetVar<FsmVar<T>>(varName);
        }

        /// <summary>
        /// 移除全局变量
        /// </summary>
        public void RemoveGlobalVar<T>(string varName)
        {
            GlobalVar.RemoveVar(varName);
        }

        /// <summary>
        /// 清除所有全局变量
        /// </summary>
        public void ClearGlobalVar()
        {
            GlobalVar.ClearVars();
        }

        //-----------------------------------------------------------------------------------------------

        public virtual void AddState<T>() where T : IState, new()
        {
            if (!ContainsState<T>())
            {
                IState item = new T();
                item.Machine = this;
                States.Add(typeof(T), item);
            }
        }

        public virtual void AddStates() {}

        public virtual void ChangeState<T>() where T : IState
        {
            ChangeState(typeof(T));
        }

        public virtual bool ContainsState<T>() where T : IState
        {
            return States.ContainsKey(typeof(T));
        }

        public virtual T GetCurrentState<T>() where T : IState
        {
            return (T)CurrentState;
        }

        public virtual T GetInitialState<T>() where T : IState
        {
            return (T)InitialState;
        }

        public virtual T GetPreviousState<T>() where T : IState
        {
            return (T)PreviousState;
        }

        public virtual T GetState<T>() where T : IState
        {
            return (T)States[typeof(T)];
        }

        public virtual List<T> GetStates<T>() where T : IState
        {
            return States.Values.Cast<T>().ToList();
        }

        public virtual void GoToPreviousState()
        {
            ChangeState(PreviousState.GetType());
        }

        protected void ChangeState(Type type)
        {
            //try
            //{
                PreviousState = CurrentState;
                NextState = States[type];

                CurrentState.Exit();
                CurrentState = NextState;
                NextState = null;

                CurrentState.Enter();
            //}
            //catch (Exception e)
            //{
            //    throw new Exception("\n" + GetType().Name + " could not be found in the state machine" + e.Message);
            //}
        }

        public virtual void Initialize()
        {
            if (string.IsNullOrEmpty(MachineName))
            {
                MachineName = GetType().Name;
            }
            AddStates();
            CurrentState = InitialState;

            if (CurrentState == null)
            {
                throw new Exception("\n" + GetType().Name + "Initial state not specified.\tSpecify the initial state inside the AddStates()!!!\n");
            }
            foreach(var de in States)
            {
                de.Value.Machine = this;
                de.Value.Initialize();
            }
            CurrentState.Enter();
        }

        /// <summary>
        /// Sets the custome update frequency of the FSM
        /// </summary>
        /// <param name="value"></param>
        public void SetUpdateFrequency(float value)
        {
            if (value < 0)
            {
                return;
            }
            updateFrequency = value;
        }

        public virtual bool IsCurrentState<T>() where T : IState
        {
            return (CurrentState.GetType() == typeof(T)) ? true : false;
        }

        public virtual bool IsPreviousState<T>() where T : IState
        {
            return (PreviousState.GetType() == typeof(T)) ? true : false;
        }

        public virtual void OnExecute(float deltaTime)
        {
            _updateFrequencyTime += deltaTime;
            if (_updateFrequencyTime >= updateFrequency)
            {
                _updateFrequencyTime = 0;
                if (CurrentState != null)
                {
                    CurrentState.Execute();
                }
            }
        }

        public void RemoveAllStates()
        {
            States.Clear();
            NextState = null;
            CurrentState = null;
            PreviousState = null;
        }

        public virtual void RemoveState<T>() where T : IState
        {
            Type t = typeof(T);
            if (States.ContainsKey(t))
            {
                States.Remove(t);
            }
        }

        public virtual void SetCurrentState<T>() where T : IState
        {
            CurrentState = States[typeof(T)];
        }

        public virtual void SetInitialState<T>() where T : IState
        {
            InitialState = States[typeof(T)];
        }

        public virtual void SetInitialStateToCurrentState()
        {
            CurrentState = InitialState;
        }
    }
}