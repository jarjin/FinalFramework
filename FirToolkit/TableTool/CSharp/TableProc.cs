using OfficeOpenXml;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace TableTool
{
    /// <summary>
    /// CSharp's TableProc
    /// </summary>
    public partial class TableProc
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 表格处理器
        /// </summary>
        static void HandleCSharpWorkSheet(string tableName, string sheetName, string excelFile, ExcelWorksheet sheet, string md5, TableType tableType, string destPath, bool generateCode)
        {
            if (generateCode)
            {
                var varName = tableName.FirstCharToLower();
                vars.AppendLine("    	public " + tableName + " " + varName + ";");
                load_funcs.AppendLine("        	" + varName + " = LoadData<" + tableName + ">(\"Tables/" + tableName + ".bytes\");");
                load_funcs.AppendLine("        	" + varName + ".Initialize();");
            }
            string destDir = destPath + "/Tables";
            var tableCode = CreateCSharpTableWithItem(tableName, destDir, sheet, generateCode);     //创建TABLE
            var compileInfo = new TableCompileInfo();
            compileInfo.tableName = tableName;
            compileInfo.tablePath = excelFile;
            compileInfo.tableType = tableType;
            compileInfo.sheetName = sheetName;
            compileInfo.tableCode = tableCode;
            compileInfos.Add(compileInfo);
        }

        /// <summary>
        /// 创建表结构跟ITEM文件
        /// </summary>
        static string CreateCSharpTableWithItem(string name, string destDir, ExcelWorksheet sheet, bool generateCode)
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
                                          .Replace("[TYPE]", keyType)
                                          .Replace("[TIME]", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss dddd"));
            if (generateCode)
            {
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
                var csPath = destDir + "/" + name + ".cs";
                File.WriteAllText(csPath, txtCode, new UTF8Encoding(false));
            }
            return txtCode;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 生成TableManager类
        /// </summary>
        static void CreateCSharpTableManager()
        {
            var tempfile = templateDir + "/C#TableManager.txt";
            if (File.Exists(tempfile))
            {
                var managerCode = File.ReadAllText(tempfile);
                WriteTableManagerFile(managerCode, csharpCodePath, "cs");
            }
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
            var etype = asm.GetType(clsInfo.typeName);
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
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.dll");
            parameters.ReferencedAssemblies.Add("netstandard.dll");
            parameters.ReferencedAssemblies.Add("FirCommon.dll");

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.OutputAssembly = assemblyPath;

            string depCode = null;
            var fullPath = Environment.CurrentDirectory + "/FirClient/Assets/Scripts/Data/Enums";
            if (Directory.Exists(fullPath))
            {
                var files = Directory.GetFiles(fullPath, "*.cs");
                foreach (var file in files)
                {
                    depCode += File.ReadAllText(file) + "\r\n";
                }
            }
            CompilerResults result = provider.CompileAssemblyFromSource(parameters, depCode, classCode);
            if (result.Errors.HasErrors)
            {
                string ErrorMessage = "";
                foreach (CompilerError err in result.Errors)
                {
                    ErrorMessage += err.ErrorText;
                }
                MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return result.CompiledAssembly;
        }
    }
}
