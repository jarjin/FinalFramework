using System;
using System.Collections.Generic;
using System.IO;

namespace FirSyncTool
{
    //copy $(TargetPath) $(SolutionDir)..\
    class Program
    {
        class SyncDataInfo
        {
            public string srcPath;
            public string destPath;
            public bool isDirectory;
            public List<string> includes;
            public List<string> excludes;
        }
        static List<SyncDataInfo> maps = new List<SyncDataInfo>();

        static void Main(string[] args)
        {
            ParseSyncMap();
            if (maps.Count > 0)
            {
                foreach(SyncDataInfo syncData in maps)
                {
                    SyncDirOrFile(syncData);
                }
            }
            Console.ReadKey();
        }

        static void ParseSyncMap()
        {
            maps.Clear();
            string currDir = Environment.CurrentDirectory;
            string mapFile = currDir + "/syncfg.txt";
            if (File.Exists(mapFile))
            {
                var lines = File.ReadAllLines(mapFile);
                foreach(string line in lines)
                {
                    if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
                    {
                        continue;
                    }
                    var syncData = new SyncDataInfo();
                    if (line.Contains(":"))
                    {
                        var strs = line.Split(':');
                        if (ParseKeyValue(strs[0], ref syncData))
                        {
                            ParseExtName(strs[1], ref syncData);
                        }
                    }
                    else
                    {
                        ParseKeyValue(line, ref syncData);
                    }
                }
            }
        }

        static void ParseExtName(string line, ref SyncDataInfo syncData)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
            {
                return;
            }
            if (line[0] == '+')
            {
                var newLine = line.Remove(0, 1);
                syncData.includes = new List<string>(newLine.Split(','));
            } 
            else if (line[0] == '-')
            {
                var newLine = line.Remove(0, 1);
                syncData.excludes = new List<string>(newLine.Split(','));
            }
        }

        static bool ParseKeyValue(string line, ref SyncDataInfo syncData)
        {
            line = line.Trim();
            var strs = line.Split('=');
            var currDir = Environment.CurrentDirectory;
            if (!string.IsNullOrEmpty(strs[0]) && !string.IsNullOrEmpty(strs[1]))
            {
                syncData.srcPath = (currDir + "/" + strs[0].Trim()).Replace('\\', '/');
                syncData.destPath = (currDir + "/" + strs[1].Trim()).Replace('\\', '/');
                syncData.isDirectory = !syncData.srcPath.Contains(".") && !syncData.destPath.Contains(".");
                maps.Add(syncData);
                return true;
            }
            return false;
        }

        static void SyncDirOrFile(SyncDataInfo syncData)
        {
            string currDir = Environment.CurrentDirectory;
            if (syncData.isDirectory)    //文件夹
            {
                CopyFolder(syncData.srcPath, syncData.destPath, syncData.includes, syncData.excludes);
            }
            else
            {
                if (File.Exists(syncData.destPath))
                {
                    File.Delete(syncData.destPath);
                }
                File.Copy(syncData.srcPath, syncData.destPath, true);
                Console.WriteLine("[{0}]=>[{1}]", syncData.srcPath, syncData.destPath);
            }
        }

        /// <summary>
        /// 复制文件夹及文件
        /// </summary>
        static int CopyFolder(string sourceFolder, string destFolder, List<string> includes, List<string> excludes)
        {
            try
            {
                if (Directory.Exists(destFolder))
                {
                    Directory.Delete(destFolder, true);
                }
                Directory.CreateDirectory(destFolder);
                string[] files = Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    var extName = Path.GetExtension(file).ToLower();
                    if (includes != null)
                    {
                        if (includes.Contains(extName))
                        {
                            CopyFile(file, destFolder);
                        }
                    }
                    if (excludes != null)
                    {
                        if (!excludes.Contains(extName))
                        {
                            CopyFile(file, destFolder);
                        }
                    }
                }
                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    string dest = Path.Combine(destFolder, name);
                    CopyFolder(folder, dest, includes, excludes);
                }
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        static void CopyFile(string file, string destDir)
        {
            string name = Path.GetFileName(file);
            string dest = Path.Combine(destDir, name);
            File.Copy(file, dest);
            Console.WriteLine("[{0}]=>[{1}]", file, dest);
        }
    }
}
