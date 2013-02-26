using MCManager.Backups;
using MCManager.Plugin_API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MCManager
{
    public partial class MainWindow : Form
    {
        bool cbxMCLoaded = false;

        public MainWindow()
        {
            InitializeComponent();
            Size = new Size(404, 390);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0 || tabControl1.SelectedIndex == 1 || tabControl1.SelectedIndex == 2 || tabControl1.SelectedIndex == 5)
            {
                Size = new Size(404, 390);
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                Size = new Size(850, 500);
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                Size = new Size(240, 310);
                CheckLoginInfo();
            }
        }

        public void CheckLoginInfo()
        {
            if (DataHolder.HasLoginInfo)
            {
                btnLoginInfo.Text = "Delete saved";
                lblLoginInfo.Text = "Stored info: \r\nusername: " + DataHolder.GetLoginInfo().GetName() + "\r\npassword (encrypted): " + DataHolder.GetLoginInfo().GetPassword();
            }
            else
            {
                btnLoginInfo.Text = "Store";
                lblLoginInfo.Text = "No stored information";
            }
        }

        private void btnLoginInfo_Click(object sender, EventArgs e)
        {
            if (!DataHolder.HasLoginInfo)
            {
                string name = txtLoginName.Text;
                string password = txtLoginPassword.Text;
                DataHolder.SetLoginInfo(new LoginInfo(name, password));
            }
            else
            {
                DataHolder.DeleteLoginInfo();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbxNewBackup.Items.Clear();
            cbxNewBackup.Items.Add("New Backup");
            foreach (IBackupFormat format in BackupLoader.formats)
            {
                cbxNewBackup.Items.Add(format.GetFormatName());
            }
            cbxNewBackup.SelectedIndex = 0;
            cbxMCStart.SelectedIndex = 0;
            cbxMCLoaded = true;
            UpdateBackupList(); 
            UpdatePluginList();
            UpdateConfigList();
            foreach (string line in changelog.loglines)
            {
                richTextBox1.AppendText(" - " + line);
            }
        }

        public void UpdateConfigList()
        {
            lstConfigs.Items.Clear();
            foreach (Plugin_API.Config con in DataHolder.GetConfigs())
            {
                lstConfigs.Items.Add(con.GetName());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxNewBackup.SelectedIndex != 0)
            {
                IBackup backup = BackupLoader.formats[cbxNewBackup.SelectedIndex - 1].CreateBackup();
                if (backup != null)
                {
                    DataHolder.AddBackup(backup);
                }
                UpdateBackupList();
                cbxNewBackup.SelectedIndex = 0;
            }
        }

        public void UpdateBackupList()
        {
            treeView1.Nodes.Clear();
            foreach (IBackupFormat format in BackupLoader.formats)
            {
                TreeNode node = new TreeNode(format.GetFormatName());
                node.ToolTipText = format.GetFormatName();
                int i = 0;
                foreach (IBackup backup in DataHolder.GetBackups())
                {
                    if (backup.GetFormat().GetType() == format.GetType())
                    {
                        TreeNode n = new TreeNode(backup.GetName());
                        n.ToolTipText = backup.GetDescription();
                        n.Tag = i;
                        node.Nodes.Add(n);
                    }
                    i++;
                }
                treeView1.Nodes.Add(node);
            }
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            DataHolder.GetBackups().FindAll(b => treeView1.SelectedNode.Text == b.GetName()).ForEach(b => b.Extract());
            MessageBox.Show("Backup restored");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataHolder.RemoveBackup(treeView1.SelectedNode.Text);
            UpdateBackupList();
        }

        public void StartMinecraftWithLauncher()
        {
            Process[] javas;

            bool start = true;

            if ((javas = Process.GetProcessesByName("javaw")).Length > 0)
            {
                foreach (Process p in javas)
                {
                    if (p.MainWindowTitle.Equals("minecraft", StringComparison.CurrentCultureIgnoreCase))
                    {
                        start = false;
                        if (MessageBox.Show("Minecraft is allready running, do you want to close it before restarting. \r\nIf no minecraft will not start a second time.", "Minecraft is allready running", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            p.Kill();
                            start = true;
                        }
                        break;
                    }
                }
            }

            if (start)
            {
                string param = "";
                if (DataHolder.HasLoginInfo)
                {
                    param += DataHolder.GetLoginInfo().GetName() + " " + DataHolder.GetLoginInfo().GetDecryptedPassword();
                }
                if (checkBox1.Checked)
                {
                    Directory.Delete(Data.minecraftbin, true);
                }
                else
                {
                }
                Process.Start(Data.minecraftexe, param);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Dll files | *.dll";
            of.ShowDialog();
            if (File.Exists(of.FileName))
            {
                PluginLoader.LoadPlugin(of.FileName);
                if (File.Exists(Data.pluginfolder + Path.GetFileName(of.FileName)))
                {
                    MessageBox.Show("Plugin allready found in plugins folder, perhaps you allredy have it or an older version? Close the program and ");
                }
                else
                {
                    File.Copy(of.FileName, Data.pluginfolder + Path.GetFileName(of.FileName));
                }
                UpdatePluginList();
                MessageBox.Show("Plugin loaded! To load backups of this format, please restart the program.");
            }
        }

        public void UpdatePluginList()
        {
            listPlugins.Items.Clear();
            foreach (IBackupFormat format in BackupLoader.formats)
            {
                listPlugins.Items.Add(format.GetFormatName() + " with sig: " + format.getSignature());
            }
        }

        public void AddTab(string tabName)
        {
            tabControl1.TabPages.Add(tabName, tabName);
        }

        public void RemoveTab(string tabName)
        {
            tabControl1.TabPages.RemoveByKey(tabName);
        }

        public TabPage GetTab(string tabName)
        {
            return tabControl1.TabPages[tabName];
        }

        public void StartMinecraftWithoutLauncher()
        {
            LoginInfo li;
            if (DataHolder.HasLoginInfo)
            {
                li = DataHolder.GetLoginInfo();
            }
            else
            {
                li = LoginInput.Show();
                if (li == null)
                {
                    MessageBox.Show("Please type in username and password!", "Login Error");
                    return;
                }
            }
            try
            {
                WebClient wc = new WebClient();
                string loginURI = String.Format("http://login.minecraft.net/?user={0}&password={1}&version=14", li.GetName(), li.GetDecryptedPassword());
                string str = wc.DownloadString(loginURI);

                if (str != "Bad login")
                {
                    string[] args = str.Split(':');

                    Process mc = new Process();
                    mc.StartInfo.FileName = "java.exe";
                    mc.StartInfo.Arguments = String.Format("-Xincgc -Xmx1024m -cp \"" + Data.minecraftbin + "minecraft.jar;" + Data.minecraftbin + "lwjgl.jar;" + Data.minecraftbin + "lwjgl_util.jar;" + Data.minecraftbin + "jinput.jar\" -Djava.library.path=\"" + Data.minecraftbin + "natives\" net.minecraft.client.Minecraft {0} {1}", args[2], args[3]);
                    mc.Start();
                }
                else
                {
                    MessageBox.Show("Invalid Login! try again...");
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show("Can't connect to login.minecraft.net, try again later or start with launcher to play offline\r\nMore information: " + ex.ToString());
            }
        }

        private void cbxMCStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxMCLoaded)
            {
                if (cbxMCStart.SelectedIndex == 0)
                {
                    StartMinecraftWithLauncher();
                }
                else if (cbxMCStart.SelectedIndex == 1)
                {
                    StartMinecraftWithoutLauncher();
                }
            }
        }

        private void cbxMCStart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (cbxMCLoaded)
            {
                if (cbxMCStart.SelectedIndex == 0)
                {
                    StartMinecraftWithLauncher();
                }
                else if (cbxMCStart.SelectedIndex == 1)
                {
                    StartMinecraftWithoutLauncher();
                }
            }
        }

        private void lstConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstConfigs.SelectedItem != null)
            {
                Dictionary<string, object> map = MapConfig(DataHolder.GetConfig(lstConfigs.SelectedItem.ToString()));
                propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter<string, object>(map);
            }
        }

        private Dictionary<string, object> MapConfig(Config config)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (KeyValuePair<string,Tuple<Config.Type,object>> item in config.GetAll())
            {
                result.Add(item.Key, item.Value.Item2);
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DictionaryPropertyGridAdapter<string, object> data = propertyGrid1.SelectedObject as DictionaryPropertyGridAdapter<string, object>;
            if (data != null)
            {
                Dictionary<string,object> mapped = data.GetDictionary();
                Dictionary<string,Tuple<Config.Type,object>> unmapped = ReverseConfigMap(mapped);
                DataHolder.GetConfig(lstConfigs.SelectedItem.ToString()).SetAll(unmapped);
            }
        }

        private Dictionary<string,Tuple<Config.Type,object>> ReverseConfigMap(Dictionary<string, object> mapped)
        {
            Dictionary<string, Tuple<Config.Type, object>> result = new Dictionary<string, Tuple<Config.Type, object>>();
            foreach (KeyValuePair<string,object> item in mapped)
            {
                result.Add(item.Key, new Tuple<Config.Type, object>(GetConfigTypeOf(item.Value), item.Value));
            }
            return result;
        }

        private Config.Type GetConfigTypeOf(object p)
        {
            if (p.GetType() == typeof(bool))
            {
                return Config.Type.Bool;
            }
            else if (p.GetType() == typeof(double))
            {
                return Config.Type.Decimal;
            }
            else if (p.GetType() == typeof(int))
            {
                return Config.Type.Integer;
            }
            else if (p.GetType() == typeof(string))
            {
                return Config.Type.Text;
            }
            else
            {
                throw new Exception("Unknown Config type:" + p.GetType());
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag is int)
            {
                textBox1.Text = DataHolder.GetBackups()[(int)treeView1.SelectedNode.Tag].GetDescription();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            label7.Text = "MCManager - Version: " + Data.VERSION;
        }
    }
}