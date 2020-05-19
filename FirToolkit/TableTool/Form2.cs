using System;
using System.Windows.Forms;

namespace TableTool
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            listView1.Groups.Clear();
            listView1.Items.Clear();
            listView1.Columns.Clear();

            var columnHeader0 = new ColumnHeader();
            columnHeader0.Text = "表名称";
            columnHeader0.Width = 250;

            var columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "MD5";
            columnHeader1.Width = 320;

            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader0, columnHeader1 });

            var table = fmMain.md5Values;
            foreach(var de in table)
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
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                var item = listView1.SelectedItems[i];
                listView1.Items.Remove(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fmMain.md5Values.Clear();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                var keyItem = listView1.Items[i].SubItems[0];
                var valueItem = listView1.Items[i].SubItems[1];
                fmMain.md5Values.Add(keyItem.Text, valueItem.Text);
            }
            Close();
        }
    }
}
