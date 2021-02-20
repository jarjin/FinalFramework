using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace TableTool
{
    public partial class TableProc
    {
        static fmMain fmMain;
        static StringBuilder vars;
        static StringBuilder load_funcs;
        static List<TableCompileInfo> compileInfos = new List<TableCompileInfo>();

        static string clientDllPath, serverDllPath;
        static string csharpDataPath, csharpCodePath, luaCodePath, serverDataPath, serverCodePath, templateDir;

        /// <summary>
        /// 导入所有的EXCEL表
        /// </summary>
        public static void Start(fmMain main, string csharpData, string csharpCode, string luaCode,
                        string serverData, string serverCode, string tempDir, string clientDll, string serverDll)
        {
            fmMain = main;
            csharpDataPath = csharpData;
            csharpCodePath = csharpCode;
            luaCodePath = luaCode;
            serverDataPath = serverData;
            serverCodePath = serverCode;
            templateDir = tempDir;
            clientDllPath = fmMain.currDir + clientDll;
            serverDllPath = fmMain.currDir + serverDll;

            StartProc(TableType.Lua);
            StartProc(TableType.CSharp);
            StartProc(TableType.Server);
            MessageBox.Show("处理完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static void StartProc(TableType type)
        {
            compileInfos.Clear();

            vars = new StringBuilder();
            load_funcs = new StringBuilder();

            var tables = fmMain.GetTables();
            if (tables.Count > 0)
            {
                var formatTables = new List<TableData>();
                foreach (var de in tables)
                {
                    if (type == TableType.Lua && de.Value.format == TableFormat.Lua)
                    {
                        formatTables.Add(de.Value);
                    }
                    if (type == TableType.CSharp && de.Value.format == TableFormat.CSharp)
                    {
                        formatTables.Add(de.Value);
                    }
                    if (type == TableType.Server && de.Value.withServer)
                    {
                        formatTables.Add(de.Value);
                    }
                }
                for(int i = 0; i < formatTables.Count; i++)
                {
                    ParseProcTable(type, formatTables[i], i, formatTables.Count);
                }
                CreateTableManager(type);
            }
        }

        static bool IsNewOrUpdateTable(string tableName, string newmd5)
        {
            return true;
            if (!fmMain.ContainsMd5(tableName))
            {
                fmMain.UpdateMd5(tableName, newmd5);
                return true;
            }
            var oldmd5 = fmMain.GetMd5(tableName);      //老的md5值

            if (newmd5 != oldmd5)
            {
                fmMain.UpdateMd5(tableName, newmd5);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 分析处理表
        /// </summary>
        static void ParseProcTable(TableType type, TableData table, float curr, float max)
        {
            string excelFile = table.fileName;
            if (!File.Exists(excelFile))
            {
                throw new Exception("excel file not exist!!!，check xlsx file settings!!!");
            }
            var md5 = md5file(excelFile);

            var name = Path.GetFileNameWithoutExtension(excelFile);
            string tableName = name + "Table";

            using (var fs = new FileStream(excelFile, FileMode.Open))
            {
                using (ExcelPackage package = new ExcelPackage(fs))
                {
                    var sheets = new List<ExcelWorksheet>();
                    var count = package.Workbook.Worksheets.Count;
                    for (int i = 1; i <= count; ++i)
                    {
                        var sheet = package.Workbook.Worksheets[i];
                        if (sheet.Cells.Count() == 0)
                        {
                            continue;
                        }
                        sheets.Add(sheet);
                    }
                    for(int i = 0; i < sheets.Count; i++)
                    {
                        var sheet = sheets[i];
                        if (sheets.Count > 1)
                        {
                            var sheetName = sheet.Name.FirstCharToUpper();
                            if (i == 0)
                            {
                                if (sheetName != "Sheet1")
                                {
                                    tableName = name + "_" + sheetName + "Table";
                                }
                            }
                            else
                            {
                                tableName = name + "_" + sheetName + "Table";
                            }
                        }
                        ProcWorkSheet(tableName, type, table, sheet, md5);
                    }
                }
            }
        }

        static void ProcWorkSheet(string tableName, TableType type, TableData table, ExcelWorksheet sheet, string md5)
        {
            var sheetName = sheet.Name.ToLower();
            Console.WriteLine("{0}, {1}", tableName + " " + sheetName, sheet.Cells.Count());

            var destPath = string.Empty;
            switch(type)
            {
                case TableType.Lua:
                    destPath = luaCodePath;
                    break;
                case TableType.CSharp:
                    destPath = csharpCodePath;
                    break;
                case TableType.Server:
                    destPath = serverCodePath;
                    break;
            }
            if (type == TableType.Lua)
            {
                HandleLuaWorkSheet(tableName, sheetName, table.fileName, sheet, md5, destPath);
            }
            else
            {
                HandleCSharpWorkSheet(tableName, sheetName, table.fileName, sheet, md5, type, destPath);
            }
        }

        static void CreateTableManager(TableType type)
        {
            if (type == TableType.Lua)
            {
                CreateLuaTableManager();
            }
            else
            {
                CreateCSharpTableManager(type);
                ExecuteExportTables();
            }
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
    }
}
