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
using SearchOption = System.IO.SearchOption;


namespace LFiles
{

    public partial class Form1 : Form
    {

        //список представлений элементов файловой системы для левой панели
        List<DirFile> LeftPanel = new List<DirFile>();
        //для правой панели
        List<DirFile> RightPanel = new List<DirFile>();

        //текущий путь до левой панели
        private string leftpath;
        //правой панели
        private string rightpath;


        //список логических дисков
        private string[] Drives;


        public Form1()
        {
            InitializeComponent();
            //инициализация контекстного меню
            listBox1.ContextMenuStrip = contextMenuStrip1;
            listBox2.ContextMenuStrip = contextMenuStrip1;

            //получаем список дисков
            Drives = Directory.GetLogicalDrives();

            //добавляем их в комбобоксы
            foreach (var drive in Drives)
            {
                LeftDrives.Items.Add(drive);
                RightDrives.Items.Add(drive);

            }

            //слева выбираем первый диск
            LeftDrives.SelectedIndex = 0;
            //справа второй, если он существует
            RightDrives.SelectedIndex = Drives.Length > 1 ? 1 : 0;
            //фокус на левую панель
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

        //МЕТОД НЕ ИСПОЛЬЗУЕТСЯ
        void GetRootDir(string path)
        {
            if (path == null) return;
            var s = Directory.GetDirectoryRoot(path);
            if (s != path)
            {
                listBox1.Items.Insert(0, "[..]");

            }
        }

        //обновление представлений по пути
        internal void UpdatePath(string path, bool left)
        {
            //тру значит что будет обновляется левая панель, иначе правая
            if (left)
            {
                //очищаем список представлений и листбокс на экране
                listBox1.Items.Clear();
                LeftPanel.Clear();
                try
                {
                    //пытаемся получить список папок
                    Directory.GetDirectories(path);
                }
                catch (Exception ex)
                {
                    //если ошибка, переходим в корневой каталог первого диска, повторяем
                    MessageBox.Show(ex.Message, path);
                    UpdatePath(Drives[0], true);
                    LeftDrives.SelectedIndex = 0;
                    return;
                }
                //получаем спсок директорий
                var dirs = Directory.GetDirectories(path);
                if (path != Directory.GetDirectoryRoot(path))
                {
                    var di = new DirectoryInfo(path);
                    //если есть родительский каталог (который не совпадает с текущим путем) значить есть уровень выше, добавим переход наверх
                    if (di.Parent != null) LeftPanel.Add(new DirFile(di.Parent.FullName, true));
                }
                foreach (var dir in dirs)
                {
                    //добавляем все директории
                    LeftPanel.Add(new DirFile(dir, false));
                }
                var files = Directory.GetFiles(path);
                //потом все файлы
                foreach (var file in files)
                {
                    LeftPanel.Add(new DirFile(file));
                }

                //заключаем названия директорий в квадратные скобки
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
                //обновим путь
                leftpath = path;
                listBox1.SelectedIndex = 0;
            }
            else//аналогично для правой панели
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
        //если поменяли выбранный диск, обновляем представление в корневую папку
        private void LeftDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePath(LeftDrives.SelectedItem.ToString(), true);

        }

        //двойной клик...
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //...если что то выбрано в листбоксе...
            var select = listBox1.SelectedIndex;
            if (select < 0)
                return;
            //...если это не папка и не переход наверх...
            if (LeftPanel[listBox1.SelectedIndex].ext != "DIR/" && LeftPanel[listBox1.SelectedIndex].name != "[..]")
            {
                //...вызывает запуск этого файла стандартными средствами...
                Process.Start(LeftPanel[listBox1.SelectedIndex].path);
                return;
            }
            //...иначе же обновляет путь
            UpdatePath(LeftPanel[select].path, true);

        }


        private void RightDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePath(RightDrives.SelectedItem.ToString(), false);

        }

        //если в фокусе первый листбокс, и если нажата клавиша
        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                //таб, то берем в фокус второй листбокс
                listBox2.Select();
            }
            if (e.KeyCode == Keys.Enter)
            {
                //если энтер, делаем то же, что и двойное нажатие мыши
                listBox1_MouseDoubleClick(sender, null);
            }
        }

        //удаление файла
        void FDelete()
        {
            //если что то выбрано в первом листбоксе
            if (listBox1.SelectedIndex > -1)
            {
                //получаем список выделенных элеметов файловой системы
                var a = listBox1.SelectedIndices;
                for (int i = 0; i < a.Count; i++)
                {
                    //и для каждого выполняем удаление файла, если это не папка и не переход наверх
                    var tmp = LeftPanel[a[i]];
                    if (LeftPanel[listBox1.SelectedIndex].ext != "DIR/" && LeftPanel[listBox1.SelectedIndex].name != "[..]")
                    {
                        FileSystem.DeleteFile(tmp.path, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently, UICancelOption.DoNothing);
                    }
                    //или удаление папки, если это не переход наверх
                    else if (LeftPanel[listBox1.SelectedIndex].name != "[..]")
                    {
                        FileSystem.DeleteDirectory(tmp.path, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently, UICancelOption.DoNothing);

                    }
                }
                //обновим путь
                UpdatePath(leftpath, true);
            }
            else if (listBox2.SelectedIndex > -1)//аналогично для второго листбокса
            {
                var a = listBox2.SelectedIndices;
                for (int i = 0; i < a.Count; i++)
                {
                    var tmp = RightPanel[a[i]];
                    if (RightPanel[listBox2.SelectedIndex].ext != "DIR/" && RightPanel[listBox2.SelectedIndex].name != "[..]")
                    {
                        FileSystem.DeleteFile(tmp.path, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently, UICancelOption.DoNothing);


                    }
                    else if (RightPanel[listBox2.SelectedIndex].name != "[..]")
                    {
                        FileSystem.DeleteDirectory(tmp.path, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently, UICancelOption.DoNothing);

                    }
                }
                UpdatePath(rightpath, false);
            }

        }

        //переименование - где выделен файл или папка, то и будем переименовывать в новой форме
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

        //копирование
        void FCopy()
        {
            //если выбрано слева
            if (listBox1.SelectedIndex > -1)
            {
                //получаем список элементов файловой системы
                var a = listBox1.SelectedIndices;
                for (int i = 0; i < a.Count; i++)
                {

                    var tmp = LeftPanel[a[i]];
                    //если не папка и не переход вверх
                    if (LeftPanel[listBox1.SelectedIndex].ext != "DIR/" && LeftPanel[listBox1.SelectedIndex].name != "[..]")
                    {
                        //копируем файл
                        FileSystem.CopyFile(tmp.path, rightpath + @"\" + tmp.name, UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                    else if (LeftPanel[listBox1.SelectedIndex].name != "[..]")
                    {
                        //иначе если не переход наверх, копируем директорию
                        FileSystem.CopyDirectory(tmp.path, rightpath + @"\" + tmp.name, UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                }
                UpdatePath(rightpath, false);
            }
            else if (listBox2.SelectedIndex > -1)//аналогично для второго листбокса, то есть правой панели
            {
                var a = listBox2.SelectedIndices;
                for (int i = 0; i < a.Count; i++)
                {
                    var tmp = RightPanel[a[i]];
                    if (RightPanel[listBox2.SelectedIndex].ext != "DIR/" && RightPanel[listBox2.SelectedIndex].name != "[..]")
                    {
                        FileSystem.CopyFile(tmp.path, leftpath + @"\" + tmp.name, UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                    else if (RightPanel[listBox2.SelectedIndex].name != "[..]")
                    {
                        FileSystem.CopyDirectory(tmp.path, leftpath + @"\" + tmp.name, UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                }
                UpdatePath(leftpath, true);
            }
        }

        //аналогично обработке нажатия в первом листбоксе
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

        //аналогично двойному клику для первого листбокса
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

        //обработка клавиши копирования
        private void button1_Click(object sender, EventArgs e)
        {
            FCopy();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //сброс выделения с листбокса, неактивного в данный момент
        private void listBox1_Enter(object sender, EventArgs e)
        {
            listBox2.ClearSelected();
        }

        private void listBox2_Enter(object sender, EventArgs e)
        {
            listBox1.ClearSelected();

        }
        //обработка клавиши удаления
        private void button3_Click(object sender, EventArgs e)
        {
            FDelete();
            UpdatePath();
        }
        //обработка контекстного меню
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FCopy();
            UpdatePath();

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FDelete();
            UpdatePath();

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FRename();
        }
        //обработка клавиши переименования
        private void button2_Click(object sender, EventArgs e)
        {
            FRename();
            UpdatePath();

        }

        //перегруженный метод обновления пути, который обновляет путь и слева, и справа
        public void UpdatePath()
        {
            bool lastsel = listBox1.SelectedIndex != -1;
            UpdatePath(leftpath, true);
            UpdatePath(rightpath, false);
            if (lastsel)
            {
                listBox1.Focus();
                listBox2.ClearSelected();
            }
            else
            {
                listBox2.Focus();
                listBox1.ClearSelected();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //получем путь к выделенной панели, открываем поиск там
            string path = null;
            if (listBox1.SelectedIndex > -1)
            {
                path = leftpath;
            }
            else if (listBox2.SelectedIndex > -1)
            {
                path = rightpath;
            }
            var f = new SearchForm(path);
            f.Show();
        }

       
        private void button7_Click(object sender, EventArgs e)
        {
            //вызов дерева
            var tw = new Derevo();
            tw.Show();
        }

  
    }
}
