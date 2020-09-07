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
    public partial class Form2 : Form
    {
        public static string DataNodeName = string.Empty;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataNodeName = textBox1.Text.Trim();
            Close();
        }
    }
}
