using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCManager.Plugin_API
{
    public interface IUpdater
    {
        bool CheckForUpdates();

        string GetUpdatePath();

        string GetLocalPath();
    }
}
