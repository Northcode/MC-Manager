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
    public partial class ErrorPage : Form
    {
        private string Error;

        public ErrorPage(string Error)
        {
            InitializeComponent();
            this.Error = Error;
            txtError.Text = "AN ERROR HAS OCURRED!\r\nTECHNICAL INFORMATION IS BEEING SENT TO OUR DATABASE!\r\nFEEL FREE TO REPORT THIS ERROR:\r\n" + Error;
        }

        private void ErrorPage_Load(object sender, EventArgs e)
        {
            string response = ErrorReporter.LogError(Error);
            txtError.Text = "Error sendt, response: " + response;
        }
    }
}