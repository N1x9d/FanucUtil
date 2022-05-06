using GCodeRobotCSharpEdition.Tamplates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCodeRobotCSharpEdition
{
    
    public partial class Settings : Form
    {
        
        
        public Settings()
        {
            InitializeComponent();
            setFormData();
        }

        

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!IgnorRule)
            {
                if (NextLayerBD.Checked)
                {
                    Form1.sets.Params.Add(new Param(RemoveSpace(NextLayerBD.Name.ToString()), textBox2.Text));
                }
                else
                {
                    Form1.sets.Params.RemoveAll(c => c.ParamName == RemoveSpace(NextLayerBD.Name.ToString()));
                }
            }
        }

       

        private void Save_Click(object sender, EventArgs e)
        {
            Form1.sets.Save();
        }
        private bool IgnorRule = false;
        private void NextLayerDelay_CheckedChanged(object sender, EventArgs e)
        {
            if (!IgnorRule)
            {
                if (NextLayerDelay.Checked)
                {
                    Form1.sets.Params.Add(new Param(RemoveSpace(NextLayerDelay.Name.ToString()), textBox1.Text));
                }
                else
                {
                    Form1.sets.Params.RemoveAll(c => c.ParamName == RemoveSpace(NextLayerDelay.Name.ToString()));
                }
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "SettingFile (.set) |*.set; | Other files (*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
           Form1.sets.Load( openFile.FileName);
        }
        private void setFormData()
        {
            IgnorRule = true;
            var a = Form1.sets.Params;
            foreach (var param in a)
            {
                if (SetSpace( param.ParamName) == NextLayerBD.Name.ToString())
                {   NextLayerBD.Checked = true;
                    textBox2.Text = param.ParamValue;
                    
                }
                else if (SetSpace(param.ParamName) == NextLayerDelay.Name.ToString())
                {   
                    NextLayerDelay.Checked = true;
                    textBox1.Text = param.ParamValue;
                    
                }
            }
            IgnorRule = false;
        }

        private string RemoveSpace(string inp)
        {
            return inp.Replace(" ", "_");
        }
        private string SetSpace(string inp)
        {
            return inp.Replace("_", " ");
        }

        private void Settings_Load(object sender, EventArgs e)
        {

        }

        private void Drop_Click(object sender, EventArgs e)
        {
            Form1.Blank();   
        }
    }
}
