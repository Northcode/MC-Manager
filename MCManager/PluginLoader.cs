using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MCManager.Backups;

namespace MCManager
{
    public class PluginLoader
    {
        public static void LoadPlugin(string file)
        {
            Assembly assembly = Assembly.LoadFile(file);

            foreach (Type t in assembly.GetTypes())
            {
                if (typeof(IBackupFormat).IsAssignableFrom(t))
                {
                    IBackupFormat format = Activator.CreateInstance(t) as IBackupFormat;
                    BackupLoader.formats.Add(format);
                }
            }
        }

        public static void LoadPlugins()
        {
            foreach (string dll in Directory.GetFiles(Data.pluginfolder, "*.dll"))
            {
                LoadPlugin(dll);
            }
        }
    }
}