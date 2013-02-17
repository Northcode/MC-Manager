using System;
using System.IO;
using System.Net;
using System.Text;
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
        
        public static string versionpath = appdatafolder + "version.txt";
        
        public const string versionurl = "https://raw.github.com/Northcode/MC-Manager/master/MCManager/ver.txt";

        public const string updateurl = "https://github.com/Northcode/MC-Manager/raw/master/MCManager/bin/Debug/MCManager.exe";

        public static string updateConfig = appdatafolder + "update.conf";

        public static string updaterExe = appdatafolder + "update.exe";

        public static StringBuilder updateData = new StringBuilder();

        public const string updateExeUrl = "https://github.com/Northcode/MC-Manager/raw/master/MCMUpdate/bin/Debug/MCMUpdate.exe";

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
            if (!File.Exists(versionpath))
            {
                File.WriteAllText(versionpath, "0.0");
            }
            if (!File.Exists(updaterExe))
            {
                WebClient wc = new WebClient();
                wc.DownloadFileAsync(new Uri(updateExeUrl), updaterExe);
            }
        }

        internal static void CheckForUpdate()
        {
            WebClient wc = new WebClient();
            string remoteVersion = wc.DownloadString(versionurl);
            string localVersion = File.ReadAllText(versionpath);
            if (remoteVersion != localVersion)
            {
                DialogResult r = MessageBox.Show("New Update Available! Download?", "Update", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    updateData.AppendLine("UPDATE;MCM;" + Application.StartupPath + ";" + updateurl);
                }
            }
        }

        internal static void PreformUpdate()
        {
            if (updateData.ToString() != "")
            {
                File.WriteAllText(updateConfig, updateData.ToString());
            }
        }
    }
}