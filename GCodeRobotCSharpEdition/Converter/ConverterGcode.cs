using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace GCodeRobotCSharpEdition
{
    public class ConverterGcode
    {
        private Form1 _form;
        private coordinates _current, _previous;
        private gcode_variable _gcode;
        private int _pointcount;
        private List<string> header;
        private List<string> footer;
        private bool _closed;
        private int _filepart;
        private bool _movement;
        private int _arcenabled;
        private coords coord = new coords();
        private positioner_variable positioner;

        public List<point> LayerPoints = new List<point>();

        public ConverterGcode(Form1 form)
        {
            this._form = form;
            header = new List<string>();
            footer = new List<string>();
        }

        // не работает
        void coord_rotate(float angle_x, float angle_y, float angle_z)
        {
            Vector3 p = new Vector3(coord.x, coord.y, coord.z);
            Vector3 a = new Vector3((float) 0.0, (float) 0.0, (float) -150.0);
            Matrix4X4 m = new Matrix4X4();

            p -= a;
            m.Rotate(angle_z, new Vector3(0, 0, 1));
            m.Rotate(-angle_x, new Vector3(1, 0, 0));
            m.Rotate(-angle_y, new Vector3(0, 1, 0));
            p = TransformNormal(p,m);
            p += a;

            coord.x = (float) p.X;
            coord.y = (float) p.Y;
            coord.z = (float) p.Z;

            coord.w += angle_x;
            coord.p += angle_y;
            coord.r += -angle_z;
        }
        public Vector3 TransformNormal(Vector3 normal, Matrix4X4 matrix)
        {
            return new Vector3
            {
                X = normal.X * matrix[1,1] + normal.Y * matrix[2,1] + normal.Z * matrix[3,1],
                Y = normal.X * matrix[1,2] + normal.Y * matrix[2,2] + normal.Z * matrix[3,2],
                Z = normal.X * matrix[1,3]+ normal.Y * matrix[2,3] + normal.Z * matrix[3,3]
            };
        }
        //НЕ РАБОТАЕТ
        void tool_rotate()
        {
            //LineF line;
           // line.setLine(_previous.x, _previous.y, _current.x, _current.y);
            //float angle = line.angle();
            //positioner.j2 = -(angle - 180);
        }

        //загрузка файла
        public void on_btn_Open_clicked()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "GCode (*.gcode *.gc *.nc) |*.gcode; *.gc' *.nc| Other files (*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            _form.InputFileInfo = openFile.FileName;
        }

        //сохранение
        void on_btn_Select_clicked()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "LS file(*.LS)| *LS";
            if (saveFile.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFile.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, "");
        }

        private string conv;

        public string Conv
        {
            get { return conv.ToString(myCIintl); }
            set
            {
                conv = value;
                if (conv.Contains(","))
                    conv = conv.Replace(",", ".");
                if (!conv.Contains("."))
                    conv += ".0";
            }
        }

        CultureInfo myCIintl = new CultureInfo("es-ES", true);
        bool Prevstart = false;
        bool PrevEnd = false; 
        coords prevP = new coords();
            coords curP = new coords();
        void robot_add_move_linear()
        {
            string termination = "";

           
            
            if (_current.states.Contains("p")) termination = _form.Tn;
            if (_current.states.Contains("P")) termination = _form.Tw;

            if (!_form.noArc)
            {
                if (_form.AutoArc)
                {
                    if (_current.states.Contains("p") && _previous.states.Contains("P"))
                    {
                        header.Add(": Arc End[1];");
                        if (_form.GetRO)
                            header.Add(": RO[1]=OFF;");
                        if (_form.GetWave)
                            header.Add($": Weave End[{_form.WaveIndex}];");
                        PrevEnd = true;
                    }

                    if (_current.states.Contains("P") && _previous.states.Contains("p"))
                    {
                        header.Add(": Arc Start[1];");
                        if (_form.GetRO)
                            header.Add(": RO[1]=ON;");
                        if (_form.GetWave)
                            header.Add($": Weave Sine[{_form.WaveIndex}];");
                        Prevstart = true;
                    }
                }

                if (_arcenabled == 1)
                {
                    header.Add(": Arc Start[1];");
                    if (_form.GetWave)
                        header.Add(": RO[1]=ON;");
                    if (_form.GetWave)
                        header.Add(": Weave Sine[2];");
                    _arcenabled = 0;
                    Prevstart = true;
                }

                if (_arcenabled == 2)
                {
                    header.Add(": Arc End[1];");
                    if (_form.GetWave)
                        header.Add(": RO[1]=OFF;");
                    if (_form.GetWave)
                        header.Add(": Weave End[2];");

                    _arcenabled = 0;
                    PrevEnd = true;
                }
            }


            _pointcount++;
            if (_movement)
            {
                string line = _pointcount + ": L P[" + _pointcount + "] ";


                float value;
                value = _current.feedrate / 10 * (float) Convert.ToDouble(_form.Es);
                int feed = (int) Math.Round(value);

                if (_form.WeldSpeed && (_current.states.Contains("A") || _current.states.Contains("P")))
                {
                    line += "WELD_SPEED ";
                }
                else
                {
                    line += feed;
                    line += _form.unit;
                    line += " ";
                }

                line += termination;
                line += " ";
                line += _form.econf;
                line += " ;";
                header.Add(line);

                line = "";
                footer.Add("P[" + _pointcount + "] {");
                footer.Add("   GP1:");
                line = "       UF : " + _form.Uf + ", UT : " + _form.Ut +
                       ",     CONFIG: 'N U T, 0, 0, 0',";
                footer.Add(line);

                coord.x = _current.x;
                coord.y = _current.y;
                coord.z = _current.z;
                coord.w = _current.a;
                coord.p = _current.b;
                coord.r = _current.c;
                prevP = curP;
                curP = coord;
                if (PrevEnd && Prevstart)
                {
                    var a = checkStartsStops(curP, prevP);
                    if (a)
                    {
                        header[_pointcount - 2] = "";
                        header[_pointcount - 3] = "";
                    }
                }

                PrevEnd = false;
                Prevstart = false;
                tool_rotate();

                coord_rotate(positioner.j1, 0, positioner.j2);


                value = float.Parse(_form.X, CultureInfo.InvariantCulture.NumberFormat) + coord.x;
                Conv = Math.Round(value, 1).ToString(myCIintl);
                line = "      X = " + Conv + " mm, ";
                value = float.Parse(_form.Y, CultureInfo.InvariantCulture.NumberFormat) + coord.y;
                Conv = Math.Round(value, 1).ToString(myCIintl);
                line += "Y = " + Conv + " mm, ";
                value = float.Parse(_form.Z, CultureInfo.InvariantCulture.NumberFormat) + coord.z;
                Conv = Math.Round(value, 1).ToString(myCIintl);
                line += "Z = " + Conv + " mm,";
                footer.Add(line);
                value = float.Parse(_form.W, CultureInfo.InvariantCulture.NumberFormat) + coord.w;
                Conv = Math.Round(value, 1).ToString(myCIintl);
                line = "      W = " + Conv + " deg, ";
                value = float.Parse(_form.P, CultureInfo.InvariantCulture.NumberFormat) + coord.p;
                Conv = Math.Round(value, 1).ToString(myCIintl);
                line += "P = " + Conv + " deg, ";
                value = float.Parse(_form.R, CultureInfo.InvariantCulture.NumberFormat) + coord.r;
                Conv = Math.Round(value, 1).ToString(myCIintl);
                line += "R = " + Conv + " deg";
                footer.Add(line);
                footer.Add("   GP2:");
                footer.Add("       UF : 1, UT : 2,");
                value = positioner.j1;
                Conv = Math.Round(value, 1).ToString(myCIintl);
                if (!Conv.Contains(".")) Conv += ".0";
                line = "      J1 = " + Conv + " deg, ";

                value = positioner.j2;
                Conv = Math.Round(value, 1).ToString(myCIintl);
                if (!Conv.Contains(".")) Conv += ".0";
                line += "J2 = " + Conv + " deg ";

                footer.Add(line);
                footer.Add("};");
            }
        }

        private bool checkStartsStops(coords p1, coords p2)
        {
            if (_form.Chechs)
            {
                var dist = Math.Pow((p1.x - p2.x), 2);
                dist+= Math.Pow((p1.y - p2.y), 2);
                dist+= Math.Pow((p1.z - p2.z), 2);
                dist = Math.Sqrt(dist);
                if (dist <= float.Parse(_form.CheckDist, myCIintl))
                    return true;
            }

            return false;
        }

        void robot_flush_to_file()
        {
            string outname = _form.FName + "_" + _filepart;
            if (!System.IO.Directory.Exists(_form.OutputFile))
            {
                Directory.CreateDirectory(_form.OutputFile);
            }

            StreamWriter sw = new StreamWriter(_form.OutputFile + outname + ".ls");


            List<string> starter = new List<string>();

            string line = "/PROG " + _form.FName;
            if (_filepart > 0) line += "_" + _filepart;
            starter.Add(line);
            starter.Add("/ATTR");
            starter.Add("OWNER       = MNEDITOR;");
            starter.Add("CREATE      = DATE 100-11-20  TIME 09:43:21;");
            starter.Add("MODIFIED    = DATE 100-12-05  TIME 05:26:29;");
            line = "LINE_COUNT = " + _pointcount + ";";
            starter.Add(line);
            starter.Add("PROTECT     = READ_WRITE;");
            starter.Add("TCD:  STACK_SIZE    = 0,");
            starter.Add("      TASK_PRIORITY = 50,");
            starter.Add("      TIME_SLICE    = 0,");
            starter.Add("      BUSY_LAMP_OFF = 0,");
            starter.Add("      ABORT_REQUEST = 0,");
            starter.Add("      PAUSE_REQUEST = 0;");
            starter.Add("DEFAULT_GROUP   = 1,1,*,*,*;");
            starter.Add("CONTROL_CODE    = 00000000 00000000;");
            starter.Add("/MN");

            for (int i = 0; i < starter.Count; ++i)
                sw.WriteLine(starter[i]);
            for (int i = 0; i < header.Count; ++i)
                sw.WriteLine(header[i]);
            if (!_closed)
            {
                var numb = _filepart+1;
              //  line = ": CALL " + _form.FName + "_" + numb + ";";
                //sw.WriteLine(line);
            }

            sw.WriteLine("/POS");
            for (int i = 0; i < footer.Count; ++i)
                sw.WriteLine(footer[i]);
            sw.WriteLine("/END");

            sw.Close();

            header.Clear();
            footer.Clear();

            _filepart++;
        }

        //обработка gcode
        void gcode_process(string line)
        {
            string lin = line;
            string[] a = lin.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            _gcode.command = "";
            _gcode.flags = "";
            foreach (var com in a)
            {
                char element = com[0];
                if (element == ';')
                    break;

                float value = float.Parse(com.Substring(1), CultureInfo.InvariantCulture.NumberFormat);
                switch (element)
                {
                    case 'X':
                        _gcode.x = value;
                        _gcode.flags += 'x';
                        break;
                    case 'Y':
                        _gcode.y = value;
                        _gcode.flags += 'y';
                        break;
                    case 'Z':
                        _gcode.z = value;
                        _gcode.flags += 'z';
                        break;
                    case 'E':
                        _gcode.e = value;
                        _gcode.flags += 'e';
                        break;
                    case 'A':
                        _gcode.a = value;
                        _gcode.flags += 'a';
                        break;
                    case 'B':
                        _gcode.b = value;
                        _gcode.flags += 'b';
                        break;
                    case 'C':
                        _gcode.c = value;
                        _gcode.flags += 'c';
                        break;
                    case 'F':
                        _gcode.feedrate = Convert.ToInt32(value);
                        _gcode.flags += 'f';
                        break;
                    case 'G':
                        _gcode.command = element.ToString();
                        _gcode.commandvalue = Convert.ToInt32(value);
                        break;
                    case 'M':
                        _gcode.command = element.ToString();
                        _gcode.commandvalue = Convert.ToInt32(value);
                        break;
                }
            }

            if (_gcode.command == "M" && (_gcode.commandvalue == 7))
            {
                _arcenabled = 1;
            } //доп охлаждение

            if (_gcode.command == "M" && (_gcode.commandvalue == 8))
            {
                _arcenabled = 2;
            } //осн охлаждение

            if (_gcode.command == "G" && (_gcode.commandvalue == 92))
            {
                // Linear move
                _previous = _current;

                if (_gcode.flags.Contains("x"))
                {
                    _current.x = _gcode.x;
                }

                if (_gcode.flags.Contains("y"))
                {
                    _current.y = _gcode.y;
                }

                if (_gcode.flags.Contains("z"))
                {
                    _current.z = _gcode.z;
                }

                //if (_gcode.flags.Contains("e"))
                //{
                //    _current.e = _gcode.e;
                //}

                if (_gcode.flags.Contains("a"))
                {
                    _current.a = _gcode.a;
                }

                if (_gcode.flags.Contains("b"))
                {
                    _current.b = _gcode.b;
                }

                if (_gcode.flags.Contains("c"))
                {
                    _current.c = _gcode.c;
                }

                if (_gcode.flags.Contains("f"))
                {
                    _current.feedrate = _gcode.feedrate;
                }
            }

            if (_gcode.command == "M" && (_gcode.commandvalue == 800))
            {
                //Positioner move
                if (_gcode.flags.Contains("a"))
                {
                    positioner.j1 = _gcode.a;
                }

                if (_gcode.flags.Contains("b"))
                {
                    positioner.j2 = _gcode.b;
                }
            }

            if (_gcode.command == "G" && (_gcode.commandvalue == 1 || _gcode.commandvalue == 0))
            {
                // Linear move
                _previous = _current;

                _movement = false;
                if (_gcode.flags.Contains("x"))
                {
                    _current.x = _gcode.x;
                    _movement = true;
                }

                if (_gcode.flags.Contains("y"))
                {
                    _current.y = _gcode.y;
                    _movement = true;
                }

                if (_gcode.flags.Contains("z"))
                {
                    _current.z = _gcode.z;
                    _movement = true;
                }

                if (_gcode.flags.Contains("e"))
                {
                    _current.e = _gcode.e;
                }

                if (_gcode.flags.Contains("a"))
                {
                    _current.a = _gcode.a;
                    _movement = true;
                }

                if (_gcode.flags.Contains("b"))
                {
                    _current.b = _gcode.b;
                    _movement = true;
                }

                if (_gcode.flags.Contains("c"))
                {
                    _current.c = _gcode.c;
                    _movement = true;
                }

                if (_gcode.flags.Contains("f"))
                {
                    _current.feedrate = _gcode.feedrate;
                }

                if (_form.AutoArc)
                {
                    if (_current.e - _previous.e > 0) _current.states = "P";
                    else _current.states = "p";
                }
                point c;
                c.coord = _current;
                c.movment = _movement;
                c.positioner = positioner;
                LayerPoints.Add(c);
                robot_add_move_linear();
            }
        }

        public void on_btn_Process_clicked()
        {
            //QRegularExpression re("(([A-Z])[-]\?\\d+\\.*\\d*)");

            _current.a = 0;
            _current.b = 0;
            _current.c = 0;
            _current.x = 0;
            _current.y = 0;
            _current.z = 0;
            _current.e = 0;
            _current.feedrate = 0;
            _current.states = "p";
            point c;
            c.movment = false;
            c.coord = _current;
            
            positioner.j1 = 0;
            positioner.j2 = 0;
            c.positioner = positioner;
            LayerPoints.Add(c);
            _previous = _current;
            header.Clear();
            footer.Clear();
            _pointcount = 0;

            string inputFile = _form.Input;

            //if (inputFile.open(QIODevice::ReadOnly))
            //{
            _closed = false;
            _filepart = 0;

            StreamReader sr = new StreamReader(inputFile);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                //if (line.Contains("; layer "))
                //{
                //    if (_form.toolList.Count > 0)
                //    {
                //        var a = line.Split(' ');
                //        try
                //        {
                //            int layerind = int.Parse(a[2].Substring(0, a[2].Length - 1));
                //            foreach( var tool in _form.toolList)
                //            {
                //                if (tool.Layer % layerind == 0)
                //                {
                //                    var lsCode = tool.DuplicateLayerForThisTool(LayerPoints,_form,_pointcount);
                //                    LayerPoints.Clear();
                //                    _pointcount = lsCode.pointcount;
                //                    footer.AddRange(lsCode.footer);
                //                    header.AddRange(lsCode.header);
                //                }
                //               }
                //        }
                //        catch (Exception)
                //        {
                           
                //        }
                        
                //    }
                //    if (_form.CheckLayer)
                //    {
                //        robot_flush_to_file();
                //        _pointcount = 0;
                //    }
                //}


                gcode_process(line);

                if (_pointcount >= Convert.ToInt32(_form.esplit) && !_form.CheckLayer)
                {
                    robot_flush_to_file();
                    _pointcount = 0;
                }
            }

            sr.Close();
            robot_flush_to_file();
            _closed = true;
            string outDir = _form.outFile.Substring(0, _form.outFile.LastIndexOf('\\'));
            outDir = outDir.Substring(0, outDir.LastIndexOf('\\') + 1) + "layer";
            if (_form.CheckLayer)
            {
                ProcessStartInfo psipy = new ProcessStartInfo();
                psipy.CreateNoWindow = false;
                psipy.WindowStyle = ProcessWindowStyle.Normal;
                string cmdString = @$"/k ""python Scrypts\Slicer.py {_form.outFile} {outDir}""";

                //cmdString += outDir;
                if (_form.LaserPass)
                    cmdString += " d";
                Process Slice = new Process();
                psipy.FileName = "cmd";
                psipy.Arguments = cmdString;
                Slice.StartInfo = psipy;
                Slice.Start();
            }
            MessageBox.Show("done");
            Process PrFolder = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            string file =_form.outFile;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.FileName = "explorer";
            psi.Arguments = @"/n, /select, " + file;
            PrFolder.StartInfo = psi;
            PrFolder.Start();
            robot_flush_to_file();
            
        }

        //void MainWindow::on_pushButton_clicked()
        //{
        //    QNetworkAccessManager* manager = new QNetworkAccessManager(this);
        //    connect(manager, SIGNAL(finished(QNetworkReply *)),
        //            this, SLOT(onResult(QNetworkReply *)));

        //    manager->get(QNetworkRequest(QUrl("http://127.0.0.2/MD/PRGSTATE.DG")));
        //}

        //void onResult(QNetworkReply reply)
        //{
        //    if (reply->error() != QNetworkReply::NoError)
        //        return;  // ...only in a blog post


        //    QStringList data = QStringList((QString)reply->readAll());

        //    ui->label->setText(data.first());
        //    //QScriptEngine engine;
        //    //QScriptValue result = engine.evaluate(data);
        //}
    }
}