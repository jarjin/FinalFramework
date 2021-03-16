using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TableTool
{
    public partial class TableProc
    {
        static void HandleLuaWorkSheet(string tableName, string sheetName, string excelFile, ExcelWorksheet sheet, string md5, string destPath)
        {
            var varName = tableName.FirstCharToLower();
            vars.AppendLine("    TableManager." + varName + " = require 'Data.Tables." + tableName + "'");
            vars.AppendLine("    TableManager." + varName + ":Initialize()");

            string destDir = destPath + "/Tables";
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            var tableCode = CreateLuaTableWithItem(tableName, excelFile, destDir, sheet);     //创建TABLE
        }

        /// <summary>
        /// 创建表结构跟ITEM文件
        /// </summary>
        static string CreateLuaTableWithItem(string name, string excelPath, string destDir, ExcelWorksheet sheet)
        {
            int colNum = sheet.Dimension.End.Column;
            int rowNum = sheet.Dimension.End.Row;

            string keyType = string.Empty;
            var refType = new StringBuilder();

            ///获取列数据类型
            var valueType = new Dictionary<string, string>();
            for (int i = 1; i <= colNum; i++)
            {
                string varName = sheet.GetValue(4, i).ToString();
                if (varName.Trim() == "note")
                {
                    continue;
                }
                string varType = sheet.GetValue(2, i).ToString().ToLower();
                valueType.Add(varName, varType);

                if (varType == "enum")
                {
                    string extraParam = sheet.GetValue(3, i) as string;
                    var strs = extraParam.Split('.');
                    var line = "local " + strs[strs.Length - 1] + " = " + extraParam;
                    refType.AppendLine(line);
                }
            }

            ///获取数据本体
            var dataBody = new StringBuilder();
            for (int i = 5; i <= rowNum; i++)
            {
                int j = 1;
                string idValue = string.Empty;
                string objValue = string.Empty;
                foreach (var de in valueType)
                {
                    string prop = de.Key;
                    var varType = valueType[prop];
                    var vObject = sheet.GetValue(i, j);
                    char splitChar = (char)0;
                    if (vObject != null)
                    {
                        var value = vObject.ToString();
                        string extraParam = sheet.GetValue(3, j) as string;
                        switch (varType)
                        {
                            case "int":
                            case "uint":
                            case "float":
                            case "long":
                            case "bool":
                                objValue += prop + " = " + value.ToLower() + ", ";
                                break;
                            case "string":
                                objValue += prop + " = '" + value + "', ";
                                break;
                            case "enum":
                                objValue += prop + " = " + GetEnumValue(extraParam, value) + ", ";
                                break;
                            case "Vector2":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue += prop + " = " + value.ToLuaVec2(splitChar) + ", ";
                                break;
                            case "Vector3":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue += prop + " = " + value.ToLuaVec3(splitChar) + ", ";
                                break;
                            case "Color":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue += prop + " = " + value.ToLuaColor(splitChar) + ", ";
                                break;
                            case "Color32":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue += prop + " = " + value.ToLuaColor32(splitChar) + ", ";
                                break;
                            case "string[]":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue += prop + " = " + value.ToLuaStringArray(splitChar) + ", ";
                                break;
                            case "int[]":
                            case "uint[]":
                            case "float[]":
                            case "long[]":
                                splitChar = char.Parse(extraParam.Trim());
                                objValue += prop + " = " + value.ToLuaArray(splitChar) + ", ";
                                break;
                        }
                        if (prop == "id")
                        {
                            idValue = varType == "string" ? "'" + value +"'" : value;
                        }
                    }
                    j++;
                }
                objValue = objValue.TrimEnd(',', ' ');
                string strRow = "        [" + idValue + "] = {" + objValue + "},";
                dataBody.AppendLine(strRow);
            }
            var result = dataBody.ToString().TrimEnd('\n', '\t', '\r', ',');
            var tableItemCode = File.ReadAllText(templateDir + "/LuaTable.txt");
            string content = tableItemCode.Replace("[NAME]", name)
                                          .Replace("[TABLE_BODY]", result)
                                          .Replace("[REFTYPE]", refType.ToString())
                                          .Replace("[TIME]", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss dddd"));

            var csPath = destDir + "/" + name + ".lua";
            File.WriteAllText(csPath, content, new UTF8Encoding(false));
            return content;
        }

        static string GetEnumValue(string extraParam, string enumValue)
        {
            var strs = extraParam.Split('.');
            return strs[strs.Length - 1] + "." + enumValue;
        }

        static void CreateLuaTableManager()
        {
            var tempfile = templateDir + "/LuaTableManager.txt";
            if (File.Exists(tempfile))
            {
                var managerCode = File.ReadAllText(tempfile);
                vars.AppendLine("---[APPEND_VAR]---");

                var content = managerCode.Replace("[DECLARE_TABLES_VARS]", vars.ToString().TrimEnd('\n', '\t', '\r'));
                var filename = luaCodePath + "/TableManager.lua";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                File.WriteAllText(filename, content, new UTF8Encoding(false));
            }
        }
    }
}
