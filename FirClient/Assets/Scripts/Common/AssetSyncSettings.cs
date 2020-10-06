using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;

[CreateAssetMenu(fileName = "AssetSyncSettings", menuName = "My Game/AssetSyncSettings")]
public class AssetSyncSettings : SerializedScriptableObject
{
    [Title("Assets only")]
    [SerializeField]
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.CollapsedFoldout)]
    public Dictionary<string, string> AssetSyncDictionary = new Dictionary<string, string>();

    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    private void StartAssetSync()
    {
        foreach(var de in AssetSyncDictionary)
        {
            if (string.IsNullOrEmpty(de.Key) || string.IsNullOrEmpty(de.Value))
            {
                continue;
            }
            CopyFile(de.Key, de.Value);
        }
        Debug.Log("Assets Sync Completed!!!!");
    }

    private void CopyFile(string src, string dest)
    {
        if (!src.StartsWith("Assets/"))
        {
            Debug.LogError("Error Src Path!!!!");
            return;
        }
        var srcPath = GetFullPath(src);
        var destPath = GetFullPath(dest);
        File.Copy(srcPath, destPath, true);
    }

    private string GetFullPath(string path)
    {
        var dataPath = Application.dataPath;
        if (path.StartsWith("Assets"))
        {
            return dataPath + path.Replace("Assets", string.Empty);
        }
        else if (path.StartsWith("../"))
        {
            return dataPath + "/" + path;
        }
        return path;
    }
}
