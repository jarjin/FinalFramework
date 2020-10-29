using LuaInterface;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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
        
        [NoToLua]
        public Object GetValue()
        {
            switch (type)
            {
                case VarType.GameObject: return objValue;
                case VarType.Transform: return tranValue;
                case VarType.Text: return txtValue;
                case VarType.Image: return imgValue;
                case VarType.Button: return btnValue;
                case VarType.TMP_InputField: return inputValue;
                case VarType.Toggle: return toggleValue;
                case VarType.Slider: return sliderValue;
                case VarType.CMultiProgressBar: return multiProgreValue;
                default: return null;
            }
        }

        [NoToLua]
        public void Set(VarData newData)
        {
            //Clear
            switch (type)
            {
                case VarType.GameObject:
                    objValue = null; break;
                case VarType.Transform:
                    tranValue = null; break;
                case VarType.Text:
                    txtValue = null; break;
                case VarType.Image:
                    imgValue = null; break;
                case VarType.Button:
                    btnValue = null; break;
                case VarType.TMP_InputField:
                    inputValue = null; break;
                case VarType.Toggle:
                    toggleValue = null; break;
                case VarType.Slider:
                    sliderValue = null; break;
                case VarType.CMultiProgressBar:
                    multiProgreValue = null; break;
            }
            //Set
            name = newData.name;
            lastType = newData.type;
            type = newData.type;
            switch (type)
            {
                case VarType.GameObject:
                    objValue = newData.objValue; break;
                case VarType.Transform:
                    tranValue = newData.tranValue; break;
                case VarType.Text:
                    txtValue = newData.txtValue; break;
                case VarType.Image:
                    imgValue = newData.imgValue; break;
                case VarType.Button:
                    btnValue = newData.btnValue; break;
                case VarType.TMP_InputField:
                    inputValue = newData.inputValue; break;
                case VarType.Toggle:
                    toggleValue = newData.toggleValue; break;
                case VarType.Slider:
                    sliderValue = newData.sliderValue; break;
                case VarType.CMultiProgressBar:
                    multiProgreValue = newData.multiProgreValue; break;
            }
        }
    }

    public class CPrefabVar : MonoBehaviour
    {
        #region AutoBindDict
        [NoToLua] public static readonly Dictionary<string, Func<GameObject, VarData>> AutoBindDict = new Dictionary<string, Func<GameObject, VarData>>
        {
            {"obj", go => new VarData {lastType = VarType.GameObject, type = VarType.GameObject, objValue = go}},
            {"tran", go => new VarData {lastType = VarType.Transform, type = VarType.Transform, tranValue = go.transform}},
            {
                "img", go =>
                {
                    var value = go.GetComponent<Image>();
                    if (value)
                        return new VarData {lastType = VarType.Image, type = VarType.Image, imgValue = value};
                    return null;
                }
            },
            {
                "txt", go =>
                {
                    var value = go.GetComponent<Text>();
                    if (value)
                        return new VarData {lastType = VarType.Text, type = VarType.Text, txtValue = value};
                    return null;
                }
            },
            {
                "btn", go =>
                {
                    var value = go.GetComponent<Button>();
                    if (value)
                        return new VarData {lastType = VarType.Button, type = VarType.Button, btnValue = value};
                    return null;
                }
            },
            {
                "input", go =>
                {
                    var value = go.GetComponent<TMP_InputField>();
                    if (value)
                        return new VarData {lastType = VarType.TMP_InputField, type = VarType.TMP_InputField, inputValue = value};
                    return null;
                }
            },
            {
                "toggle", go =>
                {
                    var value = go.GetComponent<Toggle>();
                    if (value)
                        return new VarData {lastType = VarType.Toggle, type = VarType.Toggle, toggleValue = value};
                    return null;
                }
            },
            {
                "slider", go =>
                {
                    var value = go.GetComponent<Slider>();
                    if (value)
                        return new VarData {lastType = VarType.Slider, type = VarType.Slider, sliderValue = value};
                    return null;
                }
            },
            {
                "multiProgre", go =>
                {
                    var value = go.GetComponent<CMultiProgressBar>();
                    if (value)
                        return new VarData {lastType = VarType.CMultiProgressBar, type = VarType.CMultiProgressBar, multiProgreValue = value};
                    return null;
                }
            },
        };
        #endregion
        
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

        [NoToLua]
        public void AutoBind()
        {
            DeepSearch(transform);
        }

        [NoToLua]
        private void DeepSearch(Transform tran)
        {
            if (tran.name[0] == '#')
            {
                string objName = tran.name.Substring(1);
                string varType = objName.Split('_')[0];
                if (AutoBindDict.TryGetValue(varType, out var func))
                {
                    var newData = func(tran.gameObject);
                    if (newData != null)
                    {
                        newData.name = objName;
                        bool needAdd = true;
                        foreach (var data in varData)
                        {
                            if (data.name == newData.name)
                            {
                                needAdd = false;
                                if (data.GetValue() != newData.GetValue())
                                {
                                    data.Set(newData);
                                    Debugger.Log($"Auto bind replace var! name: {data.name}");
                                }
                                break;
                            }
                        }
                        if (needAdd)
                            varData.Add(newData);
                    }
                }
            }
            for (int i = 0, count = tran.childCount; i < count; i++)
                DeepSearch(tran.GetChild(i));
        }
    }
}
