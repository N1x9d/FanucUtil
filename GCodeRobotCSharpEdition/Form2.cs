﻿using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCodeRobotCSharpEdition.Robot;
using System.Diagnostics;
using System.Threading;
using GCodeRobotCSharpEdition.Tamplates;
using NetMQ.Sockets;
using NetMQ;

namespace GCodeRobotCSharpEdition
{
    public partial class Form2 : Form
    {
        public string PrPatch { get; set; }
        public static CancellationTokenSource cts = new CancellationTokenSource(); 
        public CancellationToken ct= cts.Token;
        private StateTempl _curState= new StateTempl("Ready to print",Color.Green);
        private bool _isTranslating;
        public StateTempl CurState
        {
            get
            {
                return _curState;
            }
            set
            {
                _curState = value;
                
               
              

            }
        }
        public RobotTamplate robot;
        private System.Windows.Forms.Timer myTimer;

        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            RobotState.Text = CurState.CurrentState;
            RobotState.ForeColor = CurState.color;
            //robot.ChechConnection();
            var a = LogList.Print();
            textBox2.Text = "";
            textBox2.AppendText(a);
            RobotState.Text = _curState.CurrentState.ToString();
            RobotState.ForeColor = _curState.color;

            if (robot.isPrinting)
            {
                Print.Enabled = false;
                //StartPrint.Enabled = false;
                if (Await_layer.Checked)
                    if (robot.SendNextFile)
                    {
                        //myTimer.Stop();
                        StartPrint.Enabled = true;
                        Collection.Enabled = true;
                        //Repeat.Enabled = true;
                        //Skip.Enabled = true;
                    }
                    else
                    {

                        StartPrint.Enabled = false;
                        Collection.Enabled = false;
                        //PrintNext.Text = "Print next";
                        //Repeat.Enabled = false;
                        //Repeat.Text = "Repeat layer";
                        //Skip.Enabled = false;
                        //Skip.Text = "Skip Layer";
                    }

            }
            else
            {
                Print.Enabled = true;
                Collection.Enabled = true;
            }
            if (_isTranslating)
            {
                using (var client = new RequestSocket())
                {
                    client.Connect($"tcp://localhost:5001");
                    client.SendFrame($"states");
                    var msg2 = client.ReceiveFrameString();
                    LogList.Add(3, msg2);
                }
            }

        }

        public Form2(string ip, RobotTamplate rtmpl)
        {
            InitializeComponent();
            CurState= new StateTempl("Ready to print", Color.Green); 
            robot = rtmpl;
            textBox2.Text = "aaa";   
            this.Text = ip;
            myTimer = new System.Windows.Forms.Timer();
            myTimer.Tick += new EventHandler(TimerEventProcessor);

            // Sets the timer interval to 5 seconds.
            myTimer.Interval = 100;
            myTimer.Start();

        }



        System.Windows.Forms.Timer PrintTimer = new System.Windows.Forms.Timer();
       
        
        
        private void Print_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.ShowNewFolderButton = false;
            robot.PrParam.curnumb = 1;

            if (FBD.ShowDialog() == DialogResult.OK)
            {
                PrPatch = FBD.SelectedPath;
                var dirName = FBD.SelectedPath.Substring(FBD.SelectedPath.LastIndexOf("\\") + 1);
                textBox1.Text= dirName;
                var filesCount = GetLSCount(FBD.SelectedPath);
                robot.PrParam.count = filesCount;

                robot.PrParam.patch = FBD.SelectedPath;
                robot.PrParam.filename = dirName;

            }
            StartPrint.Enabled = true;
            
           
        }
       
        public int timerCount { 
            get
            {

                return timerCount;
            }
            set 
            {
                if(timerButtonlenght-value !=0 && value != 0)
                    StartPrint.Text = $"Следующий файл {timerButtonlenght - timerCount}";
                else
                    StartPrint.Text = $"Следующий файл";
            } 
        }
        private int timerButtonlenght;
        private bool TimerWorking = false;
        private void NextBEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            timerCount++;
            if (timerCount < timerButtonlenght)
            {
                


            }
            else
            {
                
                timerCount = 0;
                Collection.Enabled = false;
                StartPrint.Text = $"Следующий файл";
                PrintTimer.Stop();
                
                 var a =Collection.Items.IndexOf(Collection.Text);
                 var task = new Task(() => robot.printNext(Collection.Items[a].ToString(), a));
                 TimerWorking = false;
                 task.Start();
                
            }
        }

        private int GetLSCount(string patch)
        {
            Collection.Items.Clear();
            var dirName = patch.Substring(patch.LastIndexOf("\\") + 1);
            var filesCount = new DirectoryInfo(patch).GetFiles().Length;
            var res = 0;
            if (File.Exists(patch + "\\" + dirName + ".tp"))
            {
                res++;
                Collection.Items.Add(dirName + ".tp");
            }
            else if (File.Exists(patch + "\\" + dirName + $"_1" + ".tp"))
            {
                res++;
                Collection.Items.Add(dirName + $"_1" + ".tp");
            }
                
            else
                return 0;
            for (int i = 1; i < filesCount; i++)
            {
                if (File.Exists(patch + "\\" + dirName + $"_{i+1}" + ".tp"))
                { 
                    res++;
                    Collection.Items.Add(dirName + $"_{i+1}" + ".tp");
                }
                else
                    return
                        res;
            }

            return res;
        }
        public Task task { get; set; }
        private void MakeTP_Click(object sender, EventArgs e)
        {
            if (TpAll.Checked)
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
                        file.WriteLine("Robot=\\C\\Users\\02Robot\\Documents\\My Workcells\\Fanuc_002\\Robot_1");
                        file.WriteLine("Version=V7.70-1");
                        file.WriteLine(@"Path=C:\Program Files (x86)\FANUC\WinOLPC\Versions\V770-1\bin");
                        file.WriteLine(@"Support=C:\Users\02Robot\Documents\My Workcells\Fanuc_002\Robot_1\support");
                        file.WriteLine(@"Output=C:\Users\02Robot\Documents\My Workcells\Fanuc_002\Robot_1\output");
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
            else
            {
                using (var client = new RequestSocket())
                {
                    client.Connect($"tcp://localhost:5001");
                    client.SendFrame($"path${PrPatch}");
                    var msg = client.ReceiveFrameString();
                    if(msg != "1")
                        _isTranslating=true;
                }
            }

        }
        
        private void CheckTpTranslate()
        {
            using (var client = new RequestSocket())
            {
                client.Connect($"tcp://localhost:5001");
                client.SendFrame($"path${PrPatch}");
                var msg = client.ReceiveFrameString();
                while (true)
                {
                    
                }
            }
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            if (!robot.isPrinting)
            { // CurFN.Text = "Текущий файл "+Collection.Items[0];
                
                var gg = Collection.Items.IndexOf(Collection.Text);
                Await_layer.Enabled = false;
                task = new Task(() => robot.PrintAsync(Await_layer.Checked, Collection.Items[gg].ToString(), gg));
                task.Start();
                myTimer.Start();
            }
            else 
            {
                //var a = Form1.sets.Params.Where(c => c.ParamName == "NextLayerBD").ToList();
                //if (a.Count > 0)
                //{
                //    timerButtonlenght = Convert.ToInt32(a[0].ParamValue);
                //}
                //if (TimerWorking)
                //{
                    //TimerWorking = false;
                    // PrintTimer.Stop();
                   // timerCount = 0;
                  // StartPrint.Text = $"Следующий файл";
                   // Collection.Enabled = true;
                    //Skip.Enabled = true;
                    //Repeat.Enabled = true;

                //}
                //else
                {
                    //TimerWorking = true;
                    //StartPrint.Text = $"Следующий файл {timerButtonlenght}";
                    Collection.Enabled = false;
    
                    //PrintTimer.Tick += new EventHandler(NextBEventProcessor);
                    //PrintTimer.Interval = 1000;
                    //PrintTimer.Start();
                }
                
                var gg = Collection.Items.IndexOf(Collection.Text);
                var task = new Task(() => robot.printNext(Collection.Items[gg].ToString(), gg));
                task.Start();
                myTimer.Start();
            }
        }

        

        private void Drop_Click(object sender, EventArgs e)
        {
            myTimer.Stop();
            Await_layer.Enabled = true;
            cts.Cancel();
            StartPrint.Enabled = false;
            Collection.Enabled = true;
            textBox1.Text = "";
            Print.Enabled = true;
            textBox2.Text = "";
            //StartPrint.Enabled = true;
            StartPrint.Text = $"Следующий файл";
            //robot.ChechTest();
            _isTranslating = false;
        }
        /// <summary>
        /// next
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (Collection.SelectedItem != null)
                if (Collection.SelectedIndex < Collection.Items.Count-1)
                {
                    Collection.SelectedIndex++;
                }
        }

        private void Prev_Click(object sender, EventArgs e)
        {
            if (Collection.SelectedItem != null)
                if (Collection.SelectedIndex >0)
                {
                    Collection.SelectedIndex--;
                }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Collection.SelectedItem == null)
                MessageBox.Show("Файл не был выбран");
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
