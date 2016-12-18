using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LFiles
{
    //класс, хранит полный путь до объекта файловой системы, имя и разширение 
    public class DirFile
    {
        public string path;
        public string name;
        public string ext;

        //конструктор, создающий представление файла или папки
        public DirFile(string path)
        {
            this.path = path;
            var f = new FileInfo(path);
            name = f.Name;
            ext = f.Extension;
        }
        //конструктор создающий представление перехода на уровень выше
        public DirFile(string path, bool root)
        {
            this.path = path;
            var f = new FileInfo(path);
            name = f.Name;
            ext = "DIR/";
            if (!root) return;
            name = "[..]";
            ext = "";
        }
    }
}
