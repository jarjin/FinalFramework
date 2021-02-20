using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TableTool
{
    public partial class fmMain : Form
    {
        public string currDir = Environment.CurrentDirectory.Replace("\\", "/") + "/";
        static Dictionary<string, TableData> tables = new Dictionary<string, TableData>();

        public fmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tables.Clear();
            label2.Text = currDir;
            string configPath = currDir + "tablecfg.txt";
            if (File.Exists(configPath))
            {
                var lines = File.ReadAllLines(configPath);
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    var strs = line.Split('=');
                    if (strs[0] == "ExcelPath")
                    {
                        textBox1.Text = strs[1].Trim();
                    } 
                    else if (strs[0] == "ClientData")
                    {
                        textBox2.Text = strs[1].Trim();
                    }
                    else if (strs[0] == "ClientCodePath")
                    {
                        textBox3.Text = strs[1].Trim();
                    }
                    else if(strs[0] == "ServerData")
                    {
                        textBox4.Text = strs[1].Trim();
                    }
                    else if(strs[0] == "ServerCodePath")
                    {
                        textBox5.Text = strs[1].Trim();
                    }
                    else if(strs[0] == "TemplatePath")
                    {
                        textBox6.Text = strs[1].Trim();
                    }
                    else if(strs[0] == "ClientDLL")
                    {
                        textBox8.Text = strs[1].Trim();
                    }
                    else if(strs[0] == "ServerDLL")
                    {
                        textBox7.Text = strs[1].Trim();
                    }
                    else if (strs[0] == "LuaPath")
                    {
                        textBox9.Text = strs[1].Trim();
                    }
                    else
                    {
                        tables.Add(strs[0], new TableData(strs[1]));
                    }
                }
            }
        }

        private bool SelectPath(TextBox textBox)
        {
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                folderBrowserDialog1.SelectedPath = (currDir + textBox.Text).Replace("/", "\\");
            }
            else
            {
                folderBrowserDialog1.SelectedPath = currDir.Replace("/", "\\");
            }
            folderBrowserDialog1.ShowNewFolderButton = true;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = folderBrowserDialog1.SelectedPath.Replace("\\", "/");
                if (!path.ToLower().StartsWith(currDir.ToLower()))
                {
                    MessageBox.Show("不能选择工程目录以外路径！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                textBox.Text = path.Remove(0, currDir.Length);
                return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectPath(textBox1);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveTableConfig();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var excelPath = currDir + textBox1.Text.Trim();
            if (string.IsNullOrEmpty(textBox1.Text) || !Directory.Exists(excelPath))
            {
                MessageBox.Show("Excel目录设置错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var clientDir = currDir + textBox2.Text.Trim();
            if (string.IsNullOrEmpty(textBox2.Text) || !Directory.Exists(clientDir))
            {
                MessageBox.Show("客户端数据目录设置错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var clientCode = currDir + textBox3.Text.Trim();
            if (string.IsNullOrEmpty(textBox3.Text) || !Directory.Exists(clientCode))
            {
                MessageBox.Show("客户端代码目录设置错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var serverDir = currDir + textBox4.Text.Trim();
            if (string.IsNullOrEmpty(textBox4.Text) || !Directory.Exists(serverDir))
            {
                MessageBox.Show("服务器数据目录设置错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var serverCode = currDir + textBox5.Text.Trim();
            if (string.IsNullOrEmpty(textBox5.Text) || !Directory.Exists(serverCode))
            {
                MessageBox.Show("服务器代码目录设置错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var templateDir = currDir + textBox6.Text.Trim();
            if (string.IsNullOrEmpty(textBox6.Text) || !Directory.Exists(templateDir))
            {
                MessageBox.Show("服务器数据目录设置错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var clientDll = textBox8.Text.Trim();
            if (string.IsNullOrEmpty(clientDll))
            {
                MessageBox.Show("客户端DLL名称设置错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var serverDll = textBox7.Text.Trim();
            if (string.IsNullOrEmpty(serverDll))
            {
                MessageBox.Show("服务端DLL名称设置错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var luaPath = textBox9.Text.Trim();
            if (string.IsNullOrEmpty(luaPath))
            {
                MessageBox.Show("Lua代码路径设置错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            button4.Enabled = false;
            TableProc.Start(this, clientDir, clientCode, luaPath, serverDir, serverCode, templateDir, clientDll, serverDll);
            button4.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SelectPath(textBox6);
        }

        public void SetProgress(int curr, int max)
        {
            toolStripProgressBar1.Maximum = max;
            toolStripProgressBar1.Value = curr;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            (new Form2()).ShowDialog();
        }

        public static Dictionary<string, TableData> GetTables()
        {
            return tables;
        }

        public bool ContainsMd5(string key)
        {
            string tableName = key.Replace("Table", string.Empty);
            return tables.ContainsKey(tableName);
        }

        public string GetMd5(string key)
        {
            string tableName = key.Replace("Table", string.Empty);
            return tables[tableName].md5value;
        }

        public void UpdateMd5(string key, string value)
        {
            string tableName = key.Replace("Table", string.Empty);
            tables[tableName].md5value = value;
        }

        public void SaveTableConfig()
        {
            string configPath = currDir + "tablecfg.txt";
            if (File.Exists(configPath))
            {
                File.Delete(configPath);
            }
            var lines = new List<string>();
            lines.Add("ExcelPath=" + textBox1.Text.Trim());
            lines.Add("ClientData=" + textBox2.Text.Trim());
            lines.Add("ClientCodePath=" + textBox3.Text.Trim());
            lines.Add("ServerData=" + textBox4.Text.Trim());
            lines.Add("ServerCodePath=" + textBox5.Text.Trim());
            lines.Add("TemplatePath=" + textBox6.Text.Trim());
            lines.Add("ClientDLL=" + textBox8.Text.Trim());
            lines.Add("ServerDLL=" + textBox7.Text.Trim());
            lines.Add("LuaPath=" + textBox9.Text.Trim());
            File.WriteAllLines(configPath, lines.ToArray());

            foreach (var de in tables)
            {
                var str = de.Key + "=" + de.Value.ToString() + "\n";
                File.AppendAllText(configPath, str);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SelectPath(textBox3);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            SelectPath(textBox2);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            SelectPath(textBox5);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            SelectPath(textBox4);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SelectPath(textBox9);
        }
    }
}
