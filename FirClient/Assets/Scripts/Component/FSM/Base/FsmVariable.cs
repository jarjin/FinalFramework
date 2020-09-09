using System.Collections.Generic;

namespace FirClient.Component.FSM
{
    public class FsmVar<T>
    {
        public T value;
        public FsmVar(T value)
        {
            this.value = value;
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }

    public class FsmVariable
    {
        Dictionary<string, object> fsmVars = new Dictionary<string, object>();

        internal void SetVar<T>(string key, T value) 
        {
            SetVariable<T>(key, value);
        }

        private void SetVariable<T>(string key, T value)
        {
            if (fsmVars.ContainsKey(key))
            {
                fsmVars[key] = value;
            }
            else
            {
                fsmVars.Add(key, value);
            }
        }

        internal T GetVar<T>(string varName) where T : class
        {
            if (fsmVars.ContainsKey(varName))
            {
                return fsmVars[varName] as T;
            }
            return null;
        }

        internal void RemoveVar(string varName) 
        {
            if (fsmVars.ContainsKey(varName))
            {
                fsmVars.Remove(varName);
            }
        }

        internal void ClearVars()
        {
            fsmVars.Clear();
        }
    }
}