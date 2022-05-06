using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCodeRobotCSharpEdition.Tamplates
{
    public struct Param
    {
        public string ParamName;
        public string ParamValue;

        public Param(string paramName, string paramValue)
        {
            ParamName = paramName;
            ParamValue = paramValue;
        }
    }
    public class Setting
    {
        public List<Param> Params = new List<Param>();


        public void Save()
        {
            StreamWriter sw = new StreamWriter("settings.set");
            foreach (var param in Params)
            {
                sw.WriteLine(param.ParamName + " " + param.ParamValue);
            }
            sw.Close();
        }

        public void Load()
        {
            try
            {
                StreamReader sr = new StreamReader("settings.set");
                Params.Clear();
                while (!sr.EndOfStream)
                {
                    var a = sr.ReadLine().Split(' ');
                    Params.Add(new Param(a[0], a[1]));
                }
                sr.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Import setting errore");
                Settings set = new Settings();
                set.Show();
            }
        }

        public void Load(string patch)
        {
            try
            {
                StreamReader sr = new StreamReader(patch);
                Params.Clear();
                while (sr.EndOfStream)
                {
                    var a = sr.ReadLine().Split(' ');
                    Params.Add(new Param(a[0], a[1]));
                }
                sr.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Import setting errore loading defolt settings");
                Load();
            }
        }

    }
}
