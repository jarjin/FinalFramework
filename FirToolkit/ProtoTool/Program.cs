using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ProtoTool
{
    struct ProtoCfgInfo
    {
        public string protoDir;
        public string protopb;
        public List<string> protocs;
        public List<string> protofiles;
    }

    class Program
    {
        static string currDir;
        static ProtoCfgInfo protoCfg;

        static void Execute(string proc, string args, string workdir)
        {
            Console.WriteLine(proc + " " + args);

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = proc;
            info.Arguments = args;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.WorkingDirectory = workdir;

            Process pro = Process.Start(info);
            pro.WaitForExit();

            string msg = pro.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(msg))
            {
                Console.WriteLine(msg);
            }
        }

        static void CompileProtoCS(string protoPath)
        {
            var name = Path.GetFileNameWithoutExtension(protoPath);
            var protogenPath = currDir + "Tools/protoc.exe";

            foreach (var path in protoCfg.protocs)
            {
                var rpath = protoPath.Replace(protoCfg.protoDir, string.Empty).Replace(".proto", ".cs");
                var csPath = path + rpath;
                if (File.Exists(csPath))
                {
                    File.Delete(csPath);
                }
                var cscDir = Path.GetDirectoryName(csPath);
                if (!Directory.Exists(cscDir))
                {
                    Directory.CreateDirectory(cscDir);
                }
                var incDir = Path.GetDirectoryName(protoPath);
                Execute(protogenPath, "--proto_path=" + incDir + " --csharp_out=" + cscDir + " " + protoPath, protoCfg.protoDir);
            }
        }

        static void CompileProtoPB(string protoPath)
        {
            var rpath = protoPath.Replace(protoCfg.protoDir, string.Empty).Replace(".proto", ".pb");
            var luapbPath = protoCfg.protopb + rpath;
            if (File.Exists(luapbPath))
            {
                File.Delete(luapbPath);
            }
            var luapbDir = Path.GetDirectoryName(luapbPath);
            if (!Directory.Exists(luapbDir))
            {
                Directory.CreateDirectory(luapbDir);
            }
            var protocPath = currDir + "Tools/protoc.exe";
            var protoDir = Path.GetDirectoryName(protoPath);
            Execute(protocPath, " -o " + luapbPath + " " + protoPath + " -I=" + protoDir, protoCfg.protoDir);
        }

        static bool ParseConfig()
        {
            currDir = Environment.CurrentDirectory + "/";
            string cfgFile = currDir + "protocfg.txt";
            Console.WriteLine(cfgFile);

            protoCfg = new ProtoCfgInfo();
            protoCfg.protocs = new List<string>();
            protoCfg.protofiles = new List<string>();

            if (File.Exists(cfgFile))
            {
                var lines = File.ReadAllLines(cfgFile);
                foreach(var line in lines)
                {
                    if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
                    {
                        continue;
                    }
                    var strs = line.Split('=');
                    switch(strs[0].Trim())
                    {
                        case "protodir":
                            protoCfg.protoDir = (currDir + strs[1].Trim()).Replace('\\', '/');
                        break;
                        case "protopb":
                            protoCfg.protopb = (currDir + strs[1].Trim()).Replace('\\', '/');
                        break;
                        case "protocs":
                            var paths = strs[1].Split('|');
                            foreach(var path in paths)
                            {
                                protoCfg.protocs.Add((currDir + path.Trim()).Replace('\\', '/'));
                            }
                        break;
                        case "file":
                            protoCfg.protofiles.Add(strs[1].Trim());
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            if (!ParseConfig())
            {
                Console.WriteLine("Don't found protocfg.txt!!!");
                return;
            }
            handleProtoPath();
            foreach (var file in protoCfg.protofiles)
            {
                CompileProtoCS(file);
                Console.WriteLine();

                CompileProtoPB(file);
                Console.WriteLine();
            }
            Console.WriteLine("Build Photo OK!!!");
            Console.ReadKey();
        }

        static void handleProtoPath()
        {
            var files = Directory.GetFiles(protoCfg.protoDir, "*.proto", SearchOption.AllDirectories);
            var protofiles = protoCfg.protofiles;
            for(int i = 0; i < files.Length; i++)
            {
                var filename = Path.GetFileName(files[i]);
                var index = protoCfg.protofiles.IndexOf(filename);
                protoCfg.protofiles[index] = files[i].Replace('\\', '/');
            }
        }
    }
}
