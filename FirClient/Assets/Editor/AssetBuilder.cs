using System.Collections.Generic;
using System.IO;
using FirClient.Component;
using FTRuntime;
using UnityEditor;
using UnityEngine;

public class AssetBuilder : BaseEditor
{
    [MenuItem("AssetBuilder/生成所有图集特效")]
    static void BuildAtlasEffect()
    {
        BuildAtlasPrefab("Effect");
    }

    [MenuItem("AssetBuilder/生成所有Flash特效")]
    static void BuildFlashEffect()
    {
        BuildFlashEffectPrefab();
    }

    [MenuItem("AssetBuilder/生成所有图集子弹")]
    static void BuildAtlasBullet()
    {
        BuildAtlasPrefab("Bullet");
    }

    [MenuItem("AssetBuilder/生成所有Flash角色")]
    static void BuildFlashRole()
    {
        BuildFlashRolePrefab();
    }

    [MenuItem("AssetBuilder/Export Flash", false, 100)]
    static void ExportFlash()
    {
        ExecuteProc(AppDataPath + "/Libraries/FlashTools/FlashExport/FlashExport.jsfl", null, true);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Create Or Update Swf Prefab", false, 508)]
    static void BuildSingleFlash()
    {
        var swfPath = GetSelectedObjectPath();
        if (!swfPath.EndsWith(".fla"))
        {
            return;
        }
        swfPath = AppDataPath.Replace("Assets", string.Empty) + swfPath + "_export";
        BuildFlashInternal(swfPath);
        AssetDatabase.Refresh();
    }

    static void BuildFlashInternal(string dir)
    {
        var dirName = Path.GetFileNameWithoutExtension(dir);
        var files = Directory.GetFiles(dir, "*.asset", SearchOption.AllDirectories);
        var gameObj = new GameObject();
        var uswf = gameObj.AddComponent<CSwf>();
        var swfClip = gameObj.AddComponent<SwfClip>();
        swfClip.sortingOrder = AppConst.RoleSortLayer;

        foreach (var s in files)
        {
            if (s.EndsWith(".fla.asset") || s.EndsWith(".fla._Stage_.asset"))
            {
                continue;
            }
            var f = s.Replace('\\', '/').Replace(AppDataPath, "Assets");
            var clip = AssetDatabase.LoadAssetAtPath<SwfClipAsset>(f);
            if (clip != null)
            {
                uswf.AddSwfClip(clip);
                if (clip.name.ToLower().Contains("idle"))
                {
                    swfClip.clip = clip;
                }
            }
        }
        gameObj.AddComponent<SwfClipController>().autoPlay = true;
        gameObj.GetComponent<MeshRenderer>().receiveShadows = false;

        string path = "Assets/res/Prefabs/Character/" + dirName + ".prefab";
        PrefabUtility.SaveAsPrefabAsset(gameObj, path);
        GameObject.DestroyImmediate(gameObj);
    }

    static void BuildFlashRolePrefab()
    {
        string flashPath = AppDataPath + "/res/Swfs/";
        var dirs = Directory.GetDirectories(flashPath);
        foreach (var dir in dirs)
        {
            BuildFlashInternal(dir);
        }
        AssetDatabase.Refresh();
    }

    static void BuildFlashEffectPrefab()
    {
        string flashPath = AppDataPath + "/res/Effects/";
        var files = Directory.GetFiles(flashPath, "*.asset", SearchOption.AllDirectories);
        foreach (var s in files)
        {
            if (s.EndsWith(".fla.asset") || s.EndsWith(".fla._Stage_.asset"))
            {
                continue;
            }
            var name = Path.GetFileNameWithoutExtension(s);
            name = name.Substring(0, name.IndexOf('.'));

            var f = s.Replace('\\', '/').Replace(AppDataPath, "Assets");

            var gameObj = new GameObject();
            var swfClip = gameObj.AddComponent<SwfClip>();
            swfClip.sortingOrder = AppConst.RoleSortLayer;
            swfClip.clip = AssetDatabase.LoadAssetAtPath<SwfClipAsset>(f);

            gameObj.AddComponent<CSwf>();
            gameObj.GetComponent<MeshRenderer>().receiveShadows = false;

            var swfCtrl = gameObj.AddComponent<SwfClipController>();
            swfCtrl.autoPlay = true;
            swfCtrl.loopMode = SwfClipController.LoopModes.Once;

            string path = "Assets/res/Prefabs/Effect/" + name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(gameObj, path);
            GameObject.DestroyImmediate(gameObj);
        }
        AssetDatabase.Refresh();
    }

    static void BuildAtlasPrefab(string dirName)
    {
        string effectAtlasPath = AppDataPath + "/res/Atlas/" + dirName;
        var files = Directory.GetFiles(effectAtlasPath, "*.png");
        foreach (var s in files)
        {
            var name = Path.GetFileNameWithoutExtension(s);
            var f = s.Replace('\\', '/').Replace(AppDataPath, "Assets");
            var o = AssetDatabase.LoadAllAssetsAtPath(f);

            var gameObj = new GameObject();
            var actor = gameObj.AddComponent<CAnimActor>();
            actor.animations = new CAnimActor.AntAnimation[1];
            actor.animations[0].name = "Move";
            actor.initialAnimation = "Move";

            var sprites = new List<Sprite>();
            for (int i = 0; i < o.Length; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                else
                {
                    sprites.Add(o[i] as Sprite);
                }
            }
            actor.animations[0].frames = sprites.ToArray();
            actor.loop = true;

            var spriteRender = gameObj.GetComponent<SpriteRenderer>();
            spriteRender.sprite = sprites[0];

            string path = "Assets/res/Prefabs/" + dirName + "/" + name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(gameObj, path);
            GameObject.DestroyImmediate(gameObj);
        }
        AssetDatabase.Refresh();
    }
}