using FirClient.Utility;
using UnityEngine;

namespace FirClient.Component
{
    public class CLuaComponent : MonoBehaviour
    {
        [SerializeField] 
        private string componentName;
        [SerializeField]
        private bool autoCallLuaEvent = true;

        private void Awake()
        {
            AddComponent();
            CallLuaFunction("Awake");
        }

        void Start()
        {
            CallLuaFunction("Start");
        }

        void OnEnable()
        {
            CallLuaFunction("OnEnable");
        }

        void OnDisable()
        {
            CallLuaFunction("OnDisable");
        }

        void AddComponent()
        {
            if (autoCallLuaEvent && !string.IsNullOrEmpty(componentName))
            {
                Util.CallLuaMethod("AddComponent", componentName, gameObject);
            }
        }

        void CallLuaFunction(string funcName)
        {
            if (autoCallLuaEvent && !string.IsNullOrEmpty(componentName))
            {
                int uniqueid = gameObject.GetInstanceID();
                Util.CallLuaMethod("CallTableFunc", componentName, uniqueid, funcName);
            }
        }

        void OnDestroy()
        {
            CallLuaFunction("OnDestroy");
        }
    }
}