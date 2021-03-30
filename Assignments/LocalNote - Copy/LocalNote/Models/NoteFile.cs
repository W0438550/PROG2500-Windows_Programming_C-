using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalNote.Models
{
    public class NoteFile
    {
        public string FileName { get; set; }
        public string FileText { get; set; }

        public NoteFile(string fileName, string fileText)
        {
            this.FileName = fileName;
            this.FileText = fileText;
        }
    }
}
