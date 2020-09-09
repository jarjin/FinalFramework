using System;
using System.Collections.Generic;
using System.IO;
using FTRuntime;
using LuaInterface;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UObject = UnityEngine.Object;

namespace FirClient.Manager
{
    public class SimAssetManager
    {
        ResourceManager mResMgr;

        public SimAssetManager(ResourceManager manager)
        {
            mResMgr = manager;
        }

        public void Initialize(Action initOK)
        {
            if (initOK != null) initOK();
        }

        public void Update(float deltaTime) { }

        private string GetExtName(Type type)
        {
            if (type == typeof(GameObject))
            {
                return ".prefab";
            }
            else if (type == typeof(Texture2D) || type == typeof(Sprite))
            {
                return ".png";
            }
            else if (type == typeof(AudioClip))
            {
                return ".mp3";
            }
            else if (type == typeof(Material))
            {
                return ".mat";
            }
            else if (type == typeof(SwfClipAsset))
            {
                return ".asset";
            }
            else if (type == typeof(Shader))
            {
                return ".shader";    //不能返回
            }
            else if (type == typeof(Font))
            {
                return ".ttf";
            }
            return null;
        }

        public void LoadAsset(string abName, string[] assetNames, Type assetType, Action<UObject[]> action = null, LuaFunction func = null)
        {
            var result = new List<UObject>();
#if UNITY_EDITOR
            var extName = GetExtName(assetType);
            if (assetNames == null)
            {
                UObject[] objs = null;
                var assetPath = Application.dataPath + "/res/" + abName + extName;
                if (File.Exists(assetPath))
                {
                    var path = "Assets/res/" + abName + extName;
                    objs = AssetDatabase.LoadAllAssetsAtPath(path);
                }
                else
                {
                    var dirPath = Application.dataPath + "/res/" + abName;
                    var files = Directory.GetFiles(dirPath, "*" + extName, SearchOption.AllDirectories);
                    objs = new UObject[files.Length];
                    for (int i = 0; i < files.Length; i++)
                    {
                        var path = files[i].Replace(Application.dataPath, "Assets");
                        objs[i] = AssetDatabase.LoadAssetAtPath(path, assetType);
                    }
                }
                result = new List<UObject>(objs);
            }
            else
            {
                var dirName = abName.Substring(0, abName.LastIndexOf('/'));
                foreach (var name in assetNames)
                {
                    var path = "Assets/res/" + dirName + "/" + name + extName;
                    var obj = AssetDatabase.LoadAssetAtPath(path, assetType);
                    if (obj == null)
                    {
                        Debug.LogError("LoadAsset:>" + path + " was null!~~");
                    }
                    //if (obj == null) {}  //没必要判空，否则可能会影响上层逻辑
                    result.Add(obj);
                }
            }
#endif
            if (action != null)
            {
                action.Invoke(result.ToArray());
            }
            if (func != null)
            {
                func.Call(result.ToArray());
            }
        }
    }
}