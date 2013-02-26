using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCManager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Data.CheckStartupFolders();
            PluginLoader.LoadPlugins();
            DataHolder.LoadConfigs();
            if (!DataHolder.GetConfig().Has("autoupdate"))
            {
                DataHolder.GetConfig().Set("autoupdate", Plugin_API.Config.Type.Bool, false);
            }
            if ((bool)DataHolder.GetConfig().Get("autoupdate"))
            {
                Data.CheckForUpdate();
                DataHolder.UpdatePlugins();
                Data.PreformUpdate();
            }
            if (Data.updateData.ToString() != "")
            {
                DialogResult r = MessageBox.Show("New Update Available! Download?", "Update", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    Process.Start(Data.updaterExe);
                    Thread.Sleep(100);
                    return;
                }
            }
            if (File.Exists(Data.logininfo))
            {
                DataHolder.SetLoginInfo(LoginInfo.Load(Data.logininfo));
            }
            DataHolder.SetBackups(BackupLoader.LoadBackups());
            //try
            //{
                DataHolder.mainWindow = new MainWindow();
                Application.Run(DataHolder.mainWindow);
            //}
            //catch (Exception ex)
            //{
            //    ErrorPage ep = new ErrorPage(ex.ToString());
            //    Application.Run(ep);
            //}
            BackupLoader.SaveBackups(DataHolder.GetBackups());
            if (DataHolder.HasLoginInfo)
            {
                DataHolder.GetLoginInfo().Save(Data.logininfo);
            }
            else
            {
                if (File.Exists(Data.logininfo)) File.Delete(Data.logininfo);
            }
            DataHolder.SaveConfigs();
        }
    }
}