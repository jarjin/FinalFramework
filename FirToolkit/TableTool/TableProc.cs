using OfficeOpenXml;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace TableTool
{
    enum TableType
    {
        Common = 0,
        Client = 1,
        Server = 2,
    }

    class TablePath
    {
        public string path;
        public string dllpath;

        public TablePath(string p, string n)
        {
            path = p;
            dllpath = n;
        }
    }

    class TableCompileInfo
    {
        public string tableName;
        public string tablePath;
        public TableType tableType;
        public string sheetName;
        public string tableCode;
    }

    public class TableProc
    {
        static fmMain fmMain;
        static string excelDataPath, clientDllPath, serverDllPath;
        static string clientDataPath, clientCodePath, serverDataPath, serverCodePath, templateDir;
        static StringBuilder manager_vars, manager_vars_c, manager_vars_s;
        static StringBuilder manager_load_funcs, manager_load_funcs_c, manager_load_funcs_s;
        static List<TableCompileInfo> compileInfos = new List<TableCompileInfo>();
        static Dictionary<string, string> md5Values = new Dictionary<string, string>();

        /// <summary>
        /// 导入所有的EXCEL表
        /// </summary>
        public static void Start(fmMain main, string excelPath, string clientData, string clientCode, 
                        string serverData, string serverCode, string tempDir, string clientDll, string serverDll)
        {
            fmMain = main;
            excelDataPath = excelPath;
            clientDataPath = clientData;
            clientCodePath = clientCode;
            serverDataPath = serverData;
            serverCodePath = serverCode;
            templateDir = tempDir;
            clientDllPath = fmMain.currDir + clientDll;
            serverDllPath = fmMain.currDir + serverDll;

            md5Values.Clear();
            compileInfos.Clear();

            manager_vars = new StringBuilder();
            manager_vars_c = new StringBuilder();
            manager_vars_s = new StringBuilder();

            manager_load_funcs = new StringBuilder();
            manager_load_funcs_c = new StringBuilder();
            manager_load_funcs_s = new StringBuilder();

            string[] files = Directory.GetFiles(excelDataPath, "*.xlsx");
            if (files.Length == 0)
            {
                return;
            }
            for (int i = 0; i < files.Length; i++)
            {
                var excelFile = files[i].Replace('\\', '/');
                ParseProcTable(excelFile, i + 1, files.Length);
            }
            CreateTableManager();

            fmMain.md5Values.Clear();
            foreach(var de in md5Values)
            {
                fmMain.md5Values.Add(de.Key, de.Value);
            }
            fmMain.SaveTableMd5();          //保存表的MD5值
            ExecuteExportTables();   //生成数据文件
            MessageBox.Show("处理完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static bool IsNewOrUpdateTable(string tableName, string newmd5)
        {
            if (!fmMain.md5Values.ContainsKey(tableName))
            {
                return true;
            }
            var oldmd5 = fmMain.md5Values[tableName];      //老的md5值

            if (newmd5 != oldmd5)
            {
                fmMain.md5Values[tableName] = newmd5;
                return true;
            }
            return false;
        }

        static string FirstCharToLower(string input)
        {
            if (String.IsNullOrEmpty(input)) return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 分析处理表
        /// </summary>
        static void ParseProcTable(string excelFile, float curr, float max)
        {
            var md5 = md5file(excelFile);

            var name = Path.GetFileNameWithoutExtension(excelFile);
            string tableName = name + "Table";

            if (!md5Values.ContainsKey(tableName))
            {
                md5Values.Add(tableName, md5);
            }
            using (var fs = new FileStream(excelFile, FileMode.Open))
            {
                using (ExcelPackage package = new ExcelPackage(fs))
                {
                    ExcelWorksheet sheet = null;
                    var count = package.Workbook.Worksheets.Count;
                    for (int i = 1; i <= count; ++i)
                    {
                        sheet = package.Workbook.Worksheets[i];
                        if (sheet.Cells.Count() == 0)
                        {
                            continue;
                        }
                        ProcWorkSheet(tableName, excelFile, sheet, md5);
                    }
                }
            }
        }

        static void ProcWorkSheet(string tableName, string excelFile, ExcelWorksheet sheet, string md5)
        {
            var sheetName = sheet.Name.ToLower();
            Console.WriteLine("{0}, {1}", tableName + " " + sheetName, sheet.Cells.Count());

            StringBuilder vars = null;
            StringBuilder load_funcs = null;

            TableType tableType = TableType.Common;
            var destPaths = new List<string>();
            if (sheetName.ToLower() == "client")
            {
                tableType = TableType.Client;
                vars = manager_vars_c;
                load_funcs = manager_load_funcs_c;
                destPaths.Add(clientCodePath);
            }
            else if (sheetName.ToLower() == "server")
            {
                tableType = TableType.Server;
                vars = manager_vars_s;
                load_funcs = manager_load_funcs_s;
                destPaths.Add(serverCodePath);
            }
            else
            {
                vars = manager_vars;
                load_funcs = manager_load_funcs;
                destPaths.Add(clientCodePath);
                destPaths.Add(serverCodePath);
            }
            var varName = FirstCharToLower(tableName);
            vars.AppendLine("    public " + tableName + " " + varName + ";");
            load_funcs.AppendLine("        " + varName + " = LoadData<" + tableName + ">(\"Tables/" + tableName + ".bytes\");");
            load_funcs.AppendLine("        " + varName + ".Initialize();");

            if (IsNewOrUpdateTable(tableName, md5))
            {
                var tableCode = string.Empty;
                foreach (var path in destPaths)
                {
                    string destDir = path + "/Tables";
                    tableCode = CreateTableWithItemFile(tableName, excelFile, destDir, sheet);     //创建TABLE
                }
                var compileInfo = new TableCompileInfo();
                compileInfo.tableName = tableName;
                compileInfo.tablePath = excelFile;
                compileInfo.tableType = tableType;
                compileInfo.sheetName = sheetName;
                compileInfo.tableCode = tableCode;
                compileInfos.Add(compileInfo);
            }
        }

        /// <summary>
        /// 创建表结构跟ITEM文件
        /// </summary>
        static string CreateTableWithItemFile(string name, string excelPath, string destDir, ExcelWorksheet sheet)
        {
            int colNum = sheet.Dimension.End.Column;

            string keyType = string.Empty;
            var varBody = new StringBuilder();

            for (int i = 1; i <= colNum; i++)
            {
                string key = sheet.GetValue(3, i).ToString();
                if (key.Trim() == "desc")
                {
                    continue;
                }
                string value = sheet.GetValue(2, i).ToString().ToLower();

                if (i == 1)
                {
                    keyType = value;
                }
                varBody.AppendLine("    public " + value + " " + key + ";");
            }
            var tableItemCode = File.ReadAllText(templateDir + "/TableWithItem.txt");
            string txtCode = tableItemCode.Replace("[NAME]", name)
                                          .Replace("[BODY]", varBody.ToString())
                                          .Replace("[TYPE]", keyType);

            txtCode = txtCode.Replace("[TIME]", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss dddd"));
            var csPath = destDir + "/" + name + ".cs";
            File.WriteAllText(csPath, txtCode);
            return txtCode;
        }

        /// <summary>
        /// 生成TableManager类
        /// </summary>
        static void CreateTableManager()
        {
            var tempfile = templateDir + "/TableManager.txt";
            if (!File.Exists(tempfile))
            {
                return;
            }
            var managerCode = File.ReadAllText(tempfile);

            manager_vars_c.Insert(0, manager_vars.ToString());
            manager_vars_s.Insert(0, manager_vars.ToString());

            manager_load_funcs_c.Insert(0, manager_load_funcs.ToString());
            manager_load_funcs_s.Insert(0, manager_load_funcs.ToString());

            if (manager_vars_c.Length > 0 && manager_load_funcs_c.Length > 0)
            {
                WriteTableManagerFile(managerCode, manager_vars_c, manager_load_funcs_c, clientCodePath);
            }
            if (manager_vars_s.Length > 0 && manager_load_funcs_s.Length > 0)
            {
                WriteTableManagerFile(managerCode, manager_vars_s, manager_load_funcs_s, serverCodePath);
            }
        }

        static void WriteTableManagerFile(string tempText, StringBuilder varText, StringBuilder funcText, string filename)
        {
            varText.AppendLine("///[APPEND_VAR]");
            funcText.AppendLine("///[APPEND_TABLE]");

            var content = tempText.Replace("[DECLARE_TABLES_VARS]", varText.ToString());
            content = content.Replace("[LOAD_TABLE_FUNCS]", funcText.ToString());

            filename += "/TableManager.cs";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            File.WriteAllText(filename, content);
        }

        static string md5file(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// //////////////////////////////////////////////生成二进制文件////////////////////////////////////////////////////////
        static void ExecuteExportTables()
        {
            var curr = 0;
            var max = compileInfos.Count;
            foreach (var info in compileInfos)
            {
                var name = info.tableName;
                var type = info.tableType;
                var sheet = info.sheetName;
                var path = info.tablePath;
                var code = info.tableCode;
                CreateTableDataWithFile(name, type, sheet, path, code);
                fmMain.SetProgress(++curr, max);
            }
            if (File.Exists(clientDllPath))
            {
                File.Delete(clientDllPath);
            }
            if (File.Exists(serverDllPath))
            {
                File.Delete(serverDllPath);
            }
        }

        static void CreateTableDataWithFile(string tbName, TableType tbType, string sheetName, string excelPath, string code)
        {
            using (var fs = new FileStream(excelPath, FileMode.Open))
            {
                using (var package = new ExcelPackage(fs))
                {
                    ExcelWorksheet sheet = null;
                    for (int i = 1; i <= package.Workbook.Worksheets.Count; ++i)
                    {
                        sheet = package.Workbook.Worksheets[i];
                        if (sheet.Name.ToLower() == sheetName)
                        {
                            break;
                        }
                    }
                    if (sheet != null)
                    {
                        int colNum = sheet.Dimension.End.Column;
                        int rowNum = sheet.Dimension.End.Row;

                        var valueType = new Dictionary<string, string>();
                        for (int i = 1; i <= colNum; i++)
                        {
                            string key = sheet.GetValue(3, i).ToString();
                            if (key.Trim() == "desc")
                            {
                                continue;
                            }
                            string value = sheet.GetValue(2, i).ToString().ToLower();
                            valueType.Add(key, value);
                        }
                        ////////////////////////////////////////////////////////////////////////////////////
                        WriteToFile(tbName, tbType, code, rowNum, valueType, sheet);
                        Console.WriteLine("CreateTableBodyWithFile " + tbName + " OK!!!");
                    }
                }
            }
        }

        static object CreateDll(string code, string tbName, int rowNum, Dictionary<string, string> valueType, ExcelWorksheet sheet, string assemblyPath)
        {
            var assembly = CompileCodeAssembly(code, assemblyPath);
            var instance = assembly.CreateInstance(tbName);

            var tableType = assembly.GetType(tbName);
            var property = tableType.GetField("name");
            property.SetValue(instance, tbName);

            var methodInfo = tableType.GetMethod("AddItem", BindingFlags.Instance | BindingFlags.Public);
            var tableItemType = assembly.GetType(tbName + "Item");

            for (int i = 4; i <= rowNum; i++)
            {
                int j = 1;
                var tbItemInst = Activator.CreateInstance(tableItemType);
                foreach (var de in valueType)
                {
                    string prop = de.Key;
                    var type = valueType[prop].ToLower();
                    var value = sheet.GetValue(i, j++).ToString();
                    object objValue = null;
                    switch (type)
                    {
                        case "int":
                            objValue = int.Parse(value);
                            break;
                        case "uint":
                            objValue = uint.Parse(value);
                            break;
                        case "string":
                            objValue = value;
                            break;
                        case "bool":
                            objValue = value == "1" ? true : false;
                            break;
                        case "float":
                            objValue = float.Parse(value);
                            break;
                    }
                    tbItemInst.GetType().GetField(prop).SetValue(tbItemInst, objValue);
                }
                methodInfo.Invoke(instance, new object[] { tbItemInst });
            }
            return instance;
        }

        /// <summary>
        /// 将表数据写入文件
        /// </summary>
        static void WriteToFile(string tbName, TableType tbType, string code, int rowNum, Dictionary<string, string> valueType, ExcelWorksheet sheet)
        {
            var bytesPaths = new List<TablePath>();
            switch(tbType)
            {
                case TableType.Client:
                    bytesPaths.Add(new TablePath(clientDataPath + "/" + tbName, clientDllPath));
                    break;
                case TableType.Server:
                    bytesPaths.Add(new TablePath(serverDataPath + "/" + tbName, serverDllPath));
                    break;
                case TableType.Common:
                    bytesPaths.Add(new TablePath(clientDataPath + "/" + tbName, clientDllPath));
                    bytesPaths.Add(new TablePath(serverDataPath + "/" + tbName, serverDllPath));
                    break;
            }
            foreach(var bytePath in bytesPaths)
            {
                var binraryPath = bytePath.path + ".bytes";
                if (File.Exists(binraryPath))
                {
                    File.Delete(binraryPath);
                }
                var outputPath = Path.GetDirectoryName(binraryPath);
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                IFormatter serializer = new BinaryFormatter();
                using (var saveFile = new FileStream(binraryPath, FileMode.Create, FileAccess.Write))
                {
                    var dllPath = bytePath.dllpath;
                    var instance = CreateDll(code, tbName, rowNum, valueType, sheet, dllPath);
                    serializer.Serialize(saveFile, instance);
                }
            }
        }

        /// <summary>
        /// 编译代码
        /// </summary>
        static Assembly CompileCodeAssembly(string classCode, string assemblyPath)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters paras = new CompilerParameters();
            paras.ReferencedAssemblies.Add("System.dll");
            paras.ReferencedAssemblies.Add("System.Xml.dll");
            paras.GenerateExecutable = false;
            paras.GenerateInMemory = true;
            paras.OutputAssembly = assemblyPath;

            CompilerResults result = provider.CompileAssemblyFromSource(paras, classCode);
            if (result.Errors.HasErrors)
            {
                string ErrorMessage = "";
                foreach (CompilerError err in result.Errors)
                {
                    ErrorMessage += err.ErrorText;
                }
                Console.WriteLine(ErrorMessage);
                return null;
            }
            return result.CompiledAssembly;
        }
    }
}
