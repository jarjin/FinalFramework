using UnityEngine;
using System.IO;
using LuaInterface;
using FirClient.Utility;
using System;
using UObject = UnityEngine.Object;
using System.Collections.Generic;

namespace FirClient.Manager
{
    public class ResourceManager : BaseManager
    {
        SimAssetManager mSimMgr = null;
        AssetBundleManager mABMgr = null;

        /// <summary>
        /// 初始化
        /// </summary>
        [NoToLua]
        public override void Initialize()
        {
            if (AppConst.DebugMode)
            {
                mSimMgr = new SimAssetManager(this);
            }
            else
            {
                mABMgr = new AssetBundleManager(this);
            }
        }

        public void InitResManifest(string manifestName, Action initOK)
        {
            if (AppConst.DebugMode)
            {
                mSimMgr.Initialize(initOK);
            }
            else
            {
                mABMgr.Initialize(manifestName, initOK);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            if (AppConst.DebugMode)
            {
                mSimMgr.Update(deltaTime);
            }
            else
            {
                mABMgr.Update(deltaTime);
            }
        }

        /// <summary>
        /// 拍摄快照
        /// </summary>
        public void TakeSnapshot()
        {
            if (AppConst.DebugMode)
            {
                return;
            }
            var logPath = Util.DataPath + "snapshot_a.txt";
            if (File.Exists(logPath))
            {
                logPath = Util.DataPath + "snapshot_b.txt";
            }
            var abs = mABMgr.LoadedAssetBundles;
            File.WriteAllLines(logPath, abs.Keys);
        }

        /// <summary>
        /// 清除快照
        /// </summary>
        public void ClearSnapshot()
        {
            if (AppConst.DebugMode)
            {
                return;
            }
            var logPathA = Util.DataPath + "snapshot_a.txt";
            if (File.Exists(logPathA))
            {
                File.Delete(logPathA);
            }
            var logPathB = Util.DataPath + "snapshot_b.txt";
            if (File.Exists(logPathB))
            {
                File.Delete(logPathB);
            }
            var diffLog = Util.DataPath + "diff.txt";
            if (File.Exists(diffLog))
            {
                File.Delete(diffLog);
            }
        }

        /// <summary>
        /// 对比快照
        /// </summary>
        public void DiffSnapshot()
        {
            if (AppConst.DebugMode)
            {
                return;
            }
            var logPathA = Util.DataPath + "snapshot_a.txt";
            var logPathB = Util.DataPath + "snapshot_b.txt";
            if (!File.Exists(logPathB))
            {
                TakeSnapshot();
            }
            List<string> diffFiles = new List<string>();

            string[] snapshotA = File.ReadAllLines(logPathA);
            string[] snapshotB = File.ReadAllLines(logPathB);
            foreach(string fileB in snapshotB)
            {
                if (string.IsNullOrEmpty(fileB))
                {
                    continue;
                }
                bool isExist = false;
                foreach(string fileA in snapshotA)
                {
                    if (string.IsNullOrEmpty(fileA))
                    {
                        continue;
                    }
                    if (fileA == fileB)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    diffFiles.Add(fileB);
                }
            }
            if (diffFiles.Count > 0)
            {
                foreach(var file in diffFiles)
                {
                    Debugger.LogError("Remain file:>>>" + file);
                }
                var diffLog = Util.DataPath + "diff.txt";
                File.WriteAllLines(diffLog, diffFiles.ToArray());
            }
        }

        public T LoadResAsset<T>(string path) where T : UObject
        {
            UObject o = Resources.Load<T>(path);
            if (o == null)
            {
                return null;
            }
            return o as T;
        }

        public T[] LoadResAssets<T>(string path) where T : UObject
        {
            return Resources.LoadAll<T>(path);
        }

        public T LoadLocalAsset<T>(string path) where T : class
        {
            var filePath = Util.DataPath + path;
            if (File.Exists(filePath))
            {
                var type = typeof(T);
                if (type == typeof(byte[]))
                {
                    return File.ReadAllBytes(filePath) as T;
                }
                else if (type == typeof(string))
                {
                    return File.ReadAllText(filePath) as T;
                }
            }
            return null as T;
        }

        public ResourceRequest LoadResAsync<T>(string path) where T : UObject
        {
            return Resources.LoadAsync<T>(path);
        }

        public void LoadAssetAsync<T>(string abName, string[] assetNames, Action<UObject[]> func)
        {
            var assetType = typeof(T);
            if (AppConst.DebugMode)
            {
                mSimMgr.LoadAsset(abName, assetNames, assetType, func);
            }
            else
            {
                mABMgr.LoadAsset(abName, assetNames, assetType, func);
            }
        }

        public void LoadAssetAsync(string abName, string[] assetNames, Type assetType, LuaFunction func)
        {
            if (AppConst.DebugMode)
            {
                mSimMgr.LoadAsset(abName, assetNames, assetType, null, func);
            }
            else
            {
                mABMgr.LoadAsset(abName, assetNames, assetType, null, func);
            }
        }

        [NoToLua]
        public override void OnDispose()
        {
        }
    }
}