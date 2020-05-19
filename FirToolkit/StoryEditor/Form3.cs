using System;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace StoryEditor
{
    public partial class Form3 : Form
    {
        public static string role;
        public static string pos;
        public static string text;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            textBox1.Text = text;
            if (!string.IsNullOrEmpty(text))
            {
                button1.Text = "修改对话";
            }
            var i = 0;
            var index = 0;
            foreach (int v in Enum.GetValues(typeof(PosType)))
            {
                string strName = Enum.GetName(typeof(PosType), v);
                comboBox2.Items.Add(strName);
                if (strName == pos)
                {
                    index = i;
                }
                i++;
            }
            comboBox2.SelectedIndex = index;

            foreach (var de in Form1.roles)
            {
                comboBox1.Items.Add(de.Key);
            }
            comboBox1.Text = role == "系统" ? string.Empty : role;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            role = GetRole();
            text = textBox1.Text.Trim();
            pos = comboBox2.Items[comboBox2.SelectedIndex].ToString();
            Close();
        }

        string GetRole()
        {
            var rolestr = comboBox1.Text.Trim();
            if (Form1.roles.ContainsKey(rolestr))
            {
                return rolestr;
            }
            return "系统";
        }
    }
}
