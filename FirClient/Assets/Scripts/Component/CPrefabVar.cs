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

        public VarData[] GetVarArray()
        {
            return varData.ToArray();
        }

        private void TryInit()
        {
            if (varKeys.Count > 0)
            {
                return;
            }
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

        public string GetVarNameByType(int type)
        {
            TryInit();
            var varType = (VarType)type;
            if (varKeys.ContainsKey(varType))
            {
                return varKeys[varType];
            }
            return null;
        }
    }
}
