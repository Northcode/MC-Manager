﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MCManager.Backups;

namespace MCManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Size = new Size(404, 390);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                Size = new Size(404, 390);
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                Size = new Size(404, 390);
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                Size = new Size(850, 500);
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                Size = new Size(240, 310);
                CheckLoginInfo();
            }
        }

        private void CheckLoginInfo()
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
            UpdateBackupList(); 
            UpdatePluginList();
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

        private void UpdateBackupList()
        {
            treeView1.Nodes.Clear();
            foreach (IBackupFormat format in BackupLoader.formats)
            {
                TreeNode node = new TreeNode(format.GetFormatName());
                node.ToolTipText = format.GetFormatName();
                foreach (IBackup backup in DataHolder.GetBackups())
                {
                    if (backup.GetFormat().GetType() == format.GetType())
                    {
                        TreeNode n = new TreeNode(backup.GetName());
                        n.ToolTipText = backup.GetDescription();
                        node.Nodes.Add(n);
                    }
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

        private void button1_Click(object sender, EventArgs e)
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

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

        private void UpdatePluginList()
        {
            listPlugins.Items.Clear();
            foreach (IBackupFormat format in BackupLoader.formats)
            {
                listPlugins.Items.Add(format.GetFormatName() + " with sig: " + format.getSignature());
            }
        }
    }
}