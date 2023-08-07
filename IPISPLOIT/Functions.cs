using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPISPLOIT
{
    internal class Functions
    {
        public static void PopulateListBox(ListBox lsb, string Folder, string FileType)
        {
            DirectoryInfo dinfo = new DirectoryInfo(Folder);
            FileInfo[] Files = dinfo.GetFiles(FileType);
            foreach (FileInfo file in Files)
            {
                lsb.Items.Add(file.Name);
            }
        }

        public static OpenFileDialog openfiledialog = new OpenFileDialog
        {
            Filter = "Текстовые файлы (*.txt)|*.txt|Lua файлы (*.lua*)|*.lua*",
            FilterIndex = 1,
            RestoreDirectory = true,
            Title = "Load"
        };
    }
}
