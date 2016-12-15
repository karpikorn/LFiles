using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using Microsoft.VisualBasic.FileIO;

//this is new commit
namespace LFiles
{
    
    public partial class Form1 : Form
    {
        

        List<DirFile> LeftPanel = new List<DirFile>();
        List<DirFile> RightPanel = new List<DirFile>();

        private string leftpath;
        private string rightpath;

        private DirFile selected;

        private string[] Drives;
        

        public Form1()
        {
            InitializeComponent();
            listBox1.ContextMenuStrip = contextMenuStrip1;
            listBox2.ContextMenuStrip = contextMenuStrip1;

            Graphics g = CreateGraphics();
            Drives = Directory.GetLogicalDrives();
            
            foreach (var drive in Drives)
            {
                LeftDrives.Items.Add(drive);
                RightDrives.Items.Add(drive);

            }
            LeftDrives.SelectedIndex = 0;
            RightDrives.SelectedIndex = Drives.Length > 1 ? 1 : 0;
            listBox1.Select();
            listBox2.ClearSelected();

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void GetRootDir(string path)
        {
            if (path == null) return;
            var s = Directory.GetDirectoryRoot(path);
            if (s != path)
            {
                listBox1.Items.Insert(0,"[..]");
                
            }
        }

        internal void UpdatePath(string path, bool left)
        {
            if (left)
            {
               
                listBox1.Items.Clear();
                LeftPanel.Clear();
                try
                {
                    Directory.GetDirectories(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, path);
                    UpdatePath(Drives[0], true);
                    LeftDrives.SelectedIndex = 0;
                    return;
                }
                var dirs = Directory.GetDirectories(path);
                if (path != Directory.GetDirectoryRoot(path))
                {
                    var di = new DirectoryInfo(path);

                    if (di.Parent != null) LeftPanel.Add(new DirFile(di.Parent.FullName, true));
                }
                foreach (var dir in dirs)
                {
                    LeftPanel.Add(new DirFile(dir, false));
                }
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    LeftPanel.Add(new DirFile(file));
                }


                foreach (var dirFile in LeftPanel)
                {
                    var name = dirFile.name;

                    if (dirFile.ext == "DIR/")
                    {
                        name = "[" + name + "]";

                    }


                    var final = name;
                    listBox1.Items.Add(final);
                }
                leftpath = path;
                listBox1.SelectedIndex = 0;
            }
            else
            {
                listBox2.Items.Clear();
                RightPanel.Clear();
                try
                {
                    Directory.GetDirectories(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, path);
                    UpdatePath(Drives[0], true);
                    RightDrives.SelectedIndex = 0;
                    return;
                }
                var dirs = Directory.GetDirectories(path);
                if (path != Directory.GetDirectoryRoot(path))
                {
                    var di = new DirectoryInfo(path);

                    if (di.Parent != null) RightPanel.Add(new DirFile(di.Parent.FullName, true));
                }
                foreach (var dir in dirs)
                {
                    RightPanel.Add(new DirFile(dir, false));
                }
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    RightPanel.Add(new DirFile(file));
                }


                foreach (var dirFile in RightPanel)
                {
                    var name = dirFile.name;

                    if (dirFile.ext == "DIR/")
                    {
                        name = "[" + name + "]";

                    }


                    var final = name;
                    listBox2.Items.Add(final);
                }
                rightpath = path;
                listBox2.SelectedIndex = 0;
                
            }
        }

        private void LeftDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePath(LeftDrives.SelectedItem.ToString(),true);

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var select = listBox1.SelectedIndex;
            if (select < 0)
                return;

            if (LeftPanel[listBox1.SelectedIndex].ext != "DIR/" && LeftPanel[listBox1.SelectedIndex].name != "[..]")
            {
                Process.Start(LeftPanel[listBox1.SelectedIndex].path);
                return;
            }
            UpdatePath(LeftPanel[select].path,true);

        }

        private void RightDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePath(RightDrives.SelectedItem.ToString(), false);

        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                listBox2.Select();
            }
            if (e.KeyCode == Keys.Enter)
            {
                listBox1_MouseDoubleClick(sender, null);
            }
        }

        void FDelete()
        {
            if (listBox1.SelectedIndex > -1)
            {

                var a = listBox1.SelectedIndices;
                for (int i = 0; i < a.Count; i++)
                {
                    var tmp = LeftPanel[a[i]];
                    if (LeftPanel[listBox1.SelectedIndex].ext != "DIR/" && LeftPanel[listBox1.SelectedIndex].name != "[..]")
                    {
                        FileSystem.DeleteFile(tmp.path, UIOption.AllDialogs, RecycleOption.DeletePermanently, UICancelOption.DoNothing);
                    }
                    else if (LeftPanel[listBox1.SelectedIndex].name != "[..]")
                    {
                        FileSystem.DeleteDirectory(tmp.path, UIOption.AllDialogs, RecycleOption.DeletePermanently, UICancelOption.DoNothing);

                    }
                }
                UpdatePath(leftpath, true);
            }
            else if (listBox2.SelectedIndex > -1)
            {
                var a = listBox2.SelectedIndices;
                for (int i = 0; i < a.Count; i++)
                {
                    var tmp = RightPanel[a[i]];
                    if (RightPanel[listBox2.SelectedIndex].ext != "DIR/" && RightPanel[listBox2.SelectedIndex].name != "[..]")
                    {
                        FileSystem.CopyFile(tmp.path, leftpath + tmp.name, UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                    else if (RightPanel[listBox2.SelectedIndex].name != "[..]")
                    {
                        FileSystem.CopyDirectory(tmp.path, leftpath + tmp.name, UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                }
                UpdatePath(rightpath, false);
            }
            
        }

        void FRename()
        {
            if (listBox1.SelectedIndex > -1)

            {
                var a = new RenameDialog(LeftPanel[listBox1.SelectedIndex], this);
                a.Show();
            }
            else if (listBox2.SelectedIndex > -1)
            {
                var a = new RenameDialog(RightPanel[listBox2.SelectedIndex], this);
                a.Show();
            }
        }

        void FCopy()
        {
            if (listBox1.SelectedIndex > -1)
            {
                
                var a = listBox1.SelectedIndices;
                for (int i = 0; i < a.Count; i++)
                {
                    var tmp = LeftPanel[a[i]];
                    if (LeftPanel[listBox1.SelectedIndex].ext != "DIR/" && LeftPanel[listBox1.SelectedIndex].name != "[..]")
                    {
                        FileSystem.CopyFile(tmp.path, rightpath + tmp.name, UIOption.AllDialogs);
                    }
                    else if (LeftPanel[listBox1.SelectedIndex].name != "[..]")
                    {
                        FileSystem.CopyDirectory(tmp.path, rightpath + tmp.name, UIOption.AllDialogs);
                    }
                }
                UpdatePath(rightpath,false);
            }
            else if (listBox2.SelectedIndex > -1)
            {
                var a = listBox2.SelectedIndices;
                for (int i = 0; i < a.Count; i++)
                {
                    var tmp = RightPanel[a[i]];
                    if (RightPanel[listBox2.SelectedIndex].ext != "DIR/" && RightPanel[listBox2.SelectedIndex].name != "[..]")
                    {
                        FileSystem.CopyFile(tmp.path, leftpath + tmp.name, UIOption.AllDialogs);
                    }
                    else if (RightPanel[listBox2.SelectedIndex].name != "[..]")
                    {
                        FileSystem.CopyDirectory(tmp.path, leftpath + tmp.name, UIOption.AllDialogs);
                    }
                }
                UpdatePath(leftpath, true);
            }
        }

        private void listBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                listBox1.Select();
            }
            if (e.KeyCode == Keys.Enter)
            {
                listBox2_DoubleClick(sender, null);
            }
        }

        private void listBox1_Leave(object sender, EventArgs e)
        {
          
        }

        private void listBox2_Leave(object sender, EventArgs e)
        {
        
            
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            var select = listBox2.SelectedIndex;
            if (select < 0)
                return;
            if (RightPanel[listBox2.SelectedIndex].ext != "DIR/" && RightPanel[listBox2.SelectedIndex].name != "[..]")
            {
                Process.Start(RightPanel[listBox2.SelectedIndex].path);
                return;
            }
            UpdatePath(RightPanel[select].path, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FCopy();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // selected = listBox1.SelectedIndex >= 0 ? LeftPanel[listBox1.SelectedIndex] : null;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  selected = listBox2.SelectedIndex >= 0 ? RightPanel[listBox2.SelectedIndex] : null;
        }

        private void listBox1_Enter(object sender, EventArgs e)
        {
            listBox2.ClearSelected();
        }

        private void listBox2_Enter(object sender, EventArgs e)
        {
            listBox1.ClearSelected();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FDelete();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FCopy();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FDelete();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FRename();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FRename();
        }

        public void UpdatePath()
        {
            UpdatePath(leftpath, true);
            UpdatePath(rightpath, false);
        }
    }
}
