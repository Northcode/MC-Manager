using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCManager
{
    public static class Data
    {
        public static string backupdir = jevofolder + "backups\\";

        public static string jevofolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.jevo\\";

        public static string minecraftbin = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\bin\\";
    }
}