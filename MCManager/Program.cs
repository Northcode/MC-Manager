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
            Data.CheckStartupFolders();
            PluginLoader.LoadPlugins();
            Data.CheckForUpdate();
            DataHolder.UpdatePlugins();
            Data.PreformUpdate();
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //try
            //{
                Application.Run(new Form1());
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
        }
    }
}