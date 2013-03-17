using System.Collections.Generic;
using System.IO;
using System.Text;
using MCManager.Backups;
using MCManager.Plugin_API;
using System.Xml;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace MCManager
{
    public static class DataHolder
    {
        private static LoginInfo login;
        private static List<IBackup> backups = new List<IBackup>();
        private static List<IUpdater> updaters = new List<IUpdater>();
        internal static MainWindow mainWindow;
        private static List<Config> configs = new List<Config>();
        private static List<IPlugin> plugins = new List<IPlugin>();
        private static ImageList BackupImages;

        public static void AddBackup(IBackup backup)
        {
            backups.Add(backup);
        }

        public static void RemoveBackup(string name)
        {
            File.Delete(backups.Find(p => p.GetName() == name).GetFilePath());
            backups.RemoveAll(b => b.GetName() == name);
        }

        public static void RemoveBackup(int index)
        {
            File.Delete(backups[index].GetFilePath());
            backups.RemoveAt(index);
        }

        public static void RemoveBackup(IBackup backup)
        {
            File.Delete(backups.Find(b => b.GetName() == backup.GetName()).GetFilePath());
            backups.Remove(backup);
        }

        public static List<IBackup> GetBackups()
        {
            return backups;
        }

        public static void SetBackups(List<IBackup> Backups)
        {
            backups = Backups;
        }

        public static LoginInfo GetLoginInfo()
        {
            return login;
        }

        public static void SetLoginInfo(LoginInfo Login)
        {
            login = Login;
        }

        public static void DeleteLoginInfo()
        {
            login = null;
        }

        public static bool HasLoginInfo
        {
            get
            {
                return login != null;
            }
        }

        internal static void AddUpdater(IUpdater updater)
        {
            updaters.Add(updater);
        }

        internal static void UpdatePlugins()
        {
            foreach (IUpdater updater in updaters)
            {
                if (updater.CheckForUpdates())
                {
                    Data.updateData.AppendLine("UPDATE;PLUGIN;" + updater.GetLocalPath() + ";" + updater.GetUpdatePath());
                }
            }
        }

        public static MainWindow GetMainWindow()
        {
            return mainWindow;
        }

        public static Config GetConfig()
        {
            return GetConfig("main"); 
        }

        public static Config GetConfig(string name)
        {
            return configs.Find(p => p.GetName() == name);
        }

        public static void AddConfig(Config config)
        {
            if (!configs.Contains(config))
            {
                configs.Add(config);
            }
        }

        public static void RemoveConfig(string name)
        {
            configs.RemoveAll(p => p.GetName() == name);
        }

        public static void LoadConfigs()
        {
            FileStream xmlfs = new FileStream(Data.configpath, FileMode.Open);
            XmlReader xmlr = XmlReader.Create(xmlfs);
            
            Config current = null;
            while (xmlr.Read())
            {
                if (xmlr.Name == "config" && xmlr.NodeType == XmlNodeType.Element)
                {
                        
                }
                else if (xmlr.Name == "node" && xmlr.NodeType == XmlNodeType.Element)
                {
                    string nodename = xmlr.GetAttribute("name");
                    current = new Config(nodename);
                    configs.Add(current);
                }
                else if (xmlr.Name == "item" && xmlr.NodeType == XmlNodeType.Element)
                {
                    string itemName = xmlr.GetAttribute("key");
                    string itemType = xmlr.GetAttribute("type");
                    Config.Type iType = (Config.Type)Enum.Parse(typeof(Config.Type), itemType);
                    object data = null;
                    try
                    {
                        if (iType == Config.Type.Bool)
                        {
                            data = Convert.ToBoolean(xmlr.ReadElementContentAsString());
                        }
                        else if (iType == Config.Type.Decimal)
                        {
                            data = Convert.ToDouble(xmlr.ReadElementContentAsString());
                        }
                        else if (iType == Config.Type.Integer)
                        {
                            data = Convert.ToInt32(xmlr.ReadElementContentAsString());
                        }
                        else if (iType == Config.Type.Text)
                        {
                            data = xmlr.ReadElementContentAsString();
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("XML Parse Error: ", e);
                    }
                    if (current != null)
                    {
                        if (data != null)
                        {
                            current.Set(itemName, iType, data);
                        }
                    }
                }
            }
            xmlr.Close();
            xmlfs.Close();

            if (configs.FindAll(p => p.GetName() == "main").Count != 1)
            {
                configs.Add(new Config("main"));
            }
        }
    
        public static void SaveConfigs()
        {
            StringBuilder xmlbuilder = new StringBuilder();
            xmlbuilder.AppendLine("<config>");
            foreach (Config c in configs)
            {
                xmlbuilder.AppendLine(c.ToXML());
            }
            xmlbuilder.AppendLine("</config>");
            File.WriteAllText(Data.configpath, xmlbuilder.ToString());
        }

        public static List<Config> GetConfigs()
        {
            return configs;
        }

        public static void AddBackupImage(string name, Image image)
        {
            BackupImages.Images.Add(image);
            BackupImages.Images.Keys.Add(name);
        }

        internal static ImageList GetBackupImages()
        {
            return BackupImages;
        }

        internal static void LoadImages()
        {
            foreach (IBackupFormat  format in BackupLoader.formats)
            {
                Image img = format.GetImage();
                if (img != null)
                {
                    AddBackupImage(format.GetImageName(), format.GetImage());
                }
            }
        }

        internal static void AddPlugin(IPlugin plugin)
        {
            plugins.Add(plugin);
        }

        internal static void StartPlugins(MainWindow wnd)
        {
            plugins.ForEach(p => p.Start(wnd));
        }

        internal static void StopPlugins()
        {
            plugins.ForEach(p => p.Stop());
        }
    }
}