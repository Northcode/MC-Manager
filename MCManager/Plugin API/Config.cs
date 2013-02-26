using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MCManager.Plugin_API
{
    public class Config
    {
        public enum Type
        {
            Text,
            Integer,
            Decimal,
            Bool,
        }

        string name;
        Dictionary<string, Tuple<Type,object>> data;

        public Config(string Name)
        {
            name = Name;
            data = new Dictionary<string, Tuple<Type, object>>();
        }

        public void Set(string key, Type type, object val)
        {
            if (data.ContainsKey(key))
            {
                data[key] = new Tuple<Type,object>(type,val);
            }
            else
            {
                data.Add(key, new Tuple<Type, object>(type, val));
            }
        }

        public object Get(string key)
        {
            if (data.ContainsKey(key))
            {
                return data[key].Item2;
            }
            else
            {
                throw new Exception(String.Format("Key: {0}, not found in config: {1}",key,name));
            }
        }

        public bool Has(string key)
        {
            return data.ContainsKey(key);
        }

        public void Remove(string key)
        {
            data.Remove(key);
        }

        public Type GetNodeType(string key)
        {
            if (data.ContainsKey(key))
            {
                return data[key].Item1;
            }
            else
            {
                throw new Exception(String.Format("Key: {0}, not found in config: {1}", key, name));
            }
        }

        public string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendLine(String.Format("<node name=\"{0}\">", name));
            foreach (KeyValuePair<string,Tuple<Type,object>> item in data)
            {
                xml.AppendLine(String.Format("<item key=\"{0}\" type=\"{1}\">{2}</item>", item.Key, item.Value.Item1.ToString(), item.Value.Item2.ToString()));
            }
            xml.AppendLine("</node>");
            return xml.ToString();
        }

        internal string GetName()
        {
            return name;
        }

        internal Dictionary<string, Tuple<Type, object>> GetAll()
        {
            return data;
        }

        internal void SetAll(Dictionary<string, Tuple<Type, object>> unmapped)
        {
            data = unmapped;
        }
    }
}
