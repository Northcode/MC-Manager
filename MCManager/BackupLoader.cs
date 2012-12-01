using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCManager.Backups;

namespace MCManager
{
    public class BackupLoader
    {
        internal static List<IBackupFormat> formats = new List<IBackupFormat>();

        public void AddFormat(IBackupFormat format)
        {
            formats.Add(format);
        }

        public List<IBackup> loadbackups()
        {
            List<IBackup> backups = new List<IBackup>();

            foreach (string file in Directory.GetFiles(Data.backupdir))
            {
            }

            return backups;
        }
    }
}