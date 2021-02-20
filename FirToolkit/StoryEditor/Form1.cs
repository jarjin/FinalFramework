using ComponentFactory.Krypton.Toolkit;
using StoryEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public enum PosType
    {
        Left = 0,
        Right = 1,
    }

    public partial class Form1 : KryptonForm
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool MessageBeep(uint uType);

        private static StoryInfo data = null;
        private static string xmlcfg = string.Empty;
        private static string npccfg = string.Empty;
        private static bool relativeXml = true;
        private static bool relativeNpc = true;
        private static bool expandAll = true;
        public static Dictionary<string, string> roles = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConfig();
            LoadXmlData();     //载入目录树

            kryptonDataGridView.DataMember = string.Empty;
            kryptonHeaderGroupNavigation.ValuesPrimary.Heading = "剧情列表";
        }

        private void LoadXmlData()
        {
            ResetXmlData();
            if (string.IsNullOrEmpty(xmlcfg))
            {
                xmlcfg = Environment.CurrentDirectory + "/storys.xml";
            }
            if (File.Exists(xmlcfg))
            {
                this.Text = "Story Editor : " + xmlcfg;

                var xmlData = XmlHelper.LoadXml(xmlcfg);
                (data = new StoryInfo()).Init(xmlData);
                InitTreeView();
            }
            if (expandAll)
            {
                treeView.ExpandAll();
            }
        }

        private void ResetXmlData()
        {
            data = null;
            treeView.Nodes.Clear();
            kryptonDataGridView.DataSource = null;
        }

        private void InitTreeView()
        {
            if (data == null) return;
            foreach (var de in data.datas)
            {
                var node = treeView.Nodes.Add(de.Value.name);
                node.Tag = de.Key;  //TAG保存DATAID
                foreach (var page in de.Value.pages)
                {
                    var subNode = node.Nodes.Add(page.Value.title);
                    subNode.Tag = de.Key + "_" + page.Key;     //用TAG存储索引ID
                }
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null) return;
            if (e.Node != null)
            {
                kryptonHeaderGroupDetails.ValuesPrimary.Heading = e.Node.Text;
                FilterDataTable(e.Node);
            }
            else
            {
                kryptonHeaderGroupDetails.ValuesPrimary.Heading = "Details";
                kryptonHeaderGroupDetails.ValuesPrimary.Image = null;
            }
        }

        private void FilterDataTable(TreeNode node)
        {
            if (node.Tag == null) return;
            var index = node.Tag.ToString().Split('_');
            var dataid = int.Parse(index[0]);
            var pageid = int.Parse(index[1]);

            var dataData = data.datas[dataid];
            if (dataData == null) return;

            var pageData = dataData.pages[pageid];
            if (pageData == null) return;

            kryptonDataGridView.DataMember = string.Empty;
            kryptonDataGridView.DataSource = pageData.dialogTable.DefaultView;
        }

        private void kryptonDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            kryptonRichTextBox1.Text = string.Empty;
            if (kryptonDataGridView.SelectedRows.Count == 1)
            {
                var row = kryptonDataGridView.SelectedRows[0];

                string details = string.Format("[ {0} ]\n{1}\n{2}\n{3}",
                                    GetLink(),
                                    row.Cells[1].Value.ToString(),
                                    row.Cells[2].Value.ToString(),
                                    row.Cells[3].Value.ToString());
                var count = details.IndexOf('\n');
                kryptonRichTextBox1.Text = details;
                kryptonRichTextBox1.Select(0, count);
                kryptonRichTextBox1.SelectionColor = Color.Red;
                kryptonRichTextBox1.SelectionBackColor = Color.Black;
                kryptonRichTextBox1.SelectionFont = new Font("微软雅黑", 12, FontStyle.Bold); ;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addStoryData_Click(object sender, EventArgs e)
        {
            (new Form2()).ShowDialog();
            var dataNodeName = Form2.DataNodeName;
            if (string.IsNullOrEmpty(dataNodeName))
            {
                return;
            }
            var storyData = data.AddData(dataNodeName);
            var node = treeView.Nodes.Add(storyData.name);
            node.Tag = storyData.id;
        }

        private void addSubNode_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Parent != null)
            {
                return;
            }
            Form4.DataNodeName = treeView.SelectedNode.Text;
            (new Form4()).ShowDialog();
            var dataNodeName = Form4.SubNodeName;
            if (string.IsNullOrEmpty(dataNodeName))
            {
                return;
            }
            var dataid = (int)treeView.SelectedNode.Tag;
            var storyPage = data.AddPage(dataid, dataNodeName);

            var newNode = treeView.SelectedNode.Nodes.Add(dataNodeName);
            newNode.Tag = dataid + "_" + storyPage.id;
            treeView.ExpandAll();
        }

        private void saveDialog_Click(object sender, EventArgs e)
        {
            SaveDialog();
        }

        private void SaveDialog()
        {
            var str = kryptonRichTextBox1.Text.Trim();
            if (string.IsNullOrEmpty(str)) return;
            if (treeView.SelectedNode == null || treeView.SelectedNode.Parent == null)
            {
                return;
            }
            if (treeView.SelectedNode.Tag == null) return;
            var tags = treeView.SelectedNode.Tag.ToString().Split('_');
            var dataid = int.Parse(tags[0]);
            var pageid = int.Parse(tags[1]);

            if (kryptonDataGridView.SelectedRows.Count == 1)
            {
                var row = kryptonDataGridView.SelectedRows[0];
                var id = int.Parse(row.Cells[0].Value.ToString());

                var strs = str.Split('\n');
                if (strs.Length != 3) return;

                var role = strs[0];
                var pos = strs[1];
                var text = strs[2];
                data.UpdateDialog(dataid, pageid, id, role, pos, text);
            }
        }

        private void saveData_Click(object sender, EventArgs e)
        {
            if (StoryInfo.isModify)
            {
                data.SaveData(xmlcfg);
                BackupXml();
                MessageBox.Show("保存完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void addDialog_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Parent == null)
            {
                return;
            }
            if (treeView.SelectedNode.Tag == null) return;
            var strs = treeView.SelectedNode.Tag.ToString().Split('_');
            var dataid = int.Parse(strs[0]);
            var pageid = int.Parse(strs[1]);

            Form3.role = string.Empty;
            Form3.pos = string.Empty;
            Form3.text = string.Empty;

            (new Form3()).ShowDialog();
            if (string.IsNullOrEmpty(Form3.role)) return;

            var roleid = Form3.role;
            var pos = Form3.pos == PosType.Left.ToString() ? PosType.Left : PosType.Right;
            var text = Form3.text;
            data.AddDialog(dataid, pageid, roleid, pos.ToString(), text);
        }

        private void removeDialog_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Parent == null)
            {
                return;
            }
            if (MessageBox.Show("删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            if (treeView.SelectedNode.Tag == null) return;
            var strs = treeView.SelectedNode.Tag.ToString().Split('_');
            var dataid = int.Parse(strs[0]);
            var pageid = int.Parse(strs[1]);

            if (kryptonDataGridView.SelectedRows.Count == 1)
            {
                var row = kryptonDataGridView.SelectedRows[0];
                var id = row.Cells[0].Value.ToString();
                data.RemoveDialog(dataid, pageid, id);
            }
        }

        private void removeStoryData_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Parent != null)
            {
                return;
            }
            var dataid = (int)treeView.SelectedNode.Tag;
            data.RemoveData(dataid);
            treeView.Nodes.Remove(treeView.SelectedNode);
        }

        private void editDataNode_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null) return;

            Form5.newNodeName = treeView.SelectedNode.Text;
            (new Form5()).ShowDialog();

            if (treeView.SelectedNode.Parent == null)
            {
                var dataid = (int)treeView.SelectedNode.Tag;
                var newName = Form5.newNodeName;
                if (string.IsNullOrEmpty(newName)) return;

                data.EditData(dataid, newName);
                treeView.SelectedNode.Text = newName;
            }
            else
            {
                var strs = treeView.SelectedNode.Tag.ToString().Split('_');
                var dataid = int.Parse(strs[0]);
                var pageid = int.Parse(strs[1]);
                var newName = Form5.newNodeName;
                if (string.IsNullOrEmpty(newName)) return;

                data.EditPage(dataid, pageid, newName);
                treeView.SelectedNode.Text = newName;
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                SaveDialog();
                data.SaveData(xmlcfg);
            }
            else if (keyData == Keys.F2)
            {
                editDataNode_Click(null, null);
            }
            return base.ProcessDialogKey(keyData);
        }

        private void settingConfig_Click(object sender, EventArgs e)
        {
            Form6.settingCfgPath = xmlcfg;
            Form6.npcCfgPath = npccfg;
            Form6.relativeXml = relativeXml;
            Form6.relativeNpc = relativeNpc;
            Form6.expandAll = expandAll;

            (new Form6()).ShowDialog();

            xmlcfg = Form6.settingCfgPath;
            npccfg = Form6.npcCfgPath;
            relativeXml = Form6.relativeXml;
            relativeNpc = Form6.relativeNpc;
            expandAll = Form6.expandAll;

            var keyValues = new List<string>();
            keyValues.Add("xmlcfg=" + Form6.settingCfgPath);
            keyValues.Add("npccfg=" + Form6.npcCfgPath);
            keyValues.Add("relativexml=" + (Form6.relativeXml ? 1 : 0));
            keyValues.Add("relativenpc=" + (Form6.relativeNpc ? 1 : 0));
            keyValues.Add("expandall=" + (Form6.expandAll ? 1 : 0));
            foreach (var de in roles)
            {
                keyValues.Add("role_" + de.Key + "=" + de.Value);
            }
            string configPath = Environment.CurrentDirectory + "/storycfg.txt";
            File.WriteAllLines(configPath, keyValues.ToArray());

            LoadXmlData();
        }

        private void LoadConfig()
        {
            string configPath = Environment.CurrentDirectory + "/storycfg.txt";
            if (File.Exists(configPath))
            {
                var lines = File.ReadAllLines(configPath);
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    var strs = line.Split('=');
                    if (strs[0] == "xmlcfg")
                    {
                        xmlcfg = strs[1].Trim();
                    }
                    else if (strs[0] == "npccfg")
                    {
                        npccfg = strs[1].Trim();
                    }
                    else if (strs[0] == "relativexml")
                    {
                        relativeXml = strs[1].Trim() == "1" ? true : false;
                    }
                    else if (strs[0] == "relativenpc")
                    {
                        relativeNpc = strs[1].Trim() == "1" ? true : false;
                    }
                    else if (strs[0] == "expandall")
                    {
                        expandAll = strs[1].Trim() == "1" ? true : false;
                    }
                }
                LoadNpcCfg();
            }
        }

        public void LoadNpcCfg()
        {
            roles.Clear();
            var asset = XmlHelper.LoadXml(npccfg);
            if (asset != null)
            {
                for (int i = 0; i < asset.Children.Count; i++)
                {
                    var item = asset.Children[i] as SecurityElement;
                    var id = item.Attribute("id");
                    var nick = item.Attribute("nick");
                    roles.Add(nick, id);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (StoryInfo.isModify)
            {
                var result = MessageBox.Show("数据有变化，是不是需要保存数据？", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    data.SaveData(xmlcfg);
                    BackupXml();
                }
            }
        }

        private void BackupXml()
        {
            var bkPath = Environment.CurrentDirectory + "/Backups/";
            if (!Directory.Exists(bkPath))
            {
                Directory.CreateDirectory(bkPath);
            }
            bkPath += "story_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
            File.Copy(xmlcfg, bkPath, true);
        }

        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label)) return;
            if (treeView.SelectedNode == null) return;
            if (treeView.SelectedNode.Parent == null)
            {
                var dataid = (int)treeView.SelectedNode.Tag;

                data.EditData(dataid, e.Label);
                treeView.SelectedNode.Text = e.Label;
            }
            else
            {
                var strs = treeView.SelectedNode.Tag.ToString().Split('_');
                var dataid = int.Parse(strs[0]);
                var pageid = int.Parse(strs[1]);

                data.EditPage(dataid, pageid, e.Label);
                treeView.SelectedNode.Text = e.Label;
            }
        }

        private void kryptonDataGridView_DoubleClick(object sender, EventArgs e)
        {
            var row = kryptonDataGridView.SelectedRows[0];
            Form3.role = row.Cells[1].Value.ToString();
            Form3.pos = row.Cells[2].Value.ToString();
            Form3.text = row.Cells[3].Value.ToString();
            (new Form3()).ShowDialog();

            var tags = treeView.SelectedNode.Tag.ToString().Split('_');
            var dataid = int.Parse(tags[0]);
            var pageid = int.Parse(tags[1]);
            var id = int.Parse(row.Cells[0].Value.ToString());
            data.UpdateDialog(dataid, pageid, id, Form3.role, Form3.pos, Form3.text);
        }

        private void CopyLink_Click(object sender, EventArgs e)
        {
            if (kryptonDataGridView.SelectedRows.Count == 0)
            {
                return;
            }
            var link = GetLink();
            Clipboard.SetDataObject(link);
            MessageBeep(0);
            //MessageBox.Show("链接已复制: " + link, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GetLink()
        {
            var row = kryptonDataGridView.SelectedRows[0];
            var tags = treeView.SelectedNode.Tag.ToString().Split('_');
            var dataid = int.Parse(tags[0]);
            var pageid = int.Parse(tags[1]);
            var id = int.Parse(row.Cells[0].Value.ToString());
            return string.Format("{0},{1},{2},1", dataid, pageid, id);
        }

        private void insertDialog_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Parent == null)
            {
                return;
            }
            if (kryptonDataGridView.SelectedRows.Count == 0)
            {
                return;
            }
            var index = kryptonDataGridView.SelectedRows[0].Index;

            if (treeView.SelectedNode.Tag == null) return;
            var strs = treeView.SelectedNode.Tag.ToString().Split('_');
            var dataid = int.Parse(strs[0]);
            var pageid = int.Parse(strs[1]);

            Form3.role = string.Empty;
            Form3.pos = string.Empty;
            Form3.text = string.Empty;

            (new Form3()).ShowDialog();
            if (string.IsNullOrEmpty(Form3.role)) return;

            var roleid = Form3.role;
            var pos = Form3.pos == PosType.Left.ToString() ? PosType.Left : PosType.Right;
            var text = Form3.text;
            data.InsertDialog(dataid, pageid, index, roleid, pos.ToString(), text);
        }

        private void ExportText_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files(*.*)|*>**";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var txtpath = saveFileDialog1.FileName;
                if (!string.IsNullOrEmpty(txtpath))
                {
                    data.exportText(txtpath);
                    MessageBox.Show("导出完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ExportLua_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Lua files (*.lua)|*.lua|All files(*.*)|*>**";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var txtpath = saveFileDialog1.FileName;
                if (!string.IsNullOrEmpty(txtpath))
                {
                    data.exportLua(txtpath);
                    MessageBox.Show("导出完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void kryptonDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.C))
            {
                CopyLink_Click(null, null);
            }
        }
    }
}
