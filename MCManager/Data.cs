using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCManager
{
    public static class Data
    {
        public static string appdatafolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcm\\";

        public static string backupdir = appdatafolder + "backups\\";

        public static string pluginfolder = appdatafolder + "plugins\\";

        public static string logininfo = appdatafolder + "login.dat";

        public static string minecraftexe = appdatafolder + "Minecraft.exe";

        public static string minecraftdir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\";

        public static string minecraftbin = minecraftdir + "bin\\";

        internal static void CheckStartupFolders()
        {
            if (!Directory.Exists(appdatafolder))
            {
                Directory.CreateDirectory(appdatafolder);
            }
            if (!Directory.Exists(backupdir))
            {
                Directory.CreateDirectory(backupdir);
            }
            if (!Directory.Exists(pluginfolder))
            {
                Directory.CreateDirectory(pluginfolder);
            }
            if (!File.Exists(minecraftexe))
            {
                WebClient wc = new WebClient();
                wc.DownloadFileAsync(new Uri("http://northcode.no/Files/minecraftlauncher/Minecraft.exe"), minecraftexe);
            }
            if (!Directory.Exists(minecraftdir))
            {
                MessageBox.Show("Please start minecraft once to generate the folders", "Minecraft folders missing!");
            }
        }
    }
}