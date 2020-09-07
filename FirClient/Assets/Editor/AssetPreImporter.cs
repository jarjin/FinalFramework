using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class AssetPreImporter : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        var importer = assetImporter as TextureImporter;
        TexturePreImporter.ProcTexture(assetPath, ref importer);
    }

    void OnPreprocessModel()
    {
        var importer = assetImporter as ModelImporter;
        ModelPostImporter.PreProcModel(assetPath, ref importer);
    }

    void OnPostprocessModel(GameObject model)
    {
        var importer = assetImporter as ModelImporter;
        ModelPostImporter.PostProcModel(assetPath, model);
    }

    void OnPreprocessAudio()
    {
        var importer = assetImporter as AudioImporter;
        AudioPreImporter.ProcAudio(assetPath, ref importer);
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        //当移动资源的时候  也就是重新导入资源  
        for (int i = 0; i < movedAssets.Length; i++)
        {
            Debug.Log("Reimported Asset: " +  movedAssets[i]);
        }
        //删除资源  
        for (int i = 0; i < deletedAssets.Length; i++)
        {
            Debug.Log("Deleted Asset: " +  deletedAssets[i]);
        }
        //移动资源  
        for (var i = 0; i < movedAssets.Length; i++)
        {
            Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
        }
    }

    public static void ReimportAssets()
    {
        var dataPath = Application.dataPath;
        var resPath = dataPath + "/res";

        string[] allowedExtensions = { ".png", ".jpg" };
        var files = Directory.GetFiles(resPath, "*.*", SearchOption.AllDirectories)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith)).ToList();

        foreach (var file in files)
        {
            var path = file.Replace("\\", "/").Replace(dataPath, "Assets");
            AssetDatabase.ImportAsset(path, ImportAssetOptions.Default);
        }
    }
}
