using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class FixChecker : BaseEditor
{
    static string emptyAssetPath = "Assets/Resources/Textures/Empty.png";
    static string uiAssetPath = "Assets/res/Prefabs/UI/";

    [MenuItem("FixChecker/清理ItemBox素材")]
    public static void ClearItemBoxAsset()
    {
        var emptySprite = AssetDatabase.LoadAssetAtPath<Sprite>(emptyAssetPath);
        var files = Directory.GetFiles(uiAssetPath, "*.prefab", SearchOption.AllDirectories);

        //foreach(var file in files)
        //{
        //    var newPath = file.Replace(AppDataPath, "Assets");
        //    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(newPath);
        //    if (prefab != null)
        //    {
        //        var itemBoxs = prefab.GetComponentsInChildren<CItemBox>(true);
        //        foreach(var itembox in itemBoxs)
        //        {
        //            var images = itembox.GetComponentsInChildren<Image>(true);
        //            foreach(var image in images)
        //            {
        //                image.sprite = emptySprite;
        //            }
        //        }
        //        EditorUtility.SetDirty(prefab);
        //        AssetDatabase.Refresh();
        //    }
        //}
    }

    [MenuItem("FixChecker/Encode Lua File with UTF-8")]
    public static void EncodeAllLuaFile()
    {
        EncodeDir(AppDataPath + "/Scripts/Lua", "*.lua");
    }

    [MenuItem("FixChecker/Encode CS File with UTF-8")]
    public static void EncodeAllCSFile()
    {
        EncodeDir(AppDataPath + "/Scripts", "*.cs");
    }

    static void EncodeDir(string scriptPath, string extName)
    {
        var files = Directory.GetFiles(scriptPath, extName, SearchOption.AllDirectories);
        foreach (var file in files)
        {
            if (!File.Exists(file))
            {
                continue;
            }
            string text = File.ReadAllText(file, Encoding.UTF8);
            using (var sw = new StreamWriter(file, false, new UTF8Encoding(false)))
            {
                sw.Write(text);
                sw.Close();
            }
        }
        AssetDatabase.Refresh();
    }
}
