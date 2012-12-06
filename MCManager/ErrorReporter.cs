using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCManager
{
    public class ErrorReporter
    {
        static private WebClient wc = new WebClient();
        static private string url = "http://indev.northcode.no/data/do.php?p=mcme&";

        public static string LogError(string message)
        {
            string Url = url + "e=" + message.Replace("&", "§§").Replace("\r", "").Replace("\n", "%0A").Replace("\\", "/");
            MessageBox.Show(Url);
            string str = wc.DownloadString(new Uri(Url));
            return str;
        }

        public static void Error(string msg)
        {
            MessageBox.Show(msg, "Error!");
        }
    }
}