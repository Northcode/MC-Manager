using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MCManager
{
    public class ErrorReporter
    {
        static private WebClient wc = new WebClient();
        static private string url = "http://indev.northcode.no/data/do.php?p=mcme&";

        public bool LogError(string message)
        {
            string str = wc.DownloadString(new Uri(url + "e=" + message.Replace("&", "§§")));
            return str == "success";
        }
    }
}