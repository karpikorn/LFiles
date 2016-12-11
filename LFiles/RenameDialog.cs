using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;


namespace LFiles
{
    public partial class RenameDialog : Form
    {
        private readonly DirFile _file;
        private readonly Form1 _form1;

        public RenameDialog(DirFile file, Form1 form1)
        {
            _file = file;
            _form1 = form1;
            InitializeComponent();
            textBox1.Text = _file.path;
            button1.Select();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_file.ext != "DIR/" && _file.name != "[..]")
            {
                try
                {
                    FileSystem.RenameFile(_file.path, textBox1.Text.Substring(textBox1.Text.LastIndexOf('\\') + 1));

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            else if (_file.name != "[..]")
            {
                try
                {
                    FileSystem.RenameDirectory(_file.path, textBox1.Text);

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
            }
            _form1.UpdatePath();

            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();

        }
    }
}
