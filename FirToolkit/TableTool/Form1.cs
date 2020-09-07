using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TableTool
{
    public partial class fmMain : Form
    {
        string tableMd5File = string.Empty;
        public string currDir = Environment.CurrentDirectory.Replace("\\", "/") + "/";
        public static Dictionary<string, string> md5Values = new Dictionary<string, string>();

        public fmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                        LoadTableMd5();
                    }
                    if (strs[0] == "ClientData")
                    {
                        textBox2.Text = strs[1].Trim();
                    }
                    if (strs[0] == "ClientCodePath")
                    {
                        textBox3.Text = strs[1].Trim();
                    }
                    if (strs[0] == "ServerData")
                    {
                        textBox4.Text = strs[1].Trim();
                    }
                    if (strs[0] == "ServerCodePath")
                    {
                        textBox5.Text = strs[1].Trim();
                    }
                    if (strs[0] == "TemplatePath")
                    {
                        textBox6.Text = strs[1].Trim();
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
                if (!path.StartsWith(currDir))
                {
                    MessageBox.Show("不能选择工程目录以外路径！！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                textBox.Text = path.Replace(currDir, string.Empty);
                return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SelectPath(textBox1))
            {
                LoadTableMd5();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectPath(textBox2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SelectPath(textBox3);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            string configPath = currDir + "tablecfg.txt";
            if (!File.Exists(configPath))
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
            File.WriteAllLines(configPath, lines.ToArray());
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
            button4.Enabled = false;
            //try
            {
                TableProc.Start(this, excelPath, clientDir, clientCode, serverDir, serverCode, templateDir);
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            button4.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SelectPath(textBox4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SelectPath(textBox5);
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
            SaveTableMd5();
        }

        public void LoadTableMd5()
        {
            md5Values.Clear();
            var excelPath = currDir + textBox1.Text.Trim();
            var configPath = excelPath + "/_config/";
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
            tableMd5File = configPath + "md5.txt";

            if (File.Exists(tableMd5File))
            {
                var lines = File.ReadAllLines(tableMd5File);
                foreach (var l in lines)
                {
                    if (string.IsNullOrEmpty(l))
                    {
                        continue;
                    }
                    var strs = l.Split('=');
                    md5Values.Add(strs[0], strs[1]);
                }
            }
        }

        public void SaveTableMd5()
        {
            if (md5Values.Count == 0)
            {
                return;
            }
            if (File.Exists(tableMd5File))
            {
                File.Delete(tableMd5File);
            }
            foreach (var de in md5Values)
            {
                var str = de.Key + "=" + de.Value + "\n";
                File.AppendAllText(tableMd5File, str);
            }
        }
    }
}
