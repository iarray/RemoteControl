using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Plugin
{
    public class ConfigHelper
    {
        public string Path { get;private set; }
        private List<string> keys = new List<string>();
        private List<string> values = new List<string>();
        public ConfigHelper(string path)
        {
            Path = path;
            load();
        }

        private void load()
        {
            if(File.Exists(Path))
            {
                using (StreamReader sr = new StreamReader(Path))
                {
                    while(!sr.EndOfStream)
                    {
                        string str = sr.ReadLine();
                        int i = str.IndexOf(":");
                        if(i>0)
                        {
                            keys.Add(str.Substring(0,i));
                            values.Add(str.Substring(i + 1));
                        }
                    }
                }
            }
           
        }

        public void Put(string key,string value)
        {
            if(string.IsNullOrEmpty(key)||string.IsNullOrEmpty(value))
            {

            }
            else
            {
                int index=keys.IndexOf(key);
                if (index < 0)
                {
                    keys.Add(key);
                    values.Add(value);
                }else
                {
                    keys[index] = key;
                    values[index] = value;
                }
            }
        }

        public void Commit()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Path))
                {
                    for(int i=0;i<keys.Count;i++)
                    {
                        sw.WriteLine(keys[i] + ":" + values[i]);
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        public string Get(string key,string defaultValue)
        {
            int index=keys.IndexOf(key);
            if(index>=0)
            {
                return values[index];
            }
            return defaultValue;
        }
    }
}
