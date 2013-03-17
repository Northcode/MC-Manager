using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCManager.Plugin_API
{
    public interface IPlugin
    {
        void Start(MainWindow mainwnd);

        void Stop();
    }
}
