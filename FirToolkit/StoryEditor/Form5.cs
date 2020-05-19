using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoryEditor
{
    public partial class Form5 : Form
    {
        public static string newNodeName = string.Empty;

        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newNodeName = textBox1.Text.Trim();
            Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            textBox1.Text = newNodeName;
        }
    }
}
