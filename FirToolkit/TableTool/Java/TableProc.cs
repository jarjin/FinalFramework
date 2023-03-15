using FirCommon.Utility;
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
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            CreateJavaTableWithItem(tableName, excelFile, destDir, sheet);     //创建TABLE
        }

        /// <summary>
        /// 创建表结构跟ITEM文件
        /// </summary>
        static void CreateJavaTableWithItem(string name, string excelPath, string destDir, ExcelWorksheet sheet)
        {
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

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
                    keyType = GetJavaType(varType);
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
            var txtTableCode = tableCode.Replace("[NAME]", name)
                                        .Replace("[TYPE]", keyType.FirstCharToUpper())
                                        .Replace("[TIME]", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss dddd"));

            File.WriteAllText(destDir + "/" + name + ".java", txtTableCode, new UTF8Encoding(false));

            var itemCode = File.ReadAllText(templateDir + "/JavaTableItem.txt");
            var txtItemCode = itemCode.Replace("[NAME]", name).Replace("[BODY]", varText);
            File.WriteAllText(destDir + "/" + name + "Item.java", txtItemCode, new UTF8Encoding(false));
        }

        static string GetJavaType(string type)
        {
            switch (type)
            {
                case "string": return "String";
                case "bool": return "Boolean";
                case "int": return "Integer";
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
