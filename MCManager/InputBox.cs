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
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public static string Show(string title, string message)
        {
            InputBox i = new InputBox();
            i.Text = title;
            i.label1.Text = message;
            DialogResult r = i.ShowDialog();
            return (r == DialogResult.OK ? i.textBox1.Text : null);
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

        private void InputBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
