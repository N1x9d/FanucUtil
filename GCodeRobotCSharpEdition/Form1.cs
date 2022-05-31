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

        public static Setting sets;
        public static List<Process> prList = new List<Process>();

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
        public bool CheckLayer
        {
            get { return check_Layers.Checked; }
        }
        public bool LaserPass { get { return Laser_pass.Checked; } }
        public string unit
        {
            get { return edit_Units.Text; }
        }

        //public bool SecondPass { get{ return Second_pass.Checked;  } }

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
            sets = new Setting();
            ProcessStartInfo psipy = new ProcessStartInfo();
            psipy.CreateNoWindow = false;
            psipy.WindowStyle = ProcessWindowStyle.Normal;
            string cmdString = @$"/k ""python Scrypts\convert_2_tp_zmq.py """;

            Process Slice = new Process();
            prList.Add(Slice);
            psipy.FileName = "cmd";
            psipy.Arguments = cmdString;
            Slice.StartInfo = psipy;
            Slice.Start();
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
            
                conv.on_btn_Process_clicked();
            
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
           
                conv.on_btn_Open_clicked();
          
           
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
       

        private void check_startStop_Distance_CheckedChanged_1(object sender, EventArgs e)
        {
            if (check_startStop_Distance.Checked)
                CheckDistance.Enabled = true;
            else
                CheckDistance.Enabled = false;
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
                NewRobot.Text.Replace(',', '.');
                RobotTamplate r = new RobotTamplate(NewRobot.Text);
                Form2 form = new Form2(r);
                r.form = form;
                robotsList.Add(new robotVisualize(r, form));
                form.Show();
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
                Form2 form = new Form2( r);
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
                    File.Copy(@"res\robot.ini", fileDir, true);
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

        private void check_Layers_CheckedChanged(object sender, EventArgs e)
        {
            if (check_Layers.Checked)
            {
                edit_Split.ReadOnly= true;
                Laser_pass.Enabled=true;
            }
            else
            {
                edit_Split.ReadOnly = false;
                Laser_pass.Enabled = false;

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //foreach (var process in prList)
            //{
            //    process.Kill();
            //}
            //foreach (Process proc in Process.GetProcessesByName("cmd"))
            //{
            //    //proc.Kill(true);
            //}
        }
    }
}