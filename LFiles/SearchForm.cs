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
    public partial class SearchForm : Form
    {
        private string path;
        public SearchForm(string pth)
        {
            InitializeComponent();
            path = pth;
            textBox1.Text = path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] allFiles = null;
            try
            {
                //Поиск файлов осуществляется стандартной функцией, даже в поддиректориях (:
            allFiles = Directory.GetFiles(path, textBox2.Text, SearchOption.AllDirectories);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            foreach (var c in allFiles)
            {
                listBox1.Items.Add(c);
            }
        }
    }
}
