using System.Collections.Generic;
using UnityEditor;
using System.IO;
using FirClient.Utility;
using FirClient.Component;
using FirClient.Define;
using Debug = UnityEngine.Debug;

class PatchInfo
{
    public string file;
    public string md5;

    public PatchInfo(string file, string md5)
    {
        this.file = file;
        this.md5 = md5;
    }

    public string ToString()
    {
        return file + "|" + md5;
    }
}
public class PatchPackager : BaseEditor
{
    static string patchPath = AppDataPath + "/Patchs/";
    static VersionInfo localVerInfo = null;

    [MenuItem("GameAsset/Build Game Patch")]
    public static void BuildPatch() 
    {
        var verfile = AppDataPath + "/version.txt";
        localVerInfo = Util.GetVersionInfo(verfile);
        if (localVerInfo == null)
        {
            Debug.LogError("Don't find version info, make first!!!");
            return;
        }
        if (TryCreateDir(patchPath)) 
        {
            UpdateOrCreateIndexFile();
            Debug.Log("First create index file.");
        }
        else
        {
            var needUpdateFiles = FindNeedUpdateFiles();
            if (needUpdateFiles != null && needUpdateFiles.Count > 0)
            {
                BuildPatchInternal(needUpdateFiles);
                //UpdateOrCreateIndexFile();
            }
            else
            {
                Debug.LogError("Don't find need update file.");
            }
        }
        AssetDatabase.Refresh();
    }

    public static void UpdateOrCreateIndexFile()
    {
        var indexfile = AppDataPath + "/Patchs/PatchIndex.txt";
        if (File.Exists(indexfile))
        {
            File.Delete(indexfile);
        }
        foreach (var path in AppConst.AssetPaths)
        {
            string fullPath = AppDataPath + path;
            string[] files = Directory.GetFiles(fullPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.EndsWith(".meta")) continue;
                var md5 = Util.md5file(file);
                var newPath = file.Replace('\\', '/')
                                  .Replace(AppDataPath + "/", "")
                                  .Replace("StreamingAssets/", "")
                                  .ToLower();
                var patch = new PatchInfo(newPath, md5);
                File.AppendAllLines(indexfile, new [] { patch.ToString() });
            }
        }
        AssetDatabase.Refresh();
        Debug.Log("CreatePatchIndexFile OK!");
    }

    static List<string> FindNeedUpdateFiles() 
    {
        var oldPatchList = GetOldPatchList();
        List<string> updateList = new List<string>();
        foreach (var path in AppConst.AssetPaths)
        {
            string fullPath = AppDataPath + path;
            string[] files = Directory.GetFiles(fullPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.EndsWith(".meta")) continue;
                var md5 = Util.md5file(file);
                var newfile = file.Replace('\\', '/');
                var newPath = newfile.Replace(AppDataPath + "/", "")
                                     .Replace("StreamingAssets/", "")
                                     .ToLower();

                PatchInfo patch = null;
                if (oldPatchList.TryGetValue(newPath, out patch))
                {
                    if (patch.md5 != md5) { //md5值不同添加更新列表
                        updateList.Add(newfile);
                    }
                }
                else
                {
                    updateList.Add(newfile);   //不存在直接添加更新列表
                }
            }
        }
        return updateList;
    }

    /// <summary>
    /// 获取老的更新列表
    /// </summary>
    /// <returns></returns>
    static Dictionary<string, PatchInfo> GetOldPatchList()
    {
        var indexfile = AppDataPath + "/Patchs/PatchIndex.txt";
        if (!File.Exists(indexfile))
        {
            return null;
        }
        var patchList = new Dictionary<string, PatchInfo>();
        var lines = File.ReadAllLines(indexfile);
        foreach(var line in lines)
        {
            if (string.IsNullOrEmpty(line)) {
                continue;
            }
            var strs = line.Split('|');
            patchList.Add(strs[0], new PatchInfo(strs[0], strs[1]));
        }
        return patchList;
    }

    /// <summary>
    /// 开始打补丁
    /// </summary>
    /// <param name="needUpdateFiles"></param>
    static void BuildPatchInternal(List<string> needUpdateFiles) 
    {
        var fileSize = 0L;
        var fileCount = needUpdateFiles.Count;
        var patchVerDir = localVerInfo.mainVersion + "_" + localVerInfo.primaryVersion;

        RetryCreateDir(patchPath + "temps/");
        RetryCreateDir(patchPath + "files/");
        RetryCreateDir(patchPath + "files/" + patchVerDir);

        foreach(var item in needUpdateFiles)
        {
            fileSize += FileSize(item);
            Debug.Log("update file:>" + item);
            var currPath = item.Replace(AppDataPath, "")
                               .Replace("StreamingAssets/", "");
            currPath = patchPath + "temps" + currPath;
            var dir = currPath.Substring(0, currPath.LastIndexOf('/'));
            if (!Directory.Exists(dir)) 
            {
                Directory.CreateDirectory(dir.ToLower());
            }
            if (File.Exists(currPath)) 
            {
                File.Delete(currPath);
            }
            File.Copy(item, currPath);  //复制文件
        }
        var fileName = "patch_" + localVerInfo.patchVersion + "_" + fileCount + ".zip";

        var zipFile = patchPath + "files/" + patchVerDir + "/" + fileName;
        CZip.ZipFile(zipFile, patchPath + "temps/");
        Directory.Delete(patchPath + "temps/", true);        

        string patchInfo = AppConst.PatchUrl + "files/" + patchVerDir + "/" + fileName + "|" + fileSize;
        File.AppendAllLines(patchPath + "patchs_" + patchVerDir + ".txt", new[] { patchInfo });

        Debug.Log("Create Patch Zipfile:>" + zipFile);
        AssetDatabase.Refresh();
    }
}
