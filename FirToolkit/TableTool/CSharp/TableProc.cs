using FirCommon.Data;
using OfficeOpenXml;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TableTool
{
    public partial class TableProc
    {
        static void HandleCSharpWorkSheet(string tableName, string sheetName, string excelFile, ExcelWorksheet sheet, string md5, TableType tableType, string destPath)
        {
            var varName = tableName.FirstCharToLower();
            vars.AppendLine("    	public " + tableName + " " + varName + ";");
            load_funcs.AppendLine("        	" + varName + " = LoadData<" + tableName + ">(\"Tables/" + tableName + ".bytes\");");
            load_funcs.AppendLine("        	" + varName + ".Initialize();");

            if (IsNewOrUpdateTable(tableName, md5))
            {
                string destDir = destPath + "/Tables";
                var tableCode = CreateCSharpTableWithItem(tableName, excelFile, destDir, sheet);     //创建TABLE
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
        static string CreateCSharpTableWithItem(string name, string excelPath, string destDir, ExcelWorksheet sheet)
        {
            int colNum = sheet.Dimension.End.Column;

            string keyType = string.Empty;
            var varBody = new StringBuilder();

            for (int i = 1; i <= colNum; i++)
            {
                var varName = sheet.GetValue(4, i) as string;
                if (string.IsNullOrEmpty(varName))
                {
                    continue;
                }
                if (varName.Trim() == "note")
                {
                    continue;
                }
                string varType = sheet.GetValue(2, i).ToString();

                if (i == 1)
                {
                    keyType = varType;
                }
                if (varType == "enum")
                {
                    var extraParam = sheet.GetValue(3, i) as string;
                    varType = GetEnumType(extraParam).typeName;
                }
                varBody.AppendLine("    	public " + varType + " " + varName + ";");
            }
            var tableItemCode = File.ReadAllText(templateDir + "/C#Table.txt");
            var varText = varBody.ToString().TrimEnd('\n', '\t', '\r');
            string txtCode = tableItemCode.Replace("[NAME]", name)
                                          .Replace("[BODY]", varText)
                                          .Replace("[TYPE]", keyType);

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            txtCode = txtCode.Replace("[TIME]", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss dddd"));
            var csPath = destDir + "/" + name + ".cs";
            File.WriteAllText(csPath, txtCode, new UTF8Encoding(false));
            return txtCode;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 生成TableManager类
        /// </summary>
        static void CreateCSharpTableManager(TableType type)
        {
            var tempfile = templateDir + "/C#TableManager.txt";
            if (File.Exists(tempfile))
            {
                var managerCode = File.ReadAllText(tempfile);
                if (type == TableType.CSharp)
                {
                    WriteCSharpTableManager(managerCode, csharpCodePath);
                }
                if (type == TableType.Server)
                {
                    WriteCSharpTableManager(managerCode, serverCodePath);
                }
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
                            string varName = sheet.GetValue(4, i) as string;
                            if (string.IsNullOrEmpty(varName) || varName.Trim() == "note")
                            {
                                continue;
                            }
                            string varType = sheet.GetValue(2, i).ToString();
                            valueType.Add(varName, varType);
                        }
                        WriteToBinaryFile(tbName, tbType, code, rowNum, valueType, sheet);
                        Console.WriteLine("CreateTableBodyWithFile " + tbName + " OK!!!");
                    }
                }
            }
        }

        /// <summary>
        /// 写入C#表管理器
        /// </summary>
        static void WriteCSharpTableManager(string tempText, string pathname)
        {
            vars.AppendLine("///[APPEND_VAR]");
            load_funcs.AppendLine("///[APPEND_TABLE]");

            var loadfuncText = load_funcs.ToString().TrimEnd('\n', '\t', '\r');
            var content = tempText.Replace("[DECLARE_TABLES_VARS]", vars.ToString());
            content = content.Replace("[LOAD_TABLE_FUNCS]", loadfuncText);

            var filename = pathname + "/TableManager.cs";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            File.WriteAllText(filename, content, new UTF8Encoding(false));
        }

        /// <summary>
        /// 将表数据写入文件
        /// </summary>
        static void WriteToBinaryFile(string tbName, TableType tbType, string code, int rowNum, Dictionary<string, string> valueType, ExcelWorksheet sheet)
        {
            TablePath bytesPath = null;
            switch (tbType)
            {
                case TableType.CSharp:
                    bytesPath = new TablePath(csharpDataPath + "/" + tbName, clientDllPath);
                    break;
                case TableType.Server:
                    bytesPath = new TablePath(serverDataPath + "/" + tbName, serverDllPath);
                    break;
            }
            var binraryPath = bytesPath.path + ".bytes";
            if (File.Exists(binraryPath))
            {
                File.Delete(binraryPath);
            }
            var outputPath = Path.GetDirectoryName(binraryPath);
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            var instance = CreateDll(code, tbName, rowNum, valueType, sheet, bytesPath.dllpath);
            SerializeUtil.Serialize(binraryPath, instance);
        }

        /// <summary>
        /// 创建DLL程序集
        /// </summary>
        static object CreateDll(string code, string tbName, int rowNum, Dictionary<string, string> valueType, ExcelWorksheet sheet, string assemblyPath)
        {
            var nameTbName = "FirCommon.Data." + tbName;
            var assembly = CompileCodeAssembly(code, assemblyPath);
            var instance = assembly.CreateInstance(nameTbName);

            var tableType = assembly.GetType(nameTbName);
            var property = tableType.GetField("name");
            property.SetValue(instance, nameTbName);

            var methodInfo = tableType.GetMethod("AddItem", BindingFlags.Instance | BindingFlags.Public);
            var tableItemType = assembly.GetType(nameTbName + "Item");

            for (int i = 5; i <= rowNum; i++)
            {
                int j = 1;
                var tbItemInst = Activator.CreateInstance(tableItemType);
                foreach (var de in valueType)
                {
                    string prop = de.Key;
                    object objValue = null;
                    char splitChar = (char)0;
                    var varType = valueType[prop];
                    var vObject = sheet.GetValue(i, j);
                    if (vObject != null)
                    {
                        var value = vObject.ToString();
                        string extraParam = sheet.GetValue(3, j) as string;
                        switch (varType)
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
                                objValue = bool.Parse(value);
                                break;
                            case "float":
                                objValue = float.Parse(value);
                                break;
                            case "long":
                                objValue = long.Parse(value);
                                break;
                            case "enum":
                                objValue = GetEnumValue(assembly, extraParam, value);
                                break;
                            case "Vector2":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue = value.ToVec2(splitChar);
                                break;
                            case "Vector3":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue = value.ToVec3(splitChar);
                                break;
                            case "Color":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue = value.ToColor(splitChar);
                                break;
                            case "Color32":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue = value.ToColor32(splitChar);
                                break;
                            case "string[]":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue = value.Split(splitChar);
                                break;
                            case "int[]":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue = value.ToIntArray(splitChar);
                                break;
                            case "uint[]":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue = value.ToUIntArray(splitChar);
                                break;
                            case "float[]":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue = value.ToFloatArray(splitChar);
                                break;
                            case "long[]":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue = value.ToLongArray(splitChar);
                                break;
                        }
                    }
                    j++;
                    tbItemInst.GetType().GetField(prop).SetValue(tbItemInst, objValue);
                }
                methodInfo.Invoke(instance, new object[] { tbItemInst });
            }
            return instance;
        }

        static ClassInfo GetEnumType(string extraParam)
        {
            var clsInfo = new ClassInfo();
            var lastPoint = extraParam.LastIndexOf('.');
            clsInfo.namespaceName = extraParam.Substring(0, lastPoint);
            clsInfo.typeName = extraParam.Substring(lastPoint + 1, extraParam.Length - lastPoint - 1);
            return clsInfo;
        }

        static object GetEnumValue(Assembly asm, string extraParam, string enumValue)
        {
            var clsInfo = GetEnumType(extraParam);
            var etype = asm.GetType(clsInfo.namespaceName + "." + clsInfo.typeName);
            Array enumByReflection = Enum.GetValues(etype);
            foreach (var e in enumByReflection)
            {
                if (e.ToString() == enumValue)
                {
                    return e;
                }
            }
            return null;
        }

        /// <summary>
        /// 编译代码
        /// </summary>
        static Assembly CompileCodeAssembly(string classCode, string assemblyPath)
        {
            if (File.Exists(assemblyPath))
            {
                File.Delete(assemblyPath);
            }
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.dll");
            parameters.ReferencedAssemblies.Add("netstandard.dll");
            parameters.ReferencedAssemblies.Add("UnityEngine.dll");
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.OutputAssembly = assemblyPath;

            string depCode = null;
            var fullPath = Environment.CurrentDirectory + "/FirCommon/Define/CommonEnum.cs";
            if (File.Exists(fullPath))
            {
                depCode = File.ReadAllText(fullPath);
            }
            CompilerResults result = provider.CompileAssemblyFromSource(parameters, depCode, classCode);
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
