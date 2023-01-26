using GCodeRobotCSharpEdition.Converter;
using GCodeRobotCSharpEdition.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace GCodeRobotCSharpEdition.Converter
{
    public class Fanuc
    {

        private Form1 _form;

        public Fanuc(Form1 form)
        {
            header = new List<string>();
            footer = new List<string>();
            _form = form;
        }

        private int _pointcount = 0;
        private List<string> header;
        private List<string> footer;
        private bool _closed;
        private int _filepart;
        private bool _movement;
        private int _arcenabled;
        private coords coord = new coords();
        private positioner_variable positioner;
        bool Prevstart = false;
        bool PrevEnd = false;
        coords prevP = new coords();
        coords curP = new coords();

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


        public void GenerateFanucFile(List<point> points)
        {
            point c = new point();

            positioner.j1 = 0;
            positioner.j2 = 0;
            c.positioner = positioner;
            header.Clear();
            footer.Clear();
            points.Insert(0,c);

            coordinates _current = new coordinates();
            coordinates _previous;
            _current.states = PrintStatesEnum.Move;
            foreach (var point in points)
            {
                if (_pointcount >= Convert.ToInt32(_form.esplit) && !_form.CheckLayer)
                {
                    header.Add(": Arc End[1];");
                    WriteFile();
                    _pointcount = 0;
                }

                _previous = _current;
                _current = point.coord;
                string termination = "";
                

                if (_current.states == PrintStatesEnum.Move) termination = _form.Tn;
                if (_current.states == PrintStatesEnum.Printing) termination = _form.Tw;

                if (!_form.noArc)
                {
                    if (_form.AutoArc)
                    {
                        if (_current.states == PrintStatesEnum.Move && _previous.states == PrintStatesEnum.Printing)
                        {
                            header.Add(": Arc End[1];");
                            if (_form.GetRO)
                                header.Add(": RO[1]=OFF;");
                            if (_form.GetWave)
                                header.Add($": Weave End[{_form.WaveIndex}];");
                            PrevEnd = true;
                        }

                        if (_current.states == PrintStatesEnum.Printing && _previous.states == PrintStatesEnum.Move)
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
                if (point.movment)
                {
                    string line = _pointcount + ": L P[" + _pointcount + "] ";


                    float value;
                    value = _current.feedrate / 10 * (float)Convert.ToDouble(_form.Es);
                    int feed = (int)Math.Round(value);

                    if (_form.WeldSpeed && (/*_current.states.Contains("A") ||*/ _current.states == PrintStatesEnum.Printing))
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
                _pointcount++;
            }

            WriteFile(true);
            string outDir = _form.OutFile.Substring(0, _form.OutFile.LastIndexOf('\\'));
            outDir = outDir.Substring(0, outDir.LastIndexOf('\\') + 1) + "layer";
            if (_form.CheckLayer)
            {
                ProcessStartInfo psipy = new ProcessStartInfo();
                psipy.CreateNoWindow = false;
                psipy.WindowStyle = ProcessWindowStyle.Normal;
                string cmdString = @$"/k ""python Scrypts\Slicer.py {_form.OutFile} {outDir}""";

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
            string file = _form.OutFile;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.FileName = "explorer";
            psi.Arguments = @"/n, /select, " + file;
            PrFolder.StartInfo = psi;
            PrFolder.Start();
        }

        private bool checkStartsStops(coords p1, coords p2)
        {
            if (_form.Chechs)
            {
                var dist = Math.Pow((p1.x - p2.x), 2);
                dist += Math.Pow((p1.y - p2.y), 2);
                dist += Math.Pow((p1.z - p2.z), 2);
                dist = Math.Sqrt(dist);
                if (dist <= float.Parse(_form.CheckDist, myCIintl))
                    return true;
            }

            return false;
        }

        // не работает
        void coord_rotate(float angle_x, float angle_y, float angle_z)
        {
            Vector3 p = new Vector3(coord.x, coord.y, coord.z);
            Vector3 a = new Vector3((float)0.0, (float)0.0, (float)-150.0);
            Matrix4X4 m = new Matrix4X4();

            p -= a;
            m.Rotate(angle_z, new Vector3(0, 0, 1));
            m.Rotate(-angle_x, new Vector3(1, 0, 0));
            m.Rotate(-angle_y, new Vector3(0, 1, 0));
            p = TransformNormal(p, m);
            p += a;

            coord.x = (float)p.X;
            coord.y = (float)p.Y;
            coord.z = (float)p.Z;

            coord.w += angle_x;
            coord.p += angle_y;
            coord.r += -angle_z;
        }

        public Vector3 TransformNormal(Vector3 normal, Matrix4X4 matrix)
        {
            return new Vector3
            {
                X = normal.X * matrix[1, 1] + normal.Y * matrix[2, 1] + normal.Z * matrix[3, 1],
                Y = normal.X * matrix[1, 2] + normal.Y * matrix[2, 2] + normal.Z * matrix[3, 2],
                Z = normal.X * matrix[1, 3] + normal.Y * matrix[2, 3] + normal.Z * matrix[3, 3]
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


        void WriteFile(bool offArcinEnd = false)
        {
            string outname = _form.FName + "_" + _filepart;
            if (!Directory.Exists(_form.OutputFile))
            {
                Directory.CreateDirectory(_form.OutputFile);
            }

            StreamWriter sw = new StreamWriter(_form.OutputFile + outname + ".ls");

            if (offArcinEnd)
            {
                header.Add(": Arc End[1];");
            }
            
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
                var numb = _filepart + 1;
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

    }

}
