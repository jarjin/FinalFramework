using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security;
using System.Text;
using WindowsFormsApp1;

namespace StoryEditor
{
    public class DialogTable : DataTable
    {
        public DialogTable()
        {
            Columns.Add("id", typeof(int));
            Columns.Add("role", typeof(string));
            Columns.Add("pos", typeof(string));
            Columns.Add("text", typeof(string));
        }
    }

    public class StoryPage
    {
        public int id;
        public string title;
        public DialogTable dialogTable = new DialogTable();

        public void InitDialog(SecurityElement pageNode)
        {
            if (pageNode == null || pageNode.Children == null) return;
            for (int i = 0; i < pageNode.Children.Count; i++)
            {
                var dialogNode = pageNode.Children[i] as SecurityElement;
                var id = int.Parse(dialogNode.Attribute("id"));
                var role = GetRoleStrById(dialogNode.Attribute("role"));
                var pos = GetPosById(dialogNode.Attribute("pos"));
                var text = dialogNode.Attribute("text");
                dialogTable.Rows.Add(id, role, pos, text);
            }
        }

        string GetRoleStrById(string id)
        {
            var roleid = id.Trim();
            foreach(var de in Form1.roles)
            {
                if (de.Value == roleid)
                {
                    return de.Key;
                }
            }
            return "系统";
        }

        string GetPosById(string pos)
        {
            var posid = int.Parse(pos);
            return ((PosType)posid).ToString();
        }
    }


    public class StoryData
    {
        public int id;
        public string name;
        public Dictionary<int, StoryPage> pages = new Dictionary<int, StoryPage>();

        public void InitPage(SecurityElement dataNode)
        {
            if (dataNode == null || dataNode.Children == null) return;
            for (int i = 0; i < dataNode.Children.Count; i++)
            {
                var pageNode = dataNode.Children[i] as SecurityElement;
                var storyPage = new StoryPage();
                storyPage.id = int.Parse(pageNode.Attribute("id"));
                storyPage.title = pageNode.Attribute("title");
                storyPage.InitDialog(pageNode);
                pages.Add(storyPage.id, storyPage);
            }
        }
    }

    public class StoryInfo
    {
        public static bool isModify = false;
        static string xmlTemplate = @"<?xml version=""1.0"" encoding=""utf-8""?>
<datas>
[DATA]</datas>
";
        public Dictionary<int, StoryData> datas = new Dictionary<int, StoryData>();

        public void Init(SecurityElement root)
        {
            if (root == null) return;
            for (int i = 0; i < root.Children.Count; i++)
            {
                var dataNode = root.Children[i] as SecurityElement;

                var storyData = new StoryData();
                storyData.id = int.Parse(dataNode.Attribute("id"));
                storyData.name = dataNode.Attribute("name");
                storyData.InitPage(dataNode);
                datas.Add(storyData.id, storyData);
            }
        }

        int GetMaxDataId()
        {
            var result = 0;
            foreach(var item in datas)
            {
                if (item.Key > result)
                {
                    result = item.Key;
                }
            }
            return result + 1;
        }

        public StoryData AddData(string str)
        {
            StoryData data = new StoryData();
            data.id = GetMaxDataId();
            data.name = str;
            datas.Add(data.id, data);
            isModify = true;
            return data;
        }

        public void RemoveData(int dataid)
        {
            if (datas.ContainsKey(dataid))
            {
                isModify = true;
                datas.Remove(dataid);
            }
        }

        public void EditData(int dataid, string dataName)
        {
            if (!datas.ContainsKey(dataid))
            {
                return;
            }
            isModify = true;
            var data = datas[dataid];
            data.name = dataName;
        }

        public StoryPage AddPage(int dataid, string title)
        {
            if (!datas.ContainsKey(dataid))
            {
                return null;
            }
            var data = datas[dataid];
            var page = new StoryPage();
            page.title = title;
            page.id = GetMaxPageId(data);
            data.pages.Add(page.id, page);
            isModify = true;
            return page;
        }

        private int GetMaxPageId(StoryData data)
        {
            var result = 0;
            foreach(var de in data.pages)
            {
                if (de.Key > result)
                {
                    result = de.Key;
                }
            }
            return result + 1;
        }

        public void RemovePage(int dataid, int pageid)
        {
            if (datas.ContainsKey(dataid))
            {
                var data = datas[dataid];
                if (data.pages.ContainsKey(pageid))
                {
                    isModify = true;
                    data.pages.Remove(pageid);
                }
            }
        }

        public void EditPage(int dataid, int pageid, string title)
        {
            if (datas.ContainsKey(dataid))
            {
                var data = datas[dataid];
                if (data.pages.ContainsKey(pageid))
                {
                    isModify = true;
                    var page = data.pages[pageid];
                    page.title = title;
                }
            }
        }

        int GetMaxTableId(DialogTable table)
        {
            var result = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                var id = int.Parse(row["id"].ToString());
                if (id > result)
                {
                    result = id;
                }
            }
            return result + 1;
        }

        public void AddDialog(int dataid, int pageid, string role, string pos, string text)
        {
            if (!datas.ContainsKey(dataid))
            {
                return;
            }
            var data = datas[dataid];
            if (!data.pages.ContainsKey(pageid))
            {
                return;
            }
            isModify = true;
            var page = data.pages[pageid];
            var id = GetMaxTableId(page.dialogTable);
            page.dialogTable.Rows.Add(id, role, pos, text);
        }

        public void InsertDialog(int dataid, int pageid, int index, string role, string pos, string text)
        {
            if (!datas.ContainsKey(dataid))
            {
                return;
            }
            var data = datas[dataid];
            if (!data.pages.ContainsKey(pageid))
            {
                return;
            }
            isModify = true;
            var page = data.pages[pageid];
            var id = GetMaxTableId(page.dialogTable);

            var NewDataRow = page.dialogTable.NewRow();
            NewDataRow["id"] = id;
            NewDataRow["role"] = role;
            NewDataRow["pos"] = pos;
            NewDataRow["text"] = text;
            page.dialogTable.Rows.InsertAt(NewDataRow, index);
        }

        public void RemoveDialog(int dataid, int pageid, string dialogid)
        {
            if (datas.ContainsKey(dataid))
            {
                var data = datas[dataid];
                if (data.pages.ContainsKey(pageid))
                {
                    var page = data.pages[pageid];
                    var rows = page.dialogTable.Select("id = " + dialogid);
                    foreach(var row in rows )
                    {
                        row.Delete();
                    }
                    isModify = true;
                    page.dialogTable.AcceptChanges();
                }
            }
        }

        public void UpdateDialog(int dataid, int pageid, int dialogid, string role, string pos, string text)
        {
            if (datas.ContainsKey(dataid))
            {
                var data = datas[dataid];
                if (data.pages.ContainsKey(pageid))
                {
                    var page = data.pages[pageid];
                    var rows = page.dialogTable.Select("id = " + dialogid);
                    foreach (var row in rows)
                    {
                        row.BeginEdit();
                        row["role"] = role;
                        row["pos"] = pos;
                        row["text"] = text;
                        row.EndEdit();
                    }
                    isModify = true;
                    page.dialogTable.AcceptChanges();
                }
            }
        }

        public void SaveData(string xmlpath)
        {
            StringBuilder content = new StringBuilder();
            foreach(var data in datas)
            {
                content.AppendLine("    <data id=\"" + data.Key + "\" name=\"" + data.Value.name + "\" >");
                foreach(var page in data.Value.pages)
                {
                    content.AppendLine("        <page id=\"" + page.Key + "\" title=\"" + page.Value.title + "\">");
                    var dataTable = page.Value.dialogTable;
                    for(int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        var row = dataTable.Rows[i];
                        var id = row["id"].ToString();
                        var text = row["text"].ToString();

                        var pos = row["pos"].ToString();
                        var posid = (int)Enum.Parse(typeof(PosType), pos);

                        var role = row["role"].ToString();
                        var rolestr = role == "系统" ? "0" : Form1.roles[role];
                        content.AppendLine("            <dialog id=\"" + id + "\" role=\"" + rolestr + "\" pos=\"" + posid + "\" text=\"" + text + "\" />");
                    }
                    content.AppendLine("        </page>");
                }
                content.AppendLine("    </data>");
            }
            string alltxt = xmlTemplate.Replace("[DATA]", content.ToString().TrimEnd('\n', '\t'));
            File.WriteAllText(xmlpath, alltxt, new UTF8Encoding(false));
            isModify = false;
        }

        public void exportText(string txtpath)
        {
            StringBuilder content = new StringBuilder();
            foreach (var data in datas)
            {
                content.AppendLine(data.Value.name);  
                foreach (var page in data.Value.pages)
                {
                    content.AppendLine("    " + page.Value.title);
                    var dataTable = page.Value.dialogTable;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        var row = dataTable.Rows[i];
                        var text = row["text"].ToString();
                        content.AppendLine("    " + text);
                        content.AppendLine("");
                    }
                    content.AppendLine("");
                }
                content.AppendLine("");
            }
            File.WriteAllText(txtpath, content.ToString(), new UTF8Encoding(false));
        }

        public void Clear()
        {
            datas.Clear();
        }

        internal void exportLua(string luapath)
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("local storyline = {");
            foreach (var data in datas)
            {
                content.AppendLine("    ["+ data.Key + "] = {");
                content.AppendLine("        name = \"" + data.Value.name + "\",");
                content.AppendLine("        pages = {");
                foreach (var page in data.Value.pages)
                {
                    content.AppendLine("            [" + page.Key + "] = {");
                    content.AppendLine("                title = \"" + page.Value.title + "\",");
                    content.AppendLine("                dialogs = {");
                    var dataTable = page.Value.dialogTable;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        var row = dataTable.Rows[i];
                        var id = row["id"].ToString();
                        var text = row["text"].ToString();

                        var pos = row["pos"].ToString();
                        var posid = (int)Enum.Parse(typeof(PosType), pos);

                        var role = row["role"].ToString();
                        var rolestr = role == "系统" ? "0" : Form1.roles[role];
                        content.AppendLine("                    [" + id + "] = {");
                        content.AppendLine("                        roleid = " + rolestr + ", posid = " + posid + ",");
                        content.AppendLine("                        text = \"" + text + "\",");
                        content.AppendLine("                    },");
                    }
                    content.AppendLine("                },");
                    content.AppendLine("            },");
                }
                content.AppendLine("        }");
                content.AppendLine("    },");
            }
            content.AppendLine("}");
            content.AppendLine("return storyline");
            string alltxt = content.ToString().TrimEnd('\n', '\t');
            File.WriteAllText(luapath, alltxt, new UTF8Encoding(false));
        }
    }
}
