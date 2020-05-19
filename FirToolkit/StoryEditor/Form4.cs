using System;
using System.Windows.Forms;

namespace StoryEditor
{
    public partial class Form4 : Form
    {
        public static string DataNodeName = string.Empty;
        public static string SubNodeName = string.Empty;

        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SubNodeName = textBox1.Text.Trim();
            Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            label3.Text = DataNodeName;
        }
    }
}
