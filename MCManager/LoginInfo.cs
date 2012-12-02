using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MCManager
{
    public class LoginInfo
    {
        private string name;
        private string password;

        internal string GetName()
        {
            return name;
        }

        internal string GetDecryptedPassword()
        {
            return Crypto.DecryptStringAES(password, name);
        }

        public LoginInfo(string name, string password)
        {
            this.name = name;
            this.password = Crypto.EncryptStringAES(password, name);
        }

        public LoginInfo()
        {
        }

        public void Save(string path)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.OpenOrCreate));
            bw.Write(name);
            bw.Write(password);
            bw.Close();
        }

        public string GetPassword()
        {
            return password;
        }

        internal static LoginInfo Load(string path)
        {
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open));
            LoginInfo li = new LoginInfo();
            li.name = br.ReadString();
            li.password = br.ReadString();
            br.Close();
            return li;
        }
    }
}