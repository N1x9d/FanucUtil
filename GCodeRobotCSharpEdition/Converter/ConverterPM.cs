using GCodeRobotCSharpEdition.Converter;
using GCodeRobotCSharpEdition.Converter.GCodeRobotCSharpEdition;
using GCodeRobotCSharpEdition.Misc;
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
   
    class ConverterPM
    {
        private LSLineInfo lineLSR;
        private InputParametrs _form;
        private coordinates _current, _previous;
        private int _pointcount;
        private bool _movement;
        private int _arcenabled;
        private Coords coord = new Coords();
        private positioner_variable positioner;

        public List<point> LayerPoints = new List<point>();
        private Fanuc fanuc;

        public ConverterPM(InputParametrs form, OutputFileOptions output)
        {
            this._form = form;
            fanuc = new Fanuc(_form, output);
        }

        //загрузка файла
        public void on_btn_Open_clicked()
        {
            
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
       

        
        private bool ferstLine = true;
        private LSLineInfo PrevLine;
        //обработка gcode(lsr from powermill)
        void ProcessLine(string line)
        {
            try
            {
                string lin = line;
                string[] a = lin.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                lineLSR = new LSLineInfo(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8], a[9], a[10], a[11], a[12]);

                if (ferstLine)
                {
                    // Linear move
                    _previous = _current;
                    _movement = false;
                    _current.x = lineLSR.getFloat(lineLSR.StartX);
                    _current.y = lineLSR.getFloat(lineLSR.StartY);
                    _current.z = lineLSR.getFloat(lineLSR.StartZ);
                    _current.a = lineLSR.getFloat(lineLSR.ArcX);
                    _current.b = lineLSR.getFloat(lineLSR.ArcY);
                    _current.c = lineLSR.getFloat(lineLSR.ArcZ);
                    _movement = true;
                    _current.feedrate = (int)Math.Round(lineLSR.getFloat(lineLSR.Speed));

                    if (_form.Auto_arc)
                    {
                        _current.states = PrintStatesEnum.Move;
                    }
                    point c;
                    PrevLine = lineLSR;
                    c.coord = _current;
                    c.movment = _movement;
                    c.positioner = positioner;
                    LayerPoints.Add(c);
                    ferstLine = false;
                }
                else if (lineLSR.comparePrevPoint(PrevLine, _form.Chechs, Convert.ToInt32(_form.CheckDist)))
                {
                    try
                    {
                        _previous = _current;
                        _movement = false;
                        _current.x = lineLSR.getFloat(lineLSR.StartX);
                        _current.y = lineLSR.getFloat(lineLSR.StartY);
                        _current.z = lineLSR.getFloat(lineLSR.StartZ);
                        _current.a = lineLSR.getFloat(lineLSR.ArcX);
                        _current.b = lineLSR.getFloat(lineLSR.ArcY);
                        _current.c = lineLSR.getFloat(lineLSR.ArcZ);
                        _movement = true;
                        _current.feedrate = (int)Math.Round(lineLSR.getFloat(lineLSR.Speed));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }

                    if (_form.Auto_arc)
                    {
                        _current.states = PrintStatesEnum.Printing;
                    }
                    point c;
                    PrevLine = lineLSR;
                    c.coord = _current;
                    c.movment = _movement;
                    c.positioner = positioner;
                    LayerPoints.Add(c);
                }
                else
                {
                    _previous = _current;
                    _movement = false;
                    _current.x = PrevLine.getFloat(PrevLine.EndX);
                    _current.y = PrevLine.getFloat(PrevLine.EndY);
                    _movement = true;
                    _current.z = PrevLine.getFloat(PrevLine.EndZ);
                    _current.a = lineLSR.getFloat(lineLSR.ArcX);
                    _current.b = lineLSR.getFloat(lineLSR.ArcY);
                    _current.c = lineLSR.getFloat(lineLSR.ArcZ);
                    _current.feedrate = (int)Math.Round(lineLSR.getFloat(PrevLine.Speed));

                    if (_form.Auto_arc)
                    {
                        _current.states = PrintStatesEnum.Printing;
                    }
                    point c;
                    c.coord = _current;
                    c.movment = _movement;
                    c.positioner = positioner;
                    LayerPoints.Add(c);

                    _previous = _current;
                    _movement = false;
                    _current.x = lineLSR.getFloat(lineLSR.StartX);
                    _current.y = lineLSR.getFloat(lineLSR.StartY);
                    _current.z = lineLSR.getFloat(lineLSR.StartZ);
                    _current.a = lineLSR.getFloat(lineLSR.ArcX);
                    _current.b = lineLSR.getFloat(lineLSR.ArcY);
                    _current.c = lineLSR.getFloat(lineLSR.ArcZ);
                    _movement = true;
                    _current.feedrate = (int)Math.Round(lineLSR.getFloat(lineLSR.Speed));

                    if (_form.Auto_arc)
                    {
                        _current.states = PrintStatesEnum.Move;
                    }

                    c.coord = _current;
                    c.movment = _movement;
                    c.positioner = positioner;
                    LayerPoints.Add(c);
                    PrevLine = lineLSR;
                   
                }

            }
            catch (Exception e)
            {

                _current.states = PrintStatesEnum.Move;
                ferstLine = true;
                MessageBox.Show(e.ToString());
            }
        }

        public void Generate(string input)
        {            
            using(StreamReader sr = new StreamReader(input))
            {
                while (!sr.EndOfStream)
                {

                    string line = sr.ReadLine();

                    if (!line.Contains("#"))
                    {
                        ProcessLine(line);
                       
                    }
                }
            }
            fanuc.GenerateFanucFile(LayerPoints);
            


        }
    }
}
