using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnumTool
{
    public class EnumInfo
    {
        public string name;
        public Dictionary<string, string> values;
    }

    class Program
    {
        static string javaCodePath = string.Empty;
        static string csharpCodePath = string.Empty;
        static string csharpTemplate = string.Empty;
        static string javaTemplate = string.Empty;
        static List<EnumInfo> _dic = new List<EnumInfo>();

        static void ParseConfig()
        {
            javaTemplate = Environment.CurrentDirectory + "\\Resource\\Templates\\JavaEnum.txt";
            csharpTemplate = Environment.CurrentDirectory + "\\Resource\\Templates\\C#Enum.txt";

            javaCodePath = Environment.CurrentDirectory + "\\FirServer\\FirServer\\src\\com\\tables\\enum";
            csharpCodePath = Environment.CurrentDirectory + "\\FirClient\\Assets\\Scripts\\Data\\Enum";
        }

        static void ParseEnum()
        {
            string filepath = Environment.CurrentDirectory + "\\Resource\\Enum.txt";
            Console.WriteLine(filepath);

            if (File.Exists(filepath))
            {
                EnumInfo info = null;

                var sources = File.ReadAllLines(filepath);
                foreach (var source in sources)
                {
                    var line = source.Trim();
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#") || line.StartsWith("{"))
                    {
                        continue;
                    }
                    if (line.StartsWith("enum"))
                    {
                        info = new EnumInfo();

                        Console.WriteLine(line);
                        var strs = line.Split(' ');
                        info.name = strs[1];
                        info.values = new Dictionary<string, string>();
                    }
                    else if(line.StartsWith("}"))
                    {
                        _dic.Add(info);
                    }
                    else
                    {
                        var strs = line.Split("//".ToCharArray());
                        var newStrs = strs[0].Trim().Split('=');

                        var key = newStrs[0].Trim();
                        var value = newStrs[1].Trim().TrimEnd(',');

                        info.values.Add(key, value);
                        Console.WriteLine(key + " " + value);
                    }
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
            ParseEnum();

            if (string.IsNullOrEmpty(javaCodePath))
            {
                Console.WriteLine("javaCodePath was null, check protocfg.txt!!");
                Console.ReadKey();
                return;
            }
            if (string.IsNullOrEmpty(csharpCodePath))
            {
                Console.WriteLine("csharpCodePath was null, check protocfg.txt!!");
                Console.ReadKey();
                return;
            }
            if (_dic.Count > 0)
            {
                //WriteJavaProtocal();
                //WriteCSharpProtocal();
            }
            Console.ReadKey();
        }
    }
}
