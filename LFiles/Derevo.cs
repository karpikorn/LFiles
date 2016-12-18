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
    public partial class Derevo : Form
    {
        public Derevo()
        {
            InitializeComponent();
            
        }

        //срабатывает, до того как развернется нод, но после того, как пользовател нажмет на плюс
        private void dirsTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

            if (e.Node.Nodes.Count <= 0) return;

            if (e.Node.Nodes[0].Text != @"..." || e.Node.Nodes[0].Tag != null) return;


            e.Node.Nodes.Clear();

            //список поддиректорий
            var dirs = Directory.GetDirectories(e.Node.Tag.ToString());

            //добавим каждую директорию в виде нода
            foreach (var dir in dirs)
            {
                var di = new DirectoryInfo(dir);
                var node = new TreeNode(di.Name, 0, 0);

                try
                {
                    node.Tag = dir;  //оставим полный путь для последующего использования в поле тэг

                    //if the directory has any sub directories add the place holder
                    //если в директории есть любая поддиректория, оставим метку
                    if (di.GetDirectories().Any())
                        node.Nodes.Add(null, "...", 0, 0);
                }
                catch (UnauthorizedAccessException)
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"Ошибко", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    //добавим нод
                    e.Node.Nodes.Add(node);
                }
            }
        }

        private void Derevo_Load(object sender, EventArgs e)
        {
            //список всех дисков
            var drives = Environment.GetLogicalDrives();
            //для каждого...
            foreach (var drive in drives)
            {
                var di = new DriveInfo(drive);

                var node = new TreeNode(drive.Substring(0, 1), 0, 0) {Tag = drive};

                if (di.IsReady)
                    node.Nodes.Add("...");
                //добавим нод с диском
                dirsTreeView.Nodes.Add(node);
            }
        }
    }
}
