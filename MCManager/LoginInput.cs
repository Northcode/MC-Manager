using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCManager
{
    public partial class LoginInput : Form
    {
        public LoginInput()
        {
            InitializeComponent();
        }

        private void LoginInput_Load(object sender, EventArgs e)
        {

        }

        public new static LoginInfo Show()
        {
            LoginInput frm = new LoginInput();
            DialogResult r = frm.ShowDialog();
            if (r == DialogResult.OK)
            {
                LoginInfo li = new LoginInfo(frm.textBox1.Text,frm.textBox2.Text);
                return li;
            }
            else
            {
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
