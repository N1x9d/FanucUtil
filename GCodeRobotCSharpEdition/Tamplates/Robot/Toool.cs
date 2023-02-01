//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using System.Globalization;

//namespace GCodeRobotCSharpEdition.Robot
//{
//    public class point3D
//    {
//        float x = 0;
//        float y = 0;
//        float z = 0;


//        public float Z { get => z; set => z = value; }
//        public float Y { get => y; set => y = value; }
//        public float X { get => x; set => x = value; }
//    }

//    public struct ResaltTamplate
//    {
//        public List<string> footer;
//        public List<string> header;
//        public int pointcount;
//    }

//    public class Tool
//    {
//        private string conv;
//        public string Conv
//        {
//            get { return conv.ToString(myCIintl); }
//            set
//            {
//                conv = value;
//                if (conv.Contains(","))
//                    conv = conv.Replace(",", ".");
//                if (!conv.Contains("."))
//                    conv += ".0";
//            }
//        }

//        CultureInfo myCIintl = new CultureInfo("es-ES", true);
//        public bool enable = false;
//        public string ToolType { get; set; }
//        public float Value { get; set; }
//        public float Speed { get; set; }
//        public int Power { get; set; }
//        // прогонять на каждом слое крвтному данному числу инструмент
//        public int Layer { get; set; }
//        // отступ от начальных координат
//        public point3D Offcet { get; set; } = new point3D();
//        public Tool(string tooltype)
//        {
//            ToolType = tooltype;
//            enable = true;
//        }
//        public ResaltTamplate DuplicateLayerForThisTool(List<point> points, Form1 _form, int pointcount)
//        {
//            List<string> header = new List<string>();

//            List<string> footer = new List<string>();
//            header.Add(";Next tool");
//            int _pointcount = pointcount;
//            string termination = "";
//            coordinates _current, _previous;
//            _previous = points[0].coord;
//            for (int i = 1; i < points.Count; i++)
//            {
//                _current = points[i].coord;
//                //if (_current.states.Contains("p")) termination = _form.Tn;
//                //if (_current.states.Contains("P")) termination = _form.Tw;

//                //if (!_form.noArc)
//                //{
//                //if (_form.AutoArc)
//                //{
//                //    if (_current.states.Contains("p") && _previous.states.Contains("P"))
//                //    {
//                //        header.Add(": Arc End[1];");
//                //    }

//                //    if (_current.states.Contains("P") && _previous.states.Contains("p"))
//                //    {
//                //        header.Add(": Arc Start[1];");
//                //    }
//                //}

//                //if (_arcenabled == 1)
//                //{
//                //    header.Add(": Arc Start[1];");
//                //    _arcenabled = 0;
//                //}

//                //if (_arcenabled == 2)
//                //{
//                //    header.Add(": Arc End[1];");
//                //    _arcenabled = 0;
//                //}
//                //}


//                _pointcount++;
//                if (points[i].movment)
//                {
//                    string line = _pointcount + ": L P[" + _pointcount + "] ";


//                    float value;
//                    value = _current.feedrate / 10 * (float)Convert.ToDouble(_form.Es);
//                    int feed = (int)Math.Round(value);

//                    //if (_form.WeldSpeed && (_current.states.Contains("A") || _current.states.Contains("P")))
//                    //{
//                    //    line += "WELD_SPEED ";
//                    //}
//                    //else
//                    //{
//                    //    line += feed;
//                    //    line += _form.unit;
//                    //    line += " ";
//                    //}

//                    line += termination;
//                    line += " ;";
//                    header.Add(line);

//                    line = "";
//                    footer.Add("P[" + _pointcount + "] {");
//                    footer.Add("   GP1:");
//                    line = "       UF : " + _form.Uf + ", UT : " + _form.Ut +
//                           ",     CONFIG: 'N U T, 0, 0, 0',";
//                    footer.Add(line);
//                    coords coord;

//                    coord.x = _current.x;
//                    coord.y = _current.y;
//                    coord.z = _current.z;
//                    coord.w = _current.a;
//                    coord.p = _current.b;
//                    coord.r = _current.c;
//                    //prevP = curP;
//                    //curP = coord;
//                    //if (PrevEnd && Prevstart)
//                    //{
//                    //    var a = checkStartsStops(curP, prevP);
//                    //    if (a)
//                    //    {
//                    //        header[_pointcount - 2] = "";
//                    //        header[_pointcount - 3] = "";
//                    //    }
//                    //}

//                    //PrevEnd = false;
//                    //Prevstart = false;
//                    //tool_rotate();

//                    //coord_rotate(positioner.j1, 0, positioner.j2);


//                    value = float.Parse(_form.X, CultureInfo.InvariantCulture.NumberFormat) + coord.x + Offcet.X;
//                    Conv = Math.Round(value, 1).ToString(myCIintl);
//                    line = "      X = " + Conv + " mm, ";
//                    value = float.Parse(_form.Y, CultureInfo.InvariantCulture.NumberFormat) + coord.y + Offcet.Y;
//                    Conv = Math.Round(value, 1).ToString(myCIintl);
//                    line += "Y = " + Conv + " mm, ";
//                    value = float.Parse(_form.Z, CultureInfo.InvariantCulture.NumberFormat) + coord.z + Offcet.Z;
//                    Conv = Math.Round(value, 1).ToString(myCIintl);
//                    line += "Z = " + Conv + " mm,";
//                    footer.Add(line);
//                    value = float.Parse(_form.W, CultureInfo.InvariantCulture.NumberFormat) + coord.w;
//                    Conv = Math.Round(value, 1).ToString(myCIintl);
//                    line = "      W = " + Conv + " deg, ";
//                    value = float.Parse(_form.P, CultureInfo.InvariantCulture.NumberFormat) + coord.p;
//                    Conv = Math.Round(value, 1).ToString(myCIintl);
//                    line += "P = " + Conv + " deg, ";
//                    value = float.Parse(_form.R, CultureInfo.InvariantCulture.NumberFormat) + coord.r;
//                    Conv = Math.Round(value, 1).ToString(myCIintl);
//                    line += "R = " + Conv + " deg";
//                    footer.Add(line);
//                    footer.Add("   GP2:");
//                    footer.Add("       UF : 1, UT : 2,");
//                    value = points[i].positioner.j1;
//                    Conv = Math.Round(value, 1).ToString(myCIintl);
//                    if (!Conv.Contains(".")) Conv += ".0";
//                    line = "      J1 = " + Conv + " deg, ";

//                    value = points[i].positioner.j2;
//                    Conv = Math.Round(value, 1).ToString(myCIintl);
//                    if (!Conv.Contains(".")) Conv += ".0";
//                    line += "J2 = " + Conv + " deg ";

//                    footer.Add(line);
//                    footer.Add("};");
//                }
//                _previous = _current;
//            }
//            ResaltTamplate f;
//            f.footer = footer;
//            f.header = header;
//            f.pointcount = _pointcount;
//            return f;

//        }
//    }
//}
