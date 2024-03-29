﻿using GCodeRobotCSharpEdition.Converter;
using GCodeRobotCSharpEdition.Converter.GCodeRobotCSharpEdition;
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
        private InputParametrs _form;
        private readonly OutputFileOptions output;

        public Fanuc(InputParametrs form, OutputFileOptions output)
        {
            header = new List<string>();
            footer = new List<string>();
            _form = form;
            this.output = output;
        }

        private int _pointcount = 0;
        private List<string> header;
        private List<string> footer;
        private bool _closed;
        private int _filepart;
        private bool _movement;
        private int _arcenabled;
        private Coords coord = new Coords();
        private positioner_variable positioner;
        bool PrevStart = false;
        bool PrevEnd = false;
        Coords prevP = new Coords();
        Coords curP = new Coords();
        float prevX, prevY = 0;

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

        private float getAngle(float x, float y)
        {
            var a = Math.Atan2(y, x);
            a = a * 180 / Math.PI;
            return (float) (a);
        }

        private bool checkStartsStops(Coords p1, Coords p2)
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

        public void GenerateFanucFile(List<point> points)
        {
            point c = new point();

            positioner.j1 = 0;
            positioner.j2 = 0;
            c.positioner = positioner;
            header.Clear();
            footer.Clear();
            //points.Insert(0,c);

            coordinates _current = new coordinates();
            coordinates _previous;
            _current.states = PrintStatesEnum.Move;
            foreach (var point in points)
            {
                if (_pointcount >= Convert.ToInt32(output.Esplit) && !output.CheckLayer)
                {
                    header.Add(": Arc End[1];");
                    WriteFile();
                    _pointcount = 0;
                }

                _previous = _current;
                _current = point.coord;
                string termination = "";


                if (_current.states == PrintStatesEnum.Move) termination = _form.nm;
                if (_current.states == PrintStatesEnum.Printing) termination = _form.wm;

                if (!_form.Arc_disable)
                {
                    if (_form.Auto_arc)
                    {
                        if (_current.states == PrintStatesEnum.Move && _previous.states == PrintStatesEnum.Printing)
                        {
                            header.Add(": Arc End[1];");
                            if (_form.RO)
                                header.Add(": RO[1]=OFF;");
                            if (_form.WEE)
                                header.Add($": Weave End[{_form.WE}];");
                            PrevEnd = true;
                        }

                        if (_current.states == PrintStatesEnum.Printing && _previous.states == PrintStatesEnum.Move)
                        {
                            header.Add(": Arc Start[1];");
                            if (_form.RO)
                                header.Add(": RO[1]=ON;");
                            if (_form.WEE)
                                header.Add($": Weave Sine[{_form.WE}];");
                            PrevStart = true;
                        }
                    }

                    if (_arcenabled == 1)
                    {
                        header.Add(": Arc Start[1];");
                        if (_form.WEE)
                            header.Add(": RO[1]=ON;");
                        if (_form.WEE)
                            header.Add(": Weave Sine[2];");
                        _arcenabled = 0;
                        PrevStart = true;
                    }

                    if (_arcenabled == 2)
                    {
                        header.Add(": Arc End[1];");
                        if (_form.WEE)
                            header.Add(": RO[1]=OFF;");
                        if (_form.WEE)
                            header.Add(": Weave End[2];");

                        _arcenabled = 0;
                        PrevEnd = true;
                    }
                }

                float y = 180f + (getAngle(coord.r, coord.p));
                float x = (getAngle(coord.w, coord.r) + 90);
                if (y > 180)
                    y -= 360;
                else if (y < -180)
                    y += 360;
                if (x > 180)
                    x -= 360;
                else if (x < -180)
                    x += 360;
                bool _isNeed = false;
                if ((Math.Abs(prevY - y) > int.Parse(_form.WS) || Math.Abs(prevX - x) > int.Parse(_form.WS)) &&
                    _form.WSE)
                {
                    header.Add(": Arc start [2];");
                    _isNeed = true;
                }
                else if (_isNeed && _form.WSE)
                {
                    header.Add(": Arc Start[1];");
                    if (_form.WEE)
                        header.Add(": RO[1]=ON;");
                    if (_form.WEE)
                        header.Add($": Weave Sine[{_form.WE}];");
                    _arcenabled = 0;
                    PrevStart = true;
                }

                _pointcount++;

                if (point.movment)
                {
                    string line = _pointcount + ": L P[" + _pointcount + "] ";


                    float value;
                    value = _current.feedrate / 10 * (float) Convert.ToDouble("1");
                    int feed = (int) Math.Round(value);

                    if (_form.WELD_SPEED && ( /*_current.states.Contains("A") ||*/
                            _current.states == PrintStatesEnum.Printing))
                    {
                        line += "WELD_SPEED ";
                    }
                    else
                    {
                        line += feed;
                        line += "cm/min";
                        line += " ";
                    }

                    line += termination;
                    line += " ;";
                    header.Add(line);

                    line = "";
                    footer.Add("P[" + _pointcount + "] {");
                    footer.Add("   GP1:");
                    line = "       UF : " + _form.RUF + ", UT : " + _form.RUT +
                           $",     CONFIG: '{_form.W} {_form.A} {_form.B}, {_form.j1}, {_form.j4}, {_form.j6}',";
                    footer.Add(line);

                    coord.x = _current.x;
                    coord.y = _current.y;
                    coord.z = _current.z;
                    coord.w = _current.a;
                    coord.p = _current.b;
                    coord.r = _current.c;
                    prevP = curP;
                    curP = coord;
                    if (PrevEnd && PrevStart)
                    {
                        var a = checkStartsStops(curP, prevP);
                        if (a)
                        {
                            header[_pointcount - 2] = "";
                            header[_pointcount - 3] = "";
                        }
                    }

                    PrevEnd = false;
                    PrevStart = false;
                    tool_rotate();

                    coord_rotate(positioner.j1, 0, positioner.j2);


                    value = float.Parse(_form.x, CultureInfo.InvariantCulture.NumberFormat) + coord.x;
                    Conv = Math.Round(value, 1).ToString(myCIintl);
                    line = "      X = " + Conv + " mm, ";
                    value = float.Parse(_form.y, CultureInfo.InvariantCulture.NumberFormat) + coord.y;
                    Conv = Math.Round(value, 1).ToString(myCIintl);
                    line += "Y = " + Conv + " mm, ";
                    value = float.Parse(_form.z, CultureInfo.InvariantCulture.NumberFormat) + coord.z;
                    Conv = Math.Round(value, 1).ToString(myCIintl);
                    line += "Z = " + Conv + " mm,";
                    footer.Add(line);
                    value = float.Parse(_form.w, CultureInfo.InvariantCulture.NumberFormat) + coord.w;
                    Conv = Math.Round(value, 1).ToString(myCIintl);
                    line = "      W = " + Conv + " deg, ";
                    value = float.Parse(_form.p, CultureInfo.InvariantCulture.NumberFormat) + coord.p;
                    Conv = Math.Round(value, 1).ToString(myCIintl);
                    line += "P = " + Conv + " deg, ";
                    value = float.Parse(_form.r, CultureInfo.InvariantCulture.NumberFormat) + coord.r;
                    Conv = Math.Round(value, 1).ToString(myCIintl);
                    line += "R = " + Conv + " deg";
                    footer.Add(line);
                    footer.Add("   GP2:");
                    footer.Add($"       UF : {_form.PUF}, UT : {_form.PUT},");
                    value = positioner.j1 + int.Parse(_form.j1o);
                    Conv = Math.Round(value, 1).ToString(myCIintl);
                    if (!Conv.Contains(".")) Conv += ".0";
                    line = "      J1 = " + Conv + " deg, ";

                    value = positioner.j2 + int.Parse(_form.j1o);
                    Conv = Math.Round(value, 1).ToString(myCIintl);
                    if (!Conv.Contains(".")) Conv += ".0";
                    line += "J2 = " + Conv + " deg ";

                    footer.Add(line);
                    footer.Add("};");
                }
            }

            WriteFile(true);
            string outDir = Path.GetDirectoryName(output.OutputFile);
            if (output.CheckLayer)
            {
                ProcessStartInfo psipy = new ProcessStartInfo();
                psipy.CreateNoWindow = false;
                psipy.WindowStyle = ProcessWindowStyle.Normal;
                string cmdString = @$"/k ""python Scrypts\Slicer.py {output.OutputFile} {outDir}""";

                //cmdString += outDir;
                if (output.LaserPass)
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
            string file = output.OutputFile;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.FileName = "explorer";
            psi.Arguments = @"/n, /select, " + file;
            PrFolder.StartInfo = psi;
            PrFolder.Start();
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
            p = TransformNormal(p, m);
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


        void WriteFile(bool offArcInEnd = false)
        {
            string outname = output.FName + "_" + _filepart;
            if (!Directory.Exists(Path.GetDirectoryName(output.OutputFile)))
            {
                var f = Path.GetDirectoryName(output.OutputFile);
                Directory.CreateDirectory(f);
            }

            StreamWriter sw = new StreamWriter(Path.Combine(output.OutputFile, outname + ".ls"));

            if (offArcInEnd)
            {
                header.Add(": Arc End[1];");
            }

            List<string> starter = new List<string>();

            string line = "/PROG " + output.FName;
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