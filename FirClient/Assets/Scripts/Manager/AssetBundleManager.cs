using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FirClient.Utility;
using LuaInterface;
using UnityEngine;
using UObject = UnityEngine.Object;


namespace FirClient.Manager {
    public class AssetBundleInfo
    {
        public AssetBundle m_AssetBundle;
        public int m_ReferencedCount;

        public AssetBundleInfo(AssetBundle assetBundle, int RefCount = 1)
        {
            m_AssetBundle = assetBundle;
            m_ReferencedCount = RefCount;
        }
    }

    class LoadAssetRequest
    {
        public Type assetType;
        public string[] assetNames;
        public LuaFunction luaFunc;
        public Action<UObject[]> sharpFunc;
    }

    class UnloadAssetBundleRequest {
        public string abName;
        public bool unloadNow;
        public AssetBundleInfo abInfo;
    }

    public class AssetBundleManager
    {
        ResourceManager mResMgr;
        string[] m_AllManifest = null;
        AssetBundleManifest m_AssetBundleManifest = null;
        Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
        Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();
        Dictionary<string, List<LoadAssetRequest>> m_LoadRequests = new Dictionary<string, List<LoadAssetRequest>>();
        Dictionary<string, int> m_AssetBundleLoadingList = new Dictionary<string, int>();
        Dictionary<string, UnloadAssetBundleRequest> m_AssetBundleUnloadingList = new Dictionary<string, UnloadAssetBundleRequest>();

        public Dictionary<string, AssetBundleInfo> LoadedAssetBundles
        {
            get
            {
                return m_LoadedAssetBundles;
            }
        }

        public AssetBundleManager(ResourceManager manager)
        {
            mResMgr = manager;
        }

        public void Initialize(string manifestName, Action initOK)
        {
            UnloadAssetBundle(manifestName, true);
            LoadAsset(manifestName, new string[] { "AssetBundleManifest" }, typeof(AssetBundleManifest), delegate (UObject[] objs)
            {
                if (objs.Length > 0)
                {
                    m_AssetBundleManifest = objs[0] as AssetBundleManifest;
                    m_AllManifest = m_AssetBundleManifest.GetAllAssetBundles();
                }
                if (initOK != null) initOK();
            });
        }

        /// <summary>
        /// 获取素材全路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string GetAssetFullPath(string path)
        {
            string assetPath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    assetPath = Path.Combine(Util.AppContentPath(), AppConst.ResIndexFile);
                    break;
                default:
                    assetPath = Path.Combine(Util.AppContentPath(), AppConst.ResIndexFile);
                    break;
            }
            return Path.Combine(assetPath, path);
        }

        string GetRealAssetPath(string abName)
        {
            if (abName.Equals(AppConst.ResIndexFile))
            {
                return abName;
            }
            abName = abName.ToLower();
            if (!abName.EndsWith(AppConst.ExtName))
            {
                abName += AppConst.ExtName;
            }
            if (abName.Contains("/"))
            {
                return abName;
            }
            //string[] paths = m_AssetBundleManifest.GetAllAssetBundles();  产生GC，需要缓存结果
            for (int i = 0; i < m_AllManifest.Length; i++)
            {
                int index = m_AllManifest[i].LastIndexOf('/');
                string path = m_AllManifest[i].Remove(0, index + 1);    //字符串操作函数都会产生GC
                if (path.Equals(abName))
                {
                    return m_AllManifest[i];
                }
            }
            Debug.LogError("GetRealAssetPath Error:>>" + abName);
            return null;
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        public void LoadAsset(string abName, string[] assetNames, Type assetType, Action<UObject[]> action = null, LuaFunction func = null)
        {
            abName = GetRealAssetPath(abName);

            var request = new LoadAssetRequest();
            request.assetType = assetType;
            request.assetNames = assetNames;
            request.luaFunc = func;
            request.sharpFunc = action;

            List<LoadAssetRequest> requests = null;
            if (!m_LoadRequests.TryGetValue(abName, out requests))
            {
                requests = new List<LoadAssetRequest>();
                requests.Add(request);
                m_LoadRequests.Add(abName, requests);
                mResMgr.StartCoroutine(OnLoadAsset(abName, assetType));
            }
            else
            {
                requests.Add(request);
            }
        }

        IEnumerator OnLoadAsset(string abName, Type assetType)
        {
            AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null)
            {
                yield return mResMgr.StartCoroutine(OnLoadAssetBundle(abName, assetType));

                bundleInfo = GetLoadedAssetBundle(abName);
                if (bundleInfo == null)
                {
                    m_LoadRequests.Remove(abName);
                    Debug.LogError("OnLoadAsset--->>>" + abName);
                    yield break;
                }
            }
            List<LoadAssetRequest> list = null;
            if (!m_LoadRequests.TryGetValue(abName, out list))
            {
                m_LoadRequests.Remove(abName);
                yield break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                string[] assetNames = list[i].assetNames;
                List<UObject> result = new List<UObject>();

                AssetBundle ab = bundleInfo.m_AssetBundle;
                if (assetNames != null)
                {
                    for (int j = 0; j < assetNames.Length; j++)
                    {
                        string assetPath = assetNames[j];
                        var request = ab.LoadAssetAsync(assetPath, assetType);
                        yield return request;
                        result.Add(request.asset);
                    }
                }
                else
                {
                    var request = ab.LoadAllAssetsAsync();
                    yield return request;
                    result = new List<UObject>(request.allAssets);
                }
                if (list[i].sharpFunc != null)
                {
                    list[i].sharpFunc(result.ToArray());
                    list[i].sharpFunc = null;
                }
                if (list[i].luaFunc != null)
                {
                    list[i].luaFunc.Call((object)result.ToArray());
                    list[i].luaFunc.Dispose();
                    list[i].luaFunc = null;
                }
                bundleInfo.m_ReferencedCount++;
            }
            m_LoadRequests.Remove(abName);
        }

        IEnumerator OnLoadAssetBundle(string abName, Type type)
        {
            string url = GetAssetFullPath(abName);
            if (m_AssetBundleLoadingList.ContainsKey(url))
            {
                m_AssetBundleLoadingList[url]++;
                yield break;
            }
            m_AssetBundleLoadingList.Add(url, 1);
            GLogger.Gray(url);
            var abUrl = Application.isEditor ? abName : url;
            var request = AssetBundle.LoadFromFileAsync(url);
            if (abName != AppConst.ResIndexFile)
            {
                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(abName);
                if (dependencies.Length > 0)
                {
                    m_Dependencies.Add(abName, dependencies);
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        string depName = dependencies[i];
                        AssetBundleInfo bundleInfo = null;
                        if (m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo))
                        {
                            bundleInfo.m_ReferencedCount++;
                        }
                        else if (!m_LoadRequests.ContainsKey(depName))
                        {
                            yield return mResMgr.StartCoroutine(OnLoadAssetBundle(depName, type));
                        }
                    }
                }
            }
            yield return request;

            AssetBundle assetObj = request.assetBundle;
            if (assetObj != null)
            {
                var RefCount = m_AssetBundleLoadingList[url];
                var bundleInfo = new AssetBundleInfo(assetObj, RefCount);
                m_LoadedAssetBundles.Add(abName, bundleInfo);
            }
            m_AssetBundleLoadingList.Remove(url);
        }

        AssetBundleInfo GetLoadedAssetBundle(string abName)
        {
            AssetBundleInfo bundle = null;
            m_LoadedAssetBundles.TryGetValue(abName, out bundle);
            if (bundle == null) return null;

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return bundle;

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies)
            {
                AssetBundleInfo dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null) return null;
            }
            return bundle;
        }

        public void Update(float deltaTime)
        {
            TryUnloadAssetBundle();
        }

        /// <summary>
        /// 试着去卸载AB
        /// </summary>
        private void TryUnloadAssetBundle()
        {
            if (m_AssetBundleUnloadingList.Count == 0)
            {
                return;
            }
            foreach (var de in m_AssetBundleUnloadingList)
            {
                if (m_AssetBundleLoadingList.ContainsKey(de.Key))
                {
                    continue;
                }
                var request = de.Value;

                if (request.abInfo != null && request.abInfo.m_AssetBundle != null)
                {
                    request.abInfo.m_AssetBundle.Unload(true);
                }
                m_AssetBundleUnloadingList.Remove(de.Key);
                m_LoadedAssetBundles.Remove(de.Key);
                Debugger.Log(de.Key + " has been unloaded successfully");
            }
        }

        /// <summary>
        /// 此函数交给外部卸载专用，自己调整是否需要彻底清除AB
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="isThorough"></param>
        public void UnloadAssetBundle(string abName, bool isThorough = false)
        {
            abName = GetRealAssetPath(abName);
            UnloadAssetBundleInternal(abName, isThorough);
            UnloadDependencies(abName, isThorough);
            Debugger.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + abName);
        }

        void UnloadDependencies(string abName, bool isThorough)
        {
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return;

            // Loop dependencies.
            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency, isThorough);
            }
            m_Dependencies.Remove(abName);
        }

        void UnloadAssetBundleInternal(string abName, bool unloadNow)
        {
            AssetBundleInfo bundle = GetLoadedAssetBundle(abName);
            if (bundle == null) return;

            if (--bundle.m_ReferencedCount <= 0)
            {
                if (m_AssetBundleLoadingList.ContainsKey(abName))
                {
                    var request = new UnloadAssetBundleRequest();
                    request.abName = abName;
                    request.abInfo = bundle;
                    request.unloadNow = unloadNow;
                    m_AssetBundleUnloadingList.Add(abName, request);
                    return;     //如果当前AB处于Async Loading过程中，卸载会崩溃，只减去引用计数即可
                }
                bundle.m_AssetBundle.Unload(unloadNow);
                m_LoadedAssetBundles.Remove(abName);
                Debugger.Log(abName + " has been unloaded successfully");
            }
        }
    }
}