using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCManager
{
    public static class Data
    {
        internal static string backupdir = "";

        internal static string jevofolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.jevo";
    }
}