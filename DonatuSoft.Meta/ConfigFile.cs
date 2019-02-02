using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonatuSoft.Meta
{
    public class ConfigFile
    {
        string fájlNév;
        public string FájlNév { get { return fájlNév; } }

        public ConfigFile(string fájlNév)
        {
            this.fájlNév = fájlNév;
        }

        public string Read(string keresettKey)
        {
            StreamReader sr = new StreamReader(fájlNév);

            string value = "";
            while (!sr.EndOfStream)
            {
                string sor = sr.ReadLine();
                if (sor.StartsWith(keresettKey + "="))
                {
                    value = sor.Split('=')[1];
                    break;
                }
            }
            sr.Close();

            return value;
        }

        public void Delete(string törlendőKey)
        {
            List<string> sorok = new List<string>();

            StreamReader sr = new StreamReader(fájlNév);
            while (!sr.EndOfStream)
                sorok.Add(sr.ReadLine());
            sr.Close();

            int i = 0;
            while (i < sorok.Count && !sorok[i].StartsWith(törlendőKey + "="))
                i++;
            if (i < sorok.Count)
                sorok.RemoveAt(i);

            StreamWriter sw = new StreamWriter(fájlNév);
            foreach (string s in sorok)
                sw.WriteLine(s);
            sw.Close();
        }
        
        public void Write(string key, string value)
        {
            Delete(key);

            StreamWriter sw = new StreamWriter(fájlNév, true);
            sw.WriteLine("{0}={1}", key, value);
            sw.Close();
        }
    }
}
