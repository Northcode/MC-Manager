using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCManager.Backups
{
    public interface IBackupFormat
    {
        IBackup Load(string file);

        void Save(string file, IBackup backup);

        byte getSignature();

        IBackup CreateBackup();

        string GetFormatName();
    }
}