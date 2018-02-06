using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfConvertor.Files
{
    public class FileLauncher
    {
        public void Launch(string fileName)
        {
            System.Diagnostics.Process.Start(fileName);
        }
    }
}
