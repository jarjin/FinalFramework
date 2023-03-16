using FirCommon.Utility;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TableTool
{
    /// <summary>
    /// Util's TableProc
    /// </summary>
    public partial class TableProc
    {
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
        static void WriteTableManagerFile(string tempText, string pathname, string extName)
        {
            vars.AppendLine("///[APPEND_VAR]");
            load_funcs.AppendLine("///[APPEND_TABLE]");

            var loadfuncText = load_funcs.ToString().TrimEnd('\n', '\t', '\r');
            var content = tempText.Replace("[DECLARE_TABLES_VARS]", vars.ToString());
            content = content.Replace("[LOAD_TABLE_FUNCS]", loadfuncText);

            var filename = pathname + "/TableManager." + extName;
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
                case TableType.Java:
                    bytesPath = new TablePath(serverDataPath + "/" + tbName, clientDllPath);
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
            ProtoUtil.Serialize(binraryPath, instance);
        }
    }
}
