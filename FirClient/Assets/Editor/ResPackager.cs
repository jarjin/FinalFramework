using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using FirClient.Utility;
using System.Linq;
using FirClient.Component;
using FirClient.Define;
using Debug = UnityEngine.Debug;

public class ResPackager : BaseEditor
{
    static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();

    [MenuItem("GameAsset/Open Persistent Dir", false, 10)]
    static void OpenPersistentPath()
    {
        var path = Path.GetDirectoryName(AppDataPath) + "/PersistentData";
        if (!Directory.Exists(path))
        {
            Debug.LogError("Persistent DataPath not found!!:>" + path);
            return;
        }
        EditorUtility.RevealInFinder(path);
    }

    [MenuItem("GameAsset/Clear Persistent Data", false, 11)]
    static void ClearPersistentData()
    {
        PlayerPrefs.DeleteAll();
        var persistentPath = Path.GetDirectoryName(AppDataPath) + "/PersistentData";
        if (Directory.Exists(persistentPath))
        {
            Directory.Delete(persistentPath, true);
            Debug.Log("Delete Res Directory:>" + persistentPath);
        }
        var dataDir = new DirectoryInfo(Application.persistentDataPath);
        if (dataDir.Exists)
        {
            dataDir.Delete(true);
            Debug.Log("Delete Persistent DataPath:>" + Application.persistentDataPath);
        }
    }

    [MenuItem("GameAsset/Build All Assets", false, 102)]
    public static void BuildAllResource()
    {
        if (gameSettings.debugMode)
        {
            Debug.LogError("BuildAssetResource cannot run DebugMode!!!");
            Selection.activeObject = Util.LoadGameSettings();
            return;
        }
        BuildAssetBundles();
        BuildScriptWithDatas();     //构建脚本+配置
        PatchPackager.UpdateOrCreateIndexFile();

        AssetDatabase.Refresh();
    }

    [MenuItem("GameAsset/Build AssetBundle", false, 103)]
    public static void BuildAssetBundles()
    {
        if (gameSettings.debugMode)
        {
            Debug.LogError("BuildAssetBundles cannot run DebugMode!!!");
            return;
        }
        string resDir = StreamDir + "/res";
        if (!Directory.Exists(resDir))
        {
            Directory.CreateDirectory(resDir);
        }
        BuildResAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("GameAsset/Build ScriptWithDatas", false, 104)]
    public static void BuildScriptWithDatas()
    {
        if (gameSettings.debugMode)
        {
            Debug.LogError("Build ScriptWithDatas cannot run DebugMode!!!");
            return;
        }
        PackDatasFromGameSettings();
        PackLuaFiles();
        BuildFileIndex();
        BuildVersion();

        string luaDir = AppDataPath + "/" + AppConst.LuaTempDir;
        if (Directory.Exists(luaDir))
        {
            Directory.Delete(luaDir, true);
        }
        AssetDatabase.Refresh();
        Debug.Log("<color=gray>Build ScriptWithDatas Complete!!!</color>");
    }

    [MenuItem("GameAsset/Reimport All Asset")]
    public static void ReimportAssets()
    {
        AssetPreImporter.ReimportAssets();
    }

    /// <summary>
    /// 生成绑定素材
    /// </summary>
    public static void BuildResAssets()
    {
        FixChecker.ClearItemBoxAsset();
        var target = EditorUserBuildSettings.activeBuildTarget;
        Debug.Log("<color=magenta>BuildTarget:>>" + target + "</color>");
        maps.Clear();
        PackAssetFromGameSettings();
        PrintAssetBundleList(); 

        var buildOption = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
        BuildPipeline.BuildAssetBundles(AppConst.ABDir, maps.ToArray(), buildOption, target);
        Debug.Log("<color=gray>Build AssetBundle Complete!!!</color>");
    }

    static void PrintAssetBundleList()
    {
        foreach(var map in maps)
        {
            Debug.Log("abName: " + map.assetBundleName);
            for(int i = 0; i < map.assetNames.Length; i++)
            {
                Debug.Log("(" + i + ")--->" + map.assetNames[i]);
            }
        }
    }

    /*
    PackSingle("Shaders", "*.shader");
    PackSingle("Fonts", "*.*");
    PackMultiFile("Audios", "*.mp3");
    PackMultiFile("Prefabs", "*.prefab");
    PackMultiFile("Atlas", "*.png|*.tpsheet|*.mat");
    PackMultiDir("Maps");

    PackCompressDir("Tables", "*.bytes");
    PackCompressDir("Datas", "*.xml|*.dat");
    */
    /// <summary>
    /// 从游戏设置打包素材
    /// </summary>
    static void PackAssetFromGameSettings()
    {
        var settings = gameSettings.assetBundlePackSetting;
        if (settings == null || settings.Count == 0)
        {
            Debug.LogError("GameSettings AssetBundlePackSetting is empty!~");
            return;
        }
        foreach(var de in settings)
        {
            switch(de.packType)
            {
                case PackType.PackSingleFile:
                    PackSingle(de.assetPath, de.fileExtName);    
                break;
                case PackType.PackMultiFile:
                    PackMultiFile(de.assetPath, de.fileExtName);
                break;
                case PackType.PackMultiDirectory:
                    PackMultiDir(de.assetPath, de.fileExtName);    
                break;
            }
        }
    }

    /// <summary>
    /// 从游戏设置打包数据文件
    /// </summary>
    static void PackDatasFromGameSettings()
    {
        var settings = gameSettings.datasBundlePackSetting;
        if (settings == null || settings.Count == 0)
        {
            Debug.LogError("GameSettings DatasPackSetting is empty!~");
            return;
        }
        foreach (var de in settings)
        {
            switch (de.packType)
            {
                case PackCompressType.PackCompressDirectory:
                    PackCompressDir(de.dataPath, de.fileExtName);
                break;
            }
        }
    }

    /// <summary>
    /// 处理Lua文件
    /// </summary>
    static void PackLuaFiles()
    {
        string resPath = AppDataPath + "/StreamingAssets/";
        string luaPath = resPath + "lua/";

        //----------复制Lua文件----------------
        if (Directory.Exists(luaPath))
        {
            Directory.Delete(luaPath, true);
        }
        Directory.CreateDirectory(luaPath);
        string[] luaPaths = { AppDataPath + "/Scripts/lua/",
                              AppDataPath + "/Tolua/Lua/" };

        for (int i = 0; i < luaPaths.Length; i++)
        {
            int n = 0;
            string luaDataPath = luaPaths[i].ToLower();
            var files = Directory.GetFiles(luaDataPath, "*.*", SearchOption.AllDirectories);
            foreach (string f in files)
            {
                if (f.EndsWith(".meta")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string newpath = luaPath + newfile;
                string path = Path.GetDirectoryName(newpath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
                if (AppConst.LuaByteMode)
                {
                    EncodeLuaFile(f, newpath);
                }
                else
                {
                    File.Copy(f, newpath, true);
                }
                UpdateProgress(n++, files.Length, newpath);
            }
        }
        var allfiles = Directory.GetFiles(luaPath, "*.*", SearchOption.AllDirectories);
        var scriptFile = resPath + "scripts_" + allfiles.Count() + ".zip";
        if (File.Exists(scriptFile))
        {
            File.Delete(scriptFile);
        }
        CZip.ZipFile(scriptFile, luaPath);
        Directory.Delete(luaPath, true);
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void EncodeLuaFile(string srcFile, string outFile)
    {
        if (!srcFile.ToLower().EndsWith(".lua"))
        {
            File.Copy(srcFile, outFile, true);
            return;
        }
        bool isWin = true;
        string luaexe = string.Empty;
        string args = string.Empty;
        string exedir = string.Empty;
        string currDir = Directory.GetCurrentDirectory();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            isWin = true;
            luaexe = "luajit.exe";
            args = "-b -g " + srcFile + " " + outFile;
            exedir = AppDataPath.Replace("assets", "") + "LuaEncoder/luajit/";
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            isWin = false;
            luaexe = "./luajit";
            args = "-b -g " + srcFile + " " + outFile;
            exedir = AppDataPath.Replace("assets", "") + "LuaEncoder/luajit_mac/";
        }
        Directory.SetCurrentDirectory(exedir);
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = luaexe;
        info.Arguments = args;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = isWin;
        info.ErrorDialog = true;
        Debug.Log(info.FileName + " " + info.Arguments);

        Process pro = Process.Start(info);
        pro.WaitForExit();
        Directory.SetCurrentDirectory(currDir);
    }

    static void PackSingle(string dirName, string fileTypes)
    {
        var path = AppDataPath + "/" + dirName;
        if (!Directory.Exists(path))
        {
            Debug.LogError("PackSingle " + dirName + " not found!~");
            return;
        }
        var files = Directory.GetFiles(path, fileTypes, SearchOption.AllDirectories);

        List<string> resFiles = new List<string>();
        var build = new AssetBundleBuild();
        build.assetBundleName = Path.GetFileNameWithoutExtension(dirName) + AppConst.ExtName;
        foreach (var f in files)
        {
            if (f.EndsWith(".meta"))
            {
                continue;
            }
            var file = f.Replace('\\', '/').Replace(AppDataPath, "Assets");
            resFiles.Add(file);
        }
        build.assetNames = resFiles.ToArray();
        maps.Add(build);
    }

    static void PackMultiFile(string dirName, string fileTypes)
    {
        var path = AppDataPath + "/" + dirName;
        if (!Directory.Exists(path))
        {
            Debug.LogError("PackMultiFile " + dirName + " not found!~");
            return;
        }
        var allowedExtensions = fileTypes.Replace("*", "").Split('|');
        var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith)).ToList();

        foreach (var f in files)
        {
            var file = f.Replace('\\', '/').Replace(AppDataPath, "Assets");
            var extName = Path.GetExtension(file);
            var bundleName = file.Replace(extName, AppConst.ExtName).Replace("Assets/res/", "");

            var build = new AssetBundleBuild();
            build.assetBundleName = bundleName;
            build.assetNames = new string[] { file };
            maps.Add(build);
        }
    }

    static void PackMultiDir(string dirName, string fileTypes)
    {
        var path = AppDataPath + "/" + dirName;
        if (!Directory.Exists(path))
        {
            Debug.LogError("PackMultiDir " + dirName + " not found!~");
            return;
        }
        var theFolder = new DirectoryInfo(path);
        var dirInfo = theFolder.GetDirectories();
        foreach (var dir in dirInfo)
        {
            var bundleName = dirName.Replace("res/", "") + "/" + dir.Name;
            var files = dir.GetFiles();
            var assetNames = new List<string>();

            foreach (var file in files)
            {
                if (file.Name.EndsWith(".meta")) continue;
                var abfile = "Assets/" + dirName + "/" + dir.Name + "/" + file.Name;
                assetNames.Add(abfile);
            }
            var build = new AssetBundleBuild();
            build.assetBundleName = bundleName + AppConst.ExtName;
            build.assetNames = assetNames.ToArray();
            maps.Add(build);
        }
    }

    static void PackCompressDir(string dirName, string fileType)
    {
        var srcPath = AppDataPath + "/" + dirName;

        var allowedExtensions = fileType.Replace("*", "").Split('|');
        var files = Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith)).ToList();

        string zipName = dirName.ToLower().Remove(0, 4) + "_" + files.Count() + ".zip";
        string zipPath = AppDataPath + "/StreamingAssets/" + zipName;

        var zipExtNames = fileType.Replace("*.", "");
        CZip.ZipFile(zipPath, srcPath, zipExtNames);
    }

    static void BuildVersion()
    {
        string srcFile = AppDataPath + "/version.txt";
        string destFile = Application.streamingAssetsPath + "/version.txt";
        File.Copy(srcFile, destFile, true);
        AssetDatabase.Refresh();
    }

    static void BuildFileIndex()
    {
        string resPath = AppDataPath + "/StreamingAssets/";
        ///----------------------创建文件列表-----------------------
        string newFilePath = resPath + "/files.txt";
        if (File.Exists(newFilePath))
        {
            File.Delete(newFilePath);
        }
        var files = Directory.GetFiles(resPath, "*.*", SearchOption.AllDirectories);
        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Length; i++)
        {
            string file = files[i];
            string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

            string md5 = Util.md5file(file);
            string value = file.Replace(resPath, string.Empty).Replace('\\', '/');

            var location = ResPlaceType.StreamAsset;
            foreach (string prefix in AppConst.DataPrefixs)
            {
                if (value.StartsWith(prefix))
                {
                    location = ResPlaceType.DataDisk;
                }
            }
            sw.WriteLine(value + "|" + md5 + "|" + (int)location);
        }
        sw.Close(); fs.Close();
    }

    static void UpdateProgress(int progress, int progressMax, string desc)
    {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }
}