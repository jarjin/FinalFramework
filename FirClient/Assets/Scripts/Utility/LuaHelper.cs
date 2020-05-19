using FirClient.Data;
using FirClient.Define;
using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FirClient.Utility 
{
    public static class LuaHelper
    {
        static LevelType newLevel;
        static Dictionary<Button, LuaFunction> btnClickEvents = new Dictionary<Button, LuaFunction>();

        public static string GetVersionInfo()
        {
            var version = string.Empty;
            if (!AppConst.DebugMode)
            {
                var path = Util.DataPath + "version.txt";
                if (File.Exists(path))
                {
                    version = File.ReadAllText(path);
                }
            }
            return string.Format("当前版本：{0}", version);
        }

        public static void AddButtonClick(Button button, LuaFunction func)
        {
            if (button != null && func != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate ()
                {
                    if (func != null)
                    {
                        func.Call<GameObject>(button.gameObject);
                    }
                });
                btnClickEvents[button] = func;
            }
        }

        public static void RemoveButtonClick(Button button)
        {
            if (button != null)
            {
                if (btnClickEvents[button] != null)
                {
                    btnClickEvents[button].Dispose();
                }
                btnClickEvents.Remove(button);
                button.onClick.RemoveAllListeners();
            }
        }

        /// <summary>
        /// 载入关卡
        /// </summary>
        public static void LoadLevel(LevelType levelType, LuaTable self, uint mapid, LuaFunction onLeave, LuaFunction onEnter)
        {
            newLevel = levelType;
            Util.StartCoroutine(OnLoadLevel(LevelType.Loader, self, mapid, onLeave, onEnter));
        }
        
        /// <summary>
        /// 当载入关卡
        /// </summary>
        static IEnumerator OnLoadLevel(LevelType levelType, LuaTable self, uint mapid, LuaFunction onLeave, LuaFunction onEnter)
        {
            int levelid = (int)levelType;
            var op = SceneManager.LoadSceneAsync(levelid);
            yield return op;
            yield return new WaitForSeconds(0.1f);

            if (LevelType.Loader == levelType)
            {
                if (onLeave != null)
                {
                    onLeave.Call<LuaTable, LevelType, Action>(self, levelType, (Action)delegate ()
                    {
                        Util.StartCoroutine(OnLoadLevel(newLevel, self, mapid, onLeave, onEnter));
                    });
                    onLeave.Dispose();
                    onLeave = null;
                }
            }
            else
            {
                if (onEnter != null)
                {
                    onEnter.Call<LuaTable, LevelType, Action>(self, levelType, (Action)delegate ()
                    {
                        Messenger.Broadcast<uint>(EventNames.EvBeginPlay, mapid);
                    });
                    onEnter.Dispose();
                    onEnter = null;

                    self.Dispose();
                    self = null;
                }
            }
        }
    }
}