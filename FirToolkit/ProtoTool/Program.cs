using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProtoTool
{
    class Program
    {
        static string javaCodePath = string.Empty;
        static string csharpCodePath = string.Empty;
        static string csharpTemplate = string.Empty;
        static string javaTemplate = string.Empty;
        static Dictionary<string, string> _dic = new Dictionary<string, string>();

        static void ParseConfig()
        {
            javaTemplate = Environment.CurrentDirectory + "\\Resource\\Templates\\JavaProtocal.txt";
            csharpTemplate = Environment.CurrentDirectory + "\\Resource\\Templates\\C#Protocal.txt";

            javaCodePath = Environment.CurrentDirectory + "\\FirServer\\FirServer\\src\\com\\common\\Protocal.java";
            csharpCodePath = Environment.CurrentDirectory + "\\FirClient\\Assets\\Scripts\\Network\\Protocal.cs";
        }

        static void ParseProtocal()
        {
            string filepath = Environment.CurrentDirectory + "\\Resource\\Protocal.txt";
            Console.WriteLine(filepath);

            if (File.Exists(filepath))
            {
                var sources = File.ReadAllLines(filepath);
                foreach (var source in sources)
                {
                    var line = source.Trim();
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;
                    var strs = line.Split("//".ToCharArray());
                    var newStrs = strs[0].Trim().Split('=');
                    _dic.Add(newStrs[0].Trim(), newStrs[1].Trim());
                }
            }
        }

        static void WriteCSharpProtocal()
        {
            var strs = new StringBuilder();
            foreach (var kvp in _dic)
            {
                var str = string.Format("   public string {0} = {1};", kvp.Key, kvp.Value);
                Console.WriteLine(str);
                strs.AppendLine(str);
            }
            WriteFile(csharpCodePath, csharpTemplate, strs.ToString());
            Console.WriteLine("Build CSharp Photocal OK!!!:" + csharpCodePath);
        }

        static void WriteJavaProtocal()
        {
            var strs = new StringBuilder();
            foreach (var kvp in _dic)
            {
                var str = string.Format("   public String {0} = {1};", kvp.Key, kvp.Value);
                Console.WriteLine(str);
                strs.AppendLine(str);
            }
            WriteFile(javaCodePath, javaTemplate, strs.ToString());
            Console.WriteLine("Build Java Photocal OK!!!:" + javaCodePath);
        }

        static void WriteFile(string filePath, string templateFile, string content)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var strs = File.ReadAllText(templateFile);
            strs = strs.Replace("[BODY]", content);
            File.WriteAllText(filePath, strs);
        }

        static void Main(string[] args)
        {
            ParseConfig();
            ParseProtocal();

            if (string.IsNullOrEmpty(javaCodePath))
            {
                System.Console.WriteLine("javaCodePath was null, check protocfg.txt!!");
                Console.ReadKey();
                return;
            }
            if (string.IsNullOrEmpty(csharpCodePath))
            {
                System.Console.WriteLine("csharpCodePath was null, check protocfg.txt!!");
                Console.ReadKey();
                return;
            }
            if (_dic.Count > 0)
            {
                WriteJavaProtocal();
                WriteCSharpProtocal();
            }
            Console.ReadKey();
        }
    }
}
