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
    //форма переименования
    public partial class RenameDialog : Form
    {
        //цель переименования - объект файловой системы
        private readonly DirFile _file;
        //указатель на форму
        private readonly Form1 _form1;

        public RenameDialog(DirFile file, Form1 form1)
        {
            _file = file;
            _form1 = form1;
            InitializeComponent();
            //получаем текущий путь
            textBox1.Text = _file.path;
            button1.Select();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //если не папка и не переход выше
            if (_file.ext != "DIR/" && _file.name != "[..]")
            {
                try

                {
                    //переименование файла
                    FileSystem.RenameFile(_file.path, textBox1.Text.Substring(textBox1.Text.LastIndexOf('\\') + 1));

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
                //иначе, если не переход выше
            else if (_file.name != "[..]")
            {
                try
                {
                    //переименование директории
                    FileSystem.RenameDirectory(_file.path, textBox1.Text);

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
            }
            //обновим пути
            _form1.UpdatePath();

            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();

        }
    }
}
