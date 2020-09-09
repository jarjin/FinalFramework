using LuaInterface;
using System.Collections.Generic;
using UnityEngine.UI;
using FirClient.Utility;
using TMPro;

namespace FirClient.Behaviour
{
    class LuaEventData
    {
        public LuaTable self;
        public LuaFunction func;

        public LuaEventData(LuaTable self, LuaFunction func)
        {
            this.self = self;
            this.func = func;
        }

        public void Dispose()
        {
            if (self != null)
            {
                self.Dispose();
                self = null;
            }
            if (func != null)
            {
                func.Dispose();
                func = null;
            }
        }
    }

    public class LuaBehaviour : GameBehaviour
    {
        private Dictionary<object, LuaEventData> luaEvents = new Dictionary<object, LuaEventData>();

        protected void Awake()
        {
            Util.CallMethod(name, "Awake", gameObject);
        }

        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(Button button, LuaTable self, LuaFunction luafunc)
        {
            if (button != null && luafunc != null)
            {
                RemoveClick(button);
                luaEvents.Add(button, new LuaEventData(self, luafunc));

                button.onClick.AddListener(delegate ()
                {
                    luafunc.Call(self, button.gameObject);
                });
            }
        }

        /// <summary>
        /// 删除单击事件
        /// </summary>
        public void RemoveClick(Button button)
        {
            if (button == null) return;
            LuaEventData evdata = null;
            if (luaEvents.TryGetValue(button, out evdata))
            {
                if (evdata != null)
                {
                    evdata.Dispose();
                }
                luaEvents.Remove(button);
            }
            button.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// 添加选项框单击
        /// </summary>
        public void AddToggleClick(Toggle toggle, LuaTable self, LuaFunction luafunc)
        {
            if (toggle != null && luafunc != null)
            {
                RemoveToggleClick(toggle);
                luaEvents.Add(toggle, new LuaEventData(self, luafunc));

                toggle.onValueChanged.AddListener(delegate (bool check)
                {
                    luafunc.Call<LuaTable, bool>(self, check);
                });
            }
        }

        public void RemoveToggleClick(Toggle toggle)
        {
            if (toggle == null) return;
            LuaEventData evdata = null;
            if (luaEvents.TryGetValue(toggle, out evdata))
            {
                if (evdata != null)
                {
                    evdata.Dispose();
                }
                luaEvents.Remove(toggle);
            }
            toggle.onValueChanged.RemoveAllListeners();
        }

        /// <summary>
        /// 添加回车事件监听
        /// </summary>
        public void AddEndEdit(TMP_InputField input, LuaTable self, LuaFunction luaFunc)
        {
            if (input != null && luaFunc != null)
            {
                RemoveEndEdit(input);

                luaEvents.Add(input, new LuaEventData(self, luaFunc));
                input.onEndEdit.AddListener(delegate
                {
                    luaFunc.Call<LuaTable, TMP_InputField>(self, input);
                });
            }
        }

        public void RemoveEndEdit(TMP_InputField input)
        {
            if (input == null) return;
            LuaEventData evdata = null;
            if (luaEvents.TryGetValue(input, out evdata))
            {
                if (evdata != null)
                {
                    evdata.Dispose();
                }
                luaEvents.Remove(input);
            }
            input.onEndEdit.RemoveAllListeners();
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearLuaEvent()
        {
            foreach (var de in luaEvents)
            {
                if (de.Value != null)
                {
                    de.Value.Dispose();
                }
                if (de.Key.GetType() == typeof(Button))
                {
                    var button = de.Key as Button;
                    if (button != null)
                    {
                        button.onClick.RemoveAllListeners();
                    }
                }
                else if (de.Key.GetType() == typeof(Toggle))
                {
                    var toggle = de.Key as Toggle;
                    if (toggle != null)
                    {
                        toggle.onValueChanged.RemoveAllListeners();
                    }
                }
            }
            luaEvents.Clear();
            luaEvents = null;
        }

        //-----------------------------------------------------------------
        protected void OnDestroy()
        {
            ClearLuaEvent();
#if ASYNC_MODE
            string abName = name.ToLower().Replace("panel", "");
            ResManager.UnloadAssetBundle(abName + AppConst.ExtName);
#endif
            Util.ClearMemory();
            Debugger.Log("~" + name + " was destroy!");
        }
    }
}