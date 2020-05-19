using System;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace StoryEditor
{
    public partial class Form6 : Form
    {
        public static string settingCfgPath = string.Empty;
        public static string npcCfgPath = string.Empty;

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            textBox1.Text = settingCfgPath;
            textBox2.Text = npcCfgPath;

            listView1.BeginUpdate();
            listView1.Groups.Clear();
            listView1.Items.Clear();
            listView1.Columns.Clear();

            var columnHeader0 = new ColumnHeader();
            columnHeader0.Text = "角色名称";
            columnHeader0.Width = 285;

            var columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "角色ID";
            columnHeader1.Width = 270;

            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader0, columnHeader1 });

            var table = Form1.roles;
            foreach (var de in table)
            {
                listView1.Items.Add(new ListViewItem(new[] { de.Key, de.Value }));
            }
            listView1.View = View.Details;
            listView1.ShowGroups = true;
            listView1.MultiSelect = false;

            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView1.FullRowSelect = true;
            listView1.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            settingCfgPath = textBox1.Text.Trim();
            npcCfgPath = textBox2.Text.Trim();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = "剧情文件(*.xml)|*.xml";
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName.Replace('\\', '/');
            }
        }

        private void AddRole_Click(object sender, EventArgs e)
        {
        }

        private void DeleteRole_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                var item = listView1.SelectedItems[i];
                var key = listView1.Items[item.Index].SubItems[0].Text;
                Form1.roles.Remove(key);
                listView1.Items.Remove(item);
                return;
            }
        }

        private void UpdateRole_Click(object sender, EventArgs e)
        {
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = "角色配置(*.xml)|*.xml";
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName.Replace('\\', '/');
            }
        }
    }
}
