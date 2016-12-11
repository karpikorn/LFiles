using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LFiles
{
    public class DirFile
    {
        public string path;
        public string name;
        public string ext;

        public DirFile(string path)
        {
            this.path = path;
            var f = new FileInfo(path);
            name = f.Name;
            ext = f.Extension;
        }
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
