using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using FirClient.Manager;
using FirClient.Define;
using LuaInterface;
using FirClient.UI;
using FirClient.HUD;
using UnityEngine.UI;
using System.Collections.Generic;

namespace FirClient.Utility
{
    public static class Util
    {
        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static long GetTime()
        {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        public static string RandomTime()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 手机震动
        /// </summary>
        public static void Vibrate()
        {
            //int canVibrate = PlayerPrefs.GetInt(Const.AppPrefix + "Vibrate", 1);
            //if (canVibrate == 1) iPhoneUtils.Vibrate();
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        public static string Encode(string message)
        {
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        public static string Decode(string message)
        {
            byte[] bytes = Convert.FromBase64String(message);
            return Encoding.GetEncoding("utf-8").GetString(bytes);
        }

        /// <summary>
        /// 判断数字
        /// </summary>
        public static bool IsNumeric(string str)
        {
            if (str == null || str.Length == 0) return false;
            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsNumber(str[i])) { return false; }
            }
            return true;
        }

        /// <summary>
        /// HashToMD5Hex
        /// </summary>
        public static string HashToMD5Hex(string sourceStr)
        {
            byte[] Bytes = Encoding.UTF8.GetBytes(sourceStr);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] result = md5.ComputeHash(Bytes);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                    builder.Append(result[i].ToString("x2"));
                return builder.ToString();
            }
        }

        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        public static string md5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++)
            {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string md5file(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        public static void SetDebugState(bool state)
        {
            Debug.unityLogger.logEnabled = state;
        }

        /// <summary>
        /// 生成一个Key名
        /// </summary>
        public static string GetKey(string key)
        {
            return AppConst.AppPrefix + "_" + key;
        }

        /// <summary>
        /// 取得整型
        /// </summary>
        public static int GetInt(string key)
        {
            string name = GetKey(key);
            return PlayerPrefs.GetInt(name);
        }

        /// <summary>
        /// 有没有值
        /// </summary>
        public static bool HasKey(string key)
        {
            string name = GetKey(key);
            return PlayerPrefs.HasKey(name);
        }

        /// <summary>
        /// 保存整型
        /// </summary>
        public static void SetInt(string key, int value)
        {
            string name = GetKey(key);
            PlayerPrefs.DeleteKey(name);
            PlayerPrefs.SetInt(name, value);
        }

        /// <summary>
        /// 取得数据
        /// </summary>
        public static string GetString(string key)
        {
            string name = GetKey(key);
            return PlayerPrefs.GetString(name);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public static void SetString(string key, string value)
        {
            string name = GetKey(key);
            PlayerPrefs.DeleteKey(name);
            PlayerPrefs.SetString(name, value);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public static void RemoveData(string key)
        {
            string name = GetKey(key);
            PlayerPrefs.DeleteKey(name);
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect(); Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 是否为数字
        /// </summary>
        public static bool IsNumber(string strNumber)
        {
            Regex regex = new Regex("[^0-9]");
            return !regex.IsMatch(strNumber);
        }

        /// <summary>
        /// 取得行文本
        /// </summary>
        public static string GetFileText(string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// 网络可用
        /// </summary>
        public static bool NetAvailable
        {
            get
            {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        /// <summary>
        /// 是否是无线
        /// </summary>
        public static bool IsWifi
        {
            get
            {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }

        /// <summary>
        /// 应用程序内容路径
        /// </summary>
        public static string AppContentPath()
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = Application.streamingAssetsPath + "/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw/";
                    break;
                default:
                    path = Application.streamingAssetsPath + "/";
                    break;
            }
            return path;
        }

        /// <summary>
        /// 取得数据存放目录
        /// </summary>
        public static string DataPath
        {
            get
            {
                if (Application.isMobilePlatform)
                {
                    return Application.persistentDataPath + "/" + AppConst.AppName + "/";
                }
                if (AppConst.DebugMode)
                {
                    return Application.dataPath + "/res/";
                }
                var dataDir = Path.GetDirectoryName(Application.dataPath);
                return dataDir + "/PersistentData/";
            }
        }

        public static string LuaPath()
        {
            if (AppConst.DebugMode)
            {
                return Application.dataPath + "/lua/";
            }
            return DataPath + "lua/";
        }

        /// <summary>
        /// 取得Lua路径
        /// </summary>
        public static string LuaPath(string name)
        {
            if (name.EndsWith(".lua"))
            {
                return DataPath + "lua/" + name;
            }
            return DataPath + "lua/" + name + ".lua";
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        public static object[] CallMethod(string module, string func, params object[] args)
        {
            var luaMgr = ManagementCenter.GetManager<LuaManager>();
            if (luaMgr == null) return null;
            return luaMgr.CallFunction(module + "." + func, args);
        }

        /// <summary>
        /// 调用Game接口函数
        /// </summary>
        public static object[] CallLuaMethod(string func, params object[] args)
        {
            return CallMethod("Main", func, args);
        }

        /// <summary>
        /// 更新LOADING进度
        /// </summary>
        public static void UpdateLoadingProgress(string text, float curr, float max)
        {
            var loadingUi = LoadingUI.Instance();
            if (loadingUi != null)
            {
                loadingUi.UpdateLoadingText(text);
                loadingUi.UpdateLoadingProgress(curr, max);
            }
        }

        public static VersionInfo GetVersionInfo(string verfile)
        {
            if (!File.Exists(verfile))
            {
                return null;
            }
            string localVerStr = File.ReadAllText(verfile);
            return InitVersionInfo(localVerStr);
        }

        public static VersionInfo InitVersionInfo(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            var strs = content.Split('.');
            VersionInfo info = new VersionInfo();
            info.mainVersion = strs[0];
            info.primaryVersion = strs[1];
            info.patchVersion = strs[2];
            return info;
        }

        public static void CopyDir(string srcDir, string destDir, string filter = null)
        {
            var srcDirInfo = new DirectoryInfo(srcDir);
            if (!srcDirInfo.Exists) return;
            var destDirInfo = new DirectoryInfo(destDir);
            if (!destDirInfo.Exists)
            {
                destDirInfo.Create();
            }
            var files = srcDirInfo.GetFiles();
            foreach (var file in files)
            {
                var destFile = Path.Combine(destDirInfo.FullName, file.Name);
                if (file.Extension == filter)
                {
                    continue;
                }
                file.CopyTo(destFile, true);
            }
            var dirs = srcDirInfo.GetDirectories();
            foreach (var srcSubDir in dirs)
            {
                string dirPath = Path.Combine(destDirInfo.FullName, srcSubDir.Name);
                CopyDir(srcSubDir.FullName, dirPath);
            }
        }

        public static void StartCoroutine(IEnumerator routine)
        {
            ManagementCenter.main.StartCoroutine(routine);
        }

        public static GameSettings LoadGameSettings()
        {
            return Resources.Load<GameSettings>(AppConst.GameSettingName);
        }

        public static HUDObject AddHudObject(GameObject gameObject)
        {
            var objs = CallMethod("Main", "AddHudObject", gameObject);
            if (objs != null && objs.Length > 0)
            {
                var gameObj = objs[0] as GameObject;
                return gameObj.AddComponent<HUDObject>();
            }
            return null;
        }

        public static void RemoveHudObject(string name)
        {
            CallLuaMethod("RemoveHudObject", name);
        }

        public static GameObject GetFloatingTextPrefab()
        {
            var objs = CallMethod("Main", "GetFloatingTextPrefab");
            if (objs != null && objs.Length > 0)
            {
                return objs[0] as GameObject;
            }
            return null;
        }

        /// <summary>
        /// 打印LUA堆栈
        /// </summary>
        /// <param name="L"></param>
        static void PrintLuaStack(IntPtr L)
        {
            Debug.Log("========= content of stack from top to bottom: ===========");
            int stackSize = LuaDLL.lua_gettop(L);
            for (int i = stackSize; i > 0; --i)
            {
                var str = string.Format("{0} [{1}]\t", i, -1 - (stackSize - i));
                LuaTypes t = LuaDLL.lua_type(L, i);
                switch (t)
                {
                    case LuaTypes.LUA_TNUMBER: 
                        str += string.Format("{0}: \t{1}\n", LuaDLL.lua_typename(L, t), LuaDLL.lua_tonumber(L, i));
                        break;
                    case LuaTypes.LUA_TSTRING:
                        str += string.Format("{0}: \t{1}\n", LuaDLL.lua_typename(L, t), LuaDLL.lua_tostring(L, i));
                        break;
                    case LuaTypes.LUA_TTABLE:
                        str += string.Format("{0}: \t{1}\n", LuaDLL.lua_typename(L, t), LuaDLL.lua_tostring(L, i));
                        break;
                    case LuaTypes.LUA_TTHREAD:
                        str += string.Format("{0}: \t{1}\n", LuaDLL.lua_typename(L, t), LuaDLL.lua_tostring(L, i));
                        break;
                    case LuaTypes.LUA_TFUNCTION:
                        str += string.Format("{0}: \t{1}\n", LuaDLL.lua_typename(L, t), LuaDLL.lua_tostring(L, i));
                        break;
                    case LuaTypes.LUA_TLIGHTUSERDATA:
                        str += string.Format("{0}: \t{1}\n", LuaDLL.lua_typename(L, t), LuaDLL.lua_tostring(L, i));
                        break;
                    case LuaTypes.LUA_TUSERDATA:
                        str += string.Format("{0}: \t{1}\n", LuaDLL.lua_typename(L, t), LuaDLL.lua_tostring(L, i));
                        break;
                    case LuaTypes.LUA_TNIL:
                        str += string.Format("{0}: \t{1}\n", LuaDLL.lua_typename(L, t), LuaDLL.lua_tostring(L, i));
                        break;
                    default:
                        str += string.Format("{0}\n", LuaDLL.lua_typename(L, t));
                        break;
                }
                Debug.Log(str);
            }
        }

        public static void UnloadAsset(GameObject gameObj)
        {
            if (gameObj != null)
            {
                UnloadAssetType<Text>(gameObj);
                UnloadAssetType<Image>(gameObj);
            }
        }

        static void UnloadAssetType<T>(GameObject gameObj)
        {
            var components = gameObj.GetComponentsInChildren<T>();
            if (components.Length > 0)
            {
                Type compType = typeof(T);
                var assets = new List<UnityEngine.Object>();
                for (int i = 0; i < components.Length; i++)
                {
                    var c = components[i];
                    if (compType == typeof(Image))
                    {
                        var image = c as Image;
                        if (image != null && image.sprite != null && !assets.Contains(image.sprite.texture))
                        {
                            assets.Add(image.sprite.texture);
                        }
                    }
                    else if (compType == typeof(Text))
                    {
                        var text = c as Text;
                        if (text != null && !assets.Contains(text.font))
                        {
                            assets.Add(text.font);
                        }
                    }
                }
                for (int i = 0; i < assets.Count; i++)
                {
                    if (assets[i] != null)
                    {
                        Resources.UnloadAsset(assets[i]);
                    }
                }
                assets = null;
            }
        }

        /// <summary>
        /// 相机边距
        /// </summary>
        /// <returns></returns>
        public static float CameraHalfWidth()
        {
            return Camera.main.orthographicSize * ((float)Screen.width / Screen.height);
        }
    }
}