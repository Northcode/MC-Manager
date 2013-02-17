using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MCMUpdate
{
    public partial class Form1 : Form
    {
        string updateConf = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcm\\update.conf";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(updateConf)) { File.WriteAllText(updateConf, "");  }
            string[] updateData = File.ReadAllLines(updateConf);
            foreach (string update in updateData)
            {
                string[] updateInfo = update.Split(';');
                listBox1.Items.Add((updateInfo[1] == "MCM" ? "MC Manager" : "Plugin: " + updateInfo[2]));
            }
        }

        void Update()
        {
            WebClient wc = new WebClient();
            int i = 0;
            string[] updateData = File.ReadAllLines(updateConf);
            foreach (string update in updateData)
            {
                string[] updateInfo = update.Split(';');
                if (updateInfo[0] == "UPDATE")
                {
                    textBox1.AppendText("Updating: " + (updateInfo[1] == "MCM" ? "Manager" : "Plugin: " + updateInfo[2]) + "\n");
                    textBox1.AppendText("Downloading: " + updateInfo[3] + "\n");
                    wc.DownloadFile(updateInfo[3], updateInfo[2]);
                    progressBar1.Value = i / updateData.Length * 100;
                }
            }
        }
    }
}
