using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TableTool
{
    public partial class Form2 : Form
    {
        Dictionary<string, TableData> temps = new Dictionary<string, TableData>();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            temps.Clear();
            dataGridView1.Rows.Clear();
            var tables = fmMain.GetTables();
            foreach (var de in tables)
            {
                AddOne(de.Key, de.Value.Clone());
            }
        }

        private void AddOne(string key, TableData value)
        {
            var row = new DataGridViewRow();
            var cell1 = new DataGridViewTextBoxCell();
            cell1.Value = key;
            row.Cells.Add(cell1);

            var cell2 = new DataGridViewTextBoxCell();
            cell2.Value = value.md5value;
            row.Cells.Add(cell2);

            var cell3 = new DataGridViewCheckBoxCell();
            cell3.Value = value.format == TableFormat.CSharp;
            row.Cells.Add(cell3);

            var cell4 = new DataGridViewCheckBoxCell();
            cell4.Value = value.format == TableFormat.Lua;
            row.Cells.Add(cell4);

            var cell5 = new DataGridViewCheckBoxCell();
            cell5.Value = value.withServer;
            row.Cells.Add(cell5);

            dataGridView1.Rows.Add(row);

            temps.Add(key, value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = dataGridView1.SelectedRows.Count - 1; i >= 0; i--)
            {
                var item = dataGridView1.SelectedRows[i];
                dataGridView1.Rows.Remove(item);

                var strKey = item.Cells[0].Value;
                temps.Remove(strKey.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            temps.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var tables = fmMain.GetTables();
            tables.Clear();
            foreach (var de in temps)
            {
                tables.Add(de.Key, de.Value.Clone());
            }
            temps.Clear();
            Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4))
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var tableName = row.Cells["Column1"].Value.ToString();
                if (e.ColumnIndex == 2)
                {
                    var v = (bool)row.Cells["Column3"].Value;
                    row.Cells["Column3"].Value = !v;
                    row.Cells["Column4"].Value = false;

                    temps[tableName].format = TableFormat.None;
                    if ((bool)row.Cells["Column3"].Value)
                    {
                        temps[tableName].format = TableFormat.CSharp;
                    }
                    else if ((bool)row.Cells["Column4"].Value)
                    {
                        temps[tableName].format = TableFormat.Lua;
                    }
                }
                else if (e.ColumnIndex == 3)
                {
                    var v = (bool)row.Cells["Column4"].Value;
                    row.Cells["Column3"].Value = false;
                    row.Cells["Column4"].Value = !v;

                    temps[tableName].format = TableFormat.None;
                    if ((bool)row.Cells["Column3"].Value)
                    {
                        temps[tableName].format = TableFormat.CSharp;
                    }
                    else if ((bool)row.Cells["Column4"].Value)
                    {
                        temps[tableName].format = TableFormat.Lua;
                    }
                }
                else if (e.ColumnIndex == 4)
                {
                    var v = (bool)row.Cells["Column5"].Value;
                    row.Cells["Column5"].Value = !v;
                    temps[tableName].withServer = !v;
                }
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.ColumnIndex == 2 || e.ColumnIndex == 3) && e.RowIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);

                // TODO: The radio button flickers on mouse over.
                // I tried setting DoubleBuffered on the parent panel, but the flickering persists.
                // If someone figures out how to resolve this, please leave a comment.

                Rectangle rectRadioButton = new Rectangle();
                // TODO: Would be nice to not use magic numbers here.
                rectRadioButton.Width = 14;
                rectRadioButton.Height = 14;
                rectRadioButton.X = e.CellBounds.X + (e.CellBounds.Width - rectRadioButton.Width) / 2;
                rectRadioButton.Y = e.CellBounds.Y + (e.CellBounds.Height - rectRadioButton.Height) / 2;

                ButtonState buttonState;
                if (e.Value == DBNull.Value || (bool)(e.Value) == false)
                {
                    buttonState = ButtonState.Normal;
                }
                else
                {
                    buttonState = ButtonState.Checked;
                }
                ControlPaint.DrawRadioButton(e.Graphics, rectRadioButton, buttonState);

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Focus);

                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var fs = openFileDialog1.FileNames;
                foreach(string f in fs)
                {
                    if (!string.IsNullOrEmpty(f))
                    {
                        AddFileToList(f);
                    }
                }
            }
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            var path = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (path != null && path.Length > 0)
            {
                for(int i = 0; i < path.Count(); i++)
                {
                    var f = path[i];
                    if (!string.IsNullOrEmpty(f))
                    {
                        AddFileToList(f);
                    }
                }
            }
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) 
                e.Effect = DragDropEffects.Link;
            else 
                e.Effect = DragDropEffects.None;
        }

        private void AddFileToList(string filePath)
        {
            if (!filePath.ToLower().EndsWith(".xlsx"))
            {
                return;
            }
            filePath = filePath.Replace('\\', '/');
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            foreach (var de in temps)
            {
                if (de.Key == fileName)
                {
                    return;
                }
            }
            AddOne(fileName, new TableData() 
            { 
                fileName = filePath,
                md5value = string.Empty,
                format = TableFormat.CSharp,
            });;
        }
    }
}
