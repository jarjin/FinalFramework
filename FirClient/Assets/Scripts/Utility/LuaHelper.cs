using FirClient.Data;
using FirClient.Define;
using FirClient.Extensions;
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
        public static void LoadLevel(LevelType levelType, LuaTable self, LuaFunction onLeave, LuaFunction onEnter)
        {
            newLevel = levelType;
            Util.StartCoroutine(OnLoadLevel(LevelType.Loader, self, onLeave, onEnter));
        }

        /// <summary>
        /// 当载入关卡
        /// </summary>
        static IEnumerator OnLoadLevel(LevelType levelType, LuaTable self, LuaFunction onLeave, LuaFunction onEnter)
        {
            Scene scene = SceneManager.GetActiveScene();
            var levelName = scene.name.FirstCharToUpper();
            LevelType currLevel = (LevelType)Enum.Parse(typeof(LevelType), levelName);

            var op = SceneManager.LoadSceneAsync(levelType.ToString());
            yield return op;
            yield return new WaitForSeconds(0.1f);

            if (LevelType.Loader == levelType)
            {
                if (onLeave != null)
                {
                    onLeave.Call<LuaTable, LevelType, Action>(self, currLevel, delegate ()
                    {
                        Util.StartCoroutine(OnLoadLevel(newLevel, self, onLeave, onEnter));
                    });
                    onLeave.Dispose();
                    onLeave = null;
                }
            }
            else
            {
                if (onEnter != null)
                {
                    var loadMsg = "OnLoadLevel " + levelType + " OK!!!";
                    onEnter.Call<LuaTable, LevelType, Action>(self, levelType, () => GLogger.Yellow(loadMsg));
                    onEnter.Dispose();
                    onEnter = null;

                    self.Dispose();
                    self = null;
                }
            }
        }

        public static void CallAction(Action action)
        {
            if (action != null) action();
        }

        public static void InitBeginPlay(uint mapid)
        {
            Messenger.Broadcast<uint>(EventNames.EvBeginPlay, mapid);
        }
    }
}