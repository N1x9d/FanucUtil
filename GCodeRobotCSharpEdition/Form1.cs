using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCodeRobotCSharpEdition.Robot;
using GCodeRobotCSharpEdition.Tamplates;

namespace GCodeRobotCSharpEdition
{
    public partial class Form1 : Form
    {
        private ConverterGcode conv;
        private ConverterPM convPM;
        public List<Tool> toolList { get; set; } = new List<Tool>();
        public static Setting sets;


        public string W
        {
            get { return edit_W.Text; }
        }

        public string P
        {
            get { return edit_P.Text; }
        }

        public string R
        {
            get { return edit_R.Text; }
        }

        public string Y
        {
            get { return edit_Y.Text; }
        }

        public string Z
        {
            get { return edit_Z.Text; }
        }

        public string Uf
        {
            get { return edit_UF.Text; }
        }

        public string Ut
        {
            get { return edit_UT.Text; }
        }

        public string Es
        {
            get { return edit_Speed.Text; }
        }

        public string Tw
        {
            get { return edit_TW.Text; }
        }

        public string X
        {
            get { return edit_X.Text; }
        }

        public string Tn
        {
            get { return edit_TN.Text; }
        }
        public bool noArc
        {
            get { return check_Noarc.Checked; }
        }
        public bool AutoArc
        {
            get { return check_Autoarc.Checked; }
        }
        public bool WeldSpeed
        {
            get { return check_Weldspeed.Checked; }
        }
        public bool ChechkLayer
        {
            get { return check_Layers.Checked; }
        }
        public string unit
        {
            get { return edit_Units.Text; }
        }

        public bool SecondPass { get{ return Second_pass.Checked;  } }

        public string esplit
        {
            get { return edit_Split.Text; }
        }
        public string outFile
        {
            get { return edit_Outfile.Text; }
        }

        public string Input
        {
            get
            {
                return InputFile.Text;
            }
        }
        public int WaveIndex { get { return int.Parse(WaweInd.Text); } }
        public string FName
        {
            get { return edit_Name.Text; }
        }
        public string econf
        {
            get { return edit_Config.Text; }
        }

        public string InputFileInfo
        {
            get
            {
                return InputFile.Text;
            }
            set
            {
                string filename = value;
                string outfile = filename.Substring(filename.LastIndexOf('\\'));
                outfile = outfile.Substring(1, outfile.LastIndexOf(".")-1);
                string outFileWay= filename.Substring(0,filename.LastIndexOf('\\')+1)+ outfile+@"\";
                
                edit_Name.Text = outfile;
                edit_Outfile.Text = outFileWay;
                InputFile.Text = filename;
            }
        }

        public bool Chechs
        {
            get
            {
                return check_startStop_Distance.Checked;
            }
        }

        public string CheckDist
        {
            get
            {
                return CheckDistance.Text;
            }
        }

        public string OutputFile
        {
            get
            {
                return edit_Outfile.Text;
            }
            set
            {
                edit_Outfile.Text = value;
            }
        }
        public bool WieldShield { get { return WeldSheild.Checked; } }
        public int DefDegree { get { return int.Parse(Degree_def.Text); } }
        
        public Form1()
        {
            InitializeComponent();
            conv = new ConverterGcode(this);
            convPM = new ConverterPM(this);
            sets = new Setting();
            sets.Load();
        }

        private List<robotVisualize> robotsList = new List<robotVisualize>();
        private void WeldSheild_CheckedChanged(object sender, EventArgs e)
        {
            if (!WeldSheild.Checked)
            {
                Degree_def.Enabled = false;
            }
            else
            {
                Degree_def.Enabled = true;
            }
        }
        

        private void Form1_Load_1(object sender, EventArgs e)
        {
        }


      

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (!checkBox2.Checked)
                conv.on_btn_Process_clicked();
            else
                convPM.on_btn_Process_clicked();
            checkBox2.Enabled = true ;
        }

        private void check_startStop_Distance_CheckedChanged(object sender, EventArgs e)
        {
            var a = sender as CheckBox;
            if(a.Checked)
            {
                CheckDistance.Visible = true;
                label21.Visible = true;
                
            }
            else
            {
                CheckDistance.Visible = false;
                label21.Visible = false;
            }
        }

      


        private void OpenFile_Click_1(object sender, EventArgs e)
        {
            if(!checkBox2.Checked)
                conv.on_btn_Open_clicked();
            else
                convPM.on_btn_Open_clicked();
            if(InputFile.Text!="")
                checkBox2.Enabled = false;
        }
        private bool _RO;
        private bool _wave;
        public bool GetRO { get { return _RO; } private set => _RO = value; }
        public bool GetWave { get { return _wave; } private set => _wave = value; }

        private void Wave_CheckedChanged(object sender, EventArgs e)
        {
            if (Wave.Checked)
            {
                WaweInd.Enabled = true;
                GetWave = true;
            }
            else
            {
                WaweInd.Enabled = false;
                GetWave = false;
            }
            
                
        }

        private void RO_CheckedChanged(object sender, EventArgs e)
        {
            if (RO.Checked)
                GetRO = true;
            else
                GetRO = false;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                label1.Text = "PowerMill file name:";
            else
                label1.Text = "GCode file name:";
        }

        private void check_startStop_Distance_CheckedChanged_1(object sender, EventArgs e)
        {
            if (check_startStop_Distance.Checked)
                CheckDistance.Enabled = true;
            else
                CheckDistance.Enabled = false;
        }




//TOOL TAB IN WORKING (FREEZ)
        private void Hummer_CheckedChanged(object sender, EventArgs e)
        {
            if( Hammer.Checked)
            {
                var a = toolList.Where(c => c.ToolType == "Hammer").ToList();
                if (a.Count == 0)
                    toolList.Add(new Tool("Hammer"));
                else
                    a[0].enable = true;
                HammerLayer.Enabled = true;
                HammerX.Enabled = true;
                HammerY.Enabled = true;
                HammerZ.Enabled = true;
                HammerSpeed.Enabled = true;
            }
            else
            {
                var a = toolList.Where(c => c.ToolType == "Hammer").ToList();
                a[0].enable = false;
                HammerLayer.Enabled = false;
                HammerX.Enabled = false;
                HammerY.Enabled = false;
                HammerZ.Enabled = false;
                HammerSpeed.Enabled = false;
            }
        }

        private void Cutter_CheckedChanged(object sender, EventArgs e)
        {
            if (Cutter.Checked)
            {
                var a = toolList.Where(c => c.ToolType == "Cutter").ToList();
                if (a.Count == 0)
                    toolList.Add(new Tool("Cutter"));
                else
                    a[0].enable = true;
                CutterLayer.Enabled = true;
                CutterX.Enabled = true;
                CutterY.Enabled = true;
                CutterZ.Enabled = true;
                CutterSpeed.Enabled = true;
            }
            else
            {
                var a = toolList.Where(c => c.ToolType == "Cutter").ToList();
                a[0].enable = false;
                CutterLayer.Enabled = false;
                CutterX.Enabled = false;
                CutterY.Enabled = false;
                CutterZ.Enabled = false;
                CutterSpeed.Enabled = false;
            }
        }
       
        private void Laser_CheckedChanged(object sender, EventArgs e)
        {
            if (Laser.Checked)
            {
                var a = toolList.Where(c => c.ToolType == "Laser").ToList();
                if (a.Count == 0)
                    toolList.Add(new Tool("Laser"));
                else
                    a[0].enable = true;
                LaserLayer.Enabled = true;
                LaserX.Enabled = true;
                LaserY.Enabled = true;
                LaserZ.Enabled = true;
                LaserSpeed.Enabled = true;
            }
            else
            {
                var a = toolList.Where(c => c.ToolType == "Laser").ToList();
                a[0].enable = false;
                LaserLayer.Enabled = false;
                LaserX.Enabled = false;
                LaserY.Enabled = false;
                LaserZ.Enabled = false;
                LaserSpeed.Enabled = false;
            }
        }

        private void Pyrometer_CheckedChanged(object sender, EventArgs e)
        {
            if (Pyrometer.Checked)
            {
                var a = toolList.Where(c => c.ToolType == "Pyrometer").ToList();
                if (a.Count == 0)
                    toolList.Add(new Tool("Pyrometer"));
                else
                    a[0].enable = true;
                PyrometerLayer.Enabled = true;
                PyrometerX.Enabled = true;
                PyrometerY.Enabled = true;
                PyrometerZ.Enabled = true;
                PyrometerSpeed.Enabled = true;
            }
            else
            {
                var a = toolList.Where(c => c.ToolType == "Pyrometer").ToList();
                a[0].enable = false;
                PyrometerLayer.Enabled = false;
                PyrometerX.Enabled = false;
                PyrometerY.Enabled = false;
                PyrometerZ.Enabled = false;
                PyrometerSpeed.Enabled = false;
            }
        }
      
       
        private void Surfacing_CheckedChanged(object sender, EventArgs e)
        {
            if (Surfacing.Checked)
            {
                var a = toolList.Where(c => c.ToolType == "Surfacing").ToList();
                if (a.Count == 0)
                    toolList.Add(new Tool("Surfacing"));
                else
                    a[0].enable = true;
                SurfacingLayer.Enabled = true;
                SurfacingX.Enabled = true;
                SurfacingY.Enabled = true;
                SurfacingZ.Enabled = true;
                SurfacingSpeed.Enabled = true;
            }
            else
            {
                var a = toolList.Where(c => c.ToolType == "Surfacing").ToList();
                a[0].enable = false;
                SurfacingLayer.Enabled = false;
                SurfacingX.Enabled = false;
                SurfacingY.Enabled = false;
                SurfacingZ.Enabled = false;
                SurfacingSpeed.Enabled = false;
            }
        }
        private void ForAllLyaer(string value)
        {
            HammerLayer.Text = value;
            CutterLayer.Text = value;
            LaserLayer.Text = value;
            PyrometerLayer.Text = value;
            SurfacingLayer.Text = value;

        }
        private void SetX (string tool, float val)
        {
            var a = toolList.Where(c => c.ToolType == tool).ToList();
            a[0].Offcet.X = val;
        }
        private void SetY(string tool, float val)
        {
            var a = toolList.Where(c => c.ToolType == tool).ToList();
            a[0].Offcet.Y = val;
        }
        private void SetZ(string tool, float val)
        {
            var a = toolList.Where(c => c.ToolType == tool).ToList();
            a[0].Offcet.Z = val;
        }
        private void SetSpeed(string tool, float val)
        {
            var a = toolList.Where(c => c.ToolType == tool).ToList();
            a[0].Speed = val;
        }
        private void SetLayer(string tool, int val)
        {
            var a = toolList.Where(c => c.ToolType == tool).ToList();
            a[0].Layer = val;
        }
        private void apply_Click(object sender, EventArgs e)
        {
            if (Hammer.Checked)
            {
                SetX("Hammer", float.Parse(HammerX.Text));
                SetY("Hammer", float.Parse(HammerY.Text));
                SetZ("Hammer", float.Parse(HammerZ.Text));
                SetSpeed("Hammer", float.Parse(HammerSpeed.Text));
                SetLayer("Hammer", int.Parse(HammerLayer.Text));
            }
            if (Cutter.Checked)
            {
                SetX("Cutter", float.Parse(CutterX.Text));
                SetY("Cutter", float.Parse(CutterY.Text));
                SetZ("Cutter", float.Parse(CutterZ.Text));
                SetSpeed("Cutter", float.Parse(CutterSpeed.Text));
                SetLayer("Cutter", int.Parse(CutterLayer.Text));
            }
            if (Laser.Checked)
            {
                SetX("Laser", float.Parse(LaserX.Text));
                SetY("Laser", float.Parse(LaserY.Text));
                SetZ("Laser", float.Parse(LaserZ.Text));
                SetSpeed("Laser", float.Parse(LaserSpeed.Text));
                SetLayer("Laser", int.Parse(LaserLayer.Text));
            }
            if (Pyrometer.Checked)
            {
                SetX("Pyrometer", float.Parse(PyrometerX.Text));
                SetY("Pyrometer", float.Parse(PyrometerY.Text));
                SetZ("Pyrometer", float.Parse(PyrometerZ.Text));
                SetSpeed("Pyrometer", float.Parse(PyrometerSpeed.Text));
                SetLayer("Pyrometer", int.Parse(PyrometerLayer.Text));
            }
            if (Surfacing.Checked)
            {
                SetX("Surfacing", float.Parse(SurfacingX.Text));
                SetY("Surfacing", float.Parse(SurfacingY.Text));
                SetZ("Surfacing", float.Parse(SurfacingZ.Text));
                SetSpeed("Surfacing", float.Parse(SurfacingSpeed.Text));
                SetLayer("Surfacing", int.Parse(SurfacingLayer.Text));
            }
            
           
            
        }

        private void HummerLayer_TextChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
                ForAllLyaer(HammerLayer.Text);
        }

        private void CutterLayer_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                ForAllLyaer(CutterLayer.Text);
        }

        private void LaserLayer_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                ForAllLyaer(LaserLayer.Text);
        }

        private void PyrometerLayer_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                ForAllLyaer(PyrometerLayer.Text);
        }

        private void SurfacingLayer_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                ForAllLyaer(SurfacingLayer.Text);
        }

        private void SendTp_CheckedChanged(object sender, EventArgs e)
        {
            //if (SendTp.Checked)
            //{
            //    if (robotsList != null)
            //        foreach (var rob in robotsList)
            //            rob.extension = ".tp";

            //}
            //else
            //{
            //    if (robotsList != null)
            //        foreach (var rob in robotsList)
            //            rob.extension = ".ls";

            //}

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

       
        private void Await_next_CheckedChanged(object sender, EventArgs e)
        {

        }
        

        private void проверитьОбновленияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Blank();
        }
         public static void Blank()
        {
            MessageBox.Show("В будущих версиях");
        }
        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings set = new Settings();
            set.Show();
        }

        

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {   
                Blank();
                tabControl1.SelectedIndex = 0;

            }
               
        }

        // SEND TO ROBOT TAB

        public void UpdateRobotTable()
        {
            dataGridView1.Rows.Clear();
            var i = 0;
            foreach (var item in robotsList)
            {

                dataGridView1.Rows.Add(i, item.robot.Addres, item.robot.stateTempl.CurrentState);
                dataGridView1.Rows[i].Cells[2].Style.ForeColor = item.robot.stateTempl.color;
                i++;
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            robotsList[e.RowIndex].form.Show();

        }

        private void Print_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.ShowNewFolderButton = false;
            

            if (FBD.ShowDialog() == DialogResult.OK)
            {

                var dirName = FBD.SelectedPath.Substring(FBD.SelectedPath.LastIndexOf("\\") + 1);
                textBox1.Text = dirName;
               
            }
            
        }
        private void UpdateAllStates(object Sender, EventArgs e)
        {
            UpdateRobotTable();
        }


        private void Add_Click(object sender, EventArgs e)
        {
            if (NewRobot.Text != "")
            {
                NewRobot.Text.Replace(',', '.');
                RobotTamplate r = new RobotTamplate(NewRobot.Text);
                Form2 form = new Form2(r.Addres, r);
                r.form = form;
                robotsList.Add(new robotVisualize(r, form));
            }
            UpdateRobotTable();

        }


        private void MakeTP_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            OpenFileDialog FBD = new OpenFileDialog();
            FBD.Filter = "ls files (*.ls)|*.ls|All files (*.*)|*.*";
            FBD.FilterIndex = 2;
            FBD.RestoreDirectory = true;
            //MessageBox.Show("Функция не протестирована!!! но должна роаботать");
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                var filePath = FBD.FileName;
                var fileDir = filePath.Substring(0, filePath.LastIndexOf('\\'));
                //Read the contents of the file into a stream
                if (!System.IO.File.Exists(fileDir + "\\robot.ini"))
                {
                    StreamWriter file = new StreamWriter(fileDir + "\\robot.ini");
                    file.WriteLine("[WinOLPC_Util]");
                    file.WriteLine("Robot=\\C\\Users\\user\\Documents\\My Workcells\\WeldPRO1\\Robot_1");
                    file.WriteLine("Version=V7.70-1");
                    file.WriteLine(@"Path=C:\Program Files (x86)\FANUC\WinOLPC\Versions\V770-1\bin");
                    file.WriteLine(@"Support=C:\Users\user\Documents\My Workcells\WeldPRO1\Robot_1\support");
                    file.WriteLine(@"Output=C:\Users\user\Documents\My Workcells\WeldPRO1\Robot_1\output");
                    file.Close();
                }
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = @$"/k ""maketp {filePath.Substring(filePath.LastIndexOf('\\') + 1)}""",//закрываем консоль
                    WorkingDirectory = fileDir,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
        }
    }
}