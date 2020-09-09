using LuaInterface;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirClient.Component
{
    public enum VarType : byte
    {
        GameObject,
        Transform,
        Image,
        Text,
        Button,
        TMP_InputField,
        Toggle,
        Slider,
        CMultiProgressBar,
    }

    [Serializable]
    public class VarData
    {
        public string name;
        public VarType type;
        public VarType lastType;
        public GameObject objValue;
        public Transform tranValue;
        public Image imgValue;
        public Text txtValue;
        public Button btnValue;
        public TMP_InputField inputValue;
        public Toggle toggleValue;
        public Slider sliderValue;
        public CMultiProgressBar multiProgreValue;
    }

    public class CPrefabVar : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        public List<VarData> varData;

        [SerializeField]
        [HideInInspector]
        private int m_selectedIndex = -1;

        private Dictionary<VarType, string> varKeys = new Dictionary<VarType, string>();

        public List<string> varTypes 
        {
            get
            {
                TryInit();
                return new List<string>(varKeys.Values);
            }
        }

        private void TryInit()
        {
            if (varKeys.Count == 0)
            {
                varKeys.Add(VarType.GameObject, "objValue");
                varKeys.Add(VarType.Transform, "tranValue");
                varKeys.Add(VarType.Text, "txtValue");
                varKeys.Add(VarType.Image, "imgValue");
                varKeys.Add(VarType.Button, "btnValue");
                varKeys.Add(VarType.TMP_InputField, "inputValue");
                varKeys.Add(VarType.Toggle, "toggleValue");
                varKeys.Add(VarType.Slider, "sliderValue");
                varKeys.Add(VarType.CMultiProgressBar, "multiProgreValue");
            }
        }

        public VarData[] GetVarArray()
        {
            return varData.ToArray();
        }

        public string GetVarNameByType(VarType varType)
        {
            TryInit();
            if (varKeys.ContainsKey(varType))
            {
                return varKeys[varType];
            }
            return null;
        }

        [NoToLua]
        public T GetVar<T>(string varName, VarType varType) where T : class
        {
            foreach(VarData v in varData)
            {
                if (v.name == varName)
                {
                    switch (varType)
                    {
                        case VarType.GameObject: return v.objValue as T;
                        case VarType.Transform: return v.tranValue as T;
                        case VarType.Text: return v.txtValue as T;
                        case VarType.Image: return v.imgValue as T;
                        case VarType.Button: return v.btnValue as T;
                        case VarType.TMP_InputField: return v.inputValue as T;
                        case VarType.Toggle: return v.toggleValue as T;
                        case VarType.Slider: return v.sliderValue as T;
                        case VarType.CMultiProgressBar: return v.multiProgreValue as T;
                    }
                }
            }
            return default(T);
        }
    }
}
