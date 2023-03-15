using OfficeOpenXml;
using System;
using System.IO;
using System.Text;

namespace TableTool
{
    /// <summary>
    /// Java's TableProc
    /// </summary>
    public partial class TableProc
    {
        /// <summary>
        /// 表格处理器
        /// </summary>
        static void HandleJavaWorkSheet(string tableName, string sheetName, string excelFile, ExcelWorksheet sheet, string md5, TableType tableType, string destPath)
        {
            var varName = tableName.FirstCharToLower();
            vars.AppendLine("    	public " + tableName + " " + varName + ";");
            load_funcs.AppendLine("        	" + varName + " = LoadData(\"Tables/" + tableName + ".bytes\", " + tableName + ".class);");
            load_funcs.AppendLine("        	" + varName + ".Initialize();");

            string destDir = destPath + "/Tables";
            var tableCode = CreateJavaTableWithItem(tableName, excelFile, destDir, sheet);     //创建TABLE
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
        static string CreateJavaTableWithItem(string name, string excelPath, string destDir, ExcelWorksheet sheet)
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
                varBody.AppendLine("    	public " + GetJavaType(varType) + " " + varName + ";");
            }
            var varText = varBody.ToString().TrimEnd('\n', '\t', '\r');

            var tableCode = File.ReadAllText(templateDir + "/JavaTable.txt");
            string txtTableCode = tableCode.Replace("[NAME]", name)
                                           .Replace("[BODY]", varText)
                                           .Replace("[TYPE]", keyType)
                                           .Replace("[TIME]", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss dddd"));

            var itemCode = File.ReadAllText(templateDir + "/JavaTableItem.txt");

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            var csPath = destDir + "/" + name + ".java";
            File.WriteAllText(csPath, txtTableCode, new UTF8Encoding(false));
            return txtTableCode;
        }

        static string GetJavaType(string type)
        {
            switch (type)
            {
                case "string": return "String";
                case "bool": return "boolean";
            }
            return type;
        }

        static void CreateJavaTableManager()
        {
            var tempfile = templateDir + "/JavaTableManager.txt";
            if (File.Exists(tempfile))
            {
                var managerCode = File.ReadAllText(tempfile);
                WriteTableManagerFile(managerCode, serverCodePath, "java");
            }
        }
    }
}
