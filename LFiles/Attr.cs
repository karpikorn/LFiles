using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LFiles
{
    public partial class Attr : Form
    {
        private string path;
        private FileAttributes fa;
        private bool init = false;
        public Attr(string path)
        {
            this.path = path;
            //аттрибуты файла
            fa = File.GetAttributes(path);
            InitializeComponent();
            textBox1.Text = path;
            //если есть флаг директория, то один текст, иначе другой
            label1.Text = fa.HasFlag(FileAttributes.Directory) ? "Это директория." : "Это файл.";

            //загрузим параметры
            cbHI.Checked = fa.HasFlag(FileAttributes.Hidden);
            cbSY.Checked = fa.HasFlag(FileAttributes.System);
            cbRO.Checked = fa.HasFlag(FileAttributes.ReadOnly);
            init = true;
        }

        private void cbRO_CheckedChanged(object sender, EventArgs e)
        {
            //только после инициализации
            if(!init)
                return;
            //если галка стала нажата, ставим атрибуты
            if (cbRO.Checked)
            {
                File.SetAttributes(path, fa | FileAttributes.ReadOnly);
            }
                //иначе убираем
            else
            {
                fa = RemoveAttribute(fa, FileAttributes.ReadOnly);
                File.SetAttributes(path, fa);
            }
            fa = File.GetAttributes(path);

        }
        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            // X И неY дают X, в котором нет У
            return attributes & ~attributesToRemove;
        }

        private void cbHI_CheckedChanged(object sender, EventArgs e)
        {
            if (!init)
                return;
            if (cbHI.Checked)
            {
                File.SetAttributes(path, fa | FileAttributes.Hidden);
                fa = File.GetAttributes(path);
            }
            else
            {
                fa = RemoveAttribute(fa, FileAttributes.Hidden);
                File.SetAttributes(path, fa);
            }
            fa = File.GetAttributes(path);

        }

        private void cbSY_CheckedChanged(object sender, EventArgs e)
        {
            if (!init)
                return;
            if (cbSY.Checked)
            {
                File.SetAttributes(path, fa | FileAttributes.System);
            }
            else
            {
                fa = RemoveAttribute(fa, FileAttributes.System);
                File.SetAttributes(path, fa);
            }
            fa = File.GetAttributes(path);

        }
    }
}
