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

        private Fanuc fanuc;

        public List<point> LayerPoints = new List<point>();

        public ConverterGcode(Form1 form)
        {
            this._form = form;
            header = new List<string>();
            footer = new List<string>();
            fanuc = new Fanuc(_form);
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
        
        //обработка gcode
        void gcode_process(Group[] line)
        {
            _gcode.command = line[0].Value;
            _gcode.commandvalue = int.Parse(line[1].Value);
            _gcode.parametrs = SplitParams(line);

            if (_gcode.command == "M" && (_gcode.commandvalue == 7))
            {
                _arcenabled = 1;
            } //доп охлаждение

            if (_gcode.command == "M" && (_gcode.commandvalue == 8))
            {
                _arcenabled = 2;
            } //осн охлаждение

            if (_gcode.command == "M" && (_gcode.commandvalue == 800))
            {
                foreach (var param in _gcode.parametrs)
                {
                    switch (param.Type)
                    {
                        case "A":
                            positioner.j1 = param.Value;
                            break;
                        case "B":
                            positioner.j2 = param.Value;
                            break;
                    }
                }
            }

            if (_gcode.command == "G")
            {
                // Linear move
                _previous = _current;
                _movement = false;

                foreach (var param in _gcode.parametrs)
                {
                    switch (param.Type)
                    {

                        case "X":
                            _current.x = param.Value;
                            _movement = true;
                            break;
                        case "Y":
                            _current.y = param.Value;
                            _movement = true;
                            break;
                        case "Z":
                            _current.z = param.Value;
                            _movement = true;
                            break;
                        case "A":
                            _current.a = param.Value;
                            _movement = true;
                            break;
                        case "B":
                            _current.b = param.Value;
                            _movement = true;
                            break;
                        case "C":
                            _current.c = param.Value;
                            _movement = true;
                            break;
                        case "F":
                            _current.feedrate = (int)param.Value;
                            break;
                        case "E":
                            _current.e = param.Value;
                            break;

                    }
                }           
            

                 if (_gcode.commandvalue == 1 || _gcode.commandvalue == 0)
                 {
                    // Linear move

                    if (_form.AutoArc)
                    {
                        if (_current.e - _previous.e > 0) _current.states = PrintStatesEnum.Printing;
                        else _current.states = PrintStatesEnum.Move;
                    }
                    point c;
                    c.coord = _current;
                    c.movment = _movement;
                    c.positioner = positioner;
                    LayerPoints.Add(c);
                 }
            }

        }

        private List<Param> SplitParams(Group[] line) 
        {
            var m1 = Regex.Matches(line[2].Value,@"((?!\d)\w+?)('.*'|(\d+\.?)+|-?\d*\.?\d*)");
            var Params = new List<Param>();
            foreach (Match match in m1)
            {
                var gr = match.Groups;
                var grops = gr.Values.Where(c => c.Name != "0").ToArray();
                var type = grops[0].Value.ToUpper();
                var val = grops[1].Value;
                Params.Add(new Param(type, float.Parse(val, CultureInfo.InvariantCulture.NumberFormat)));
            }
            return Params;
        }

        public void on_btn_Process_clicked()
        {
            string inputFile = _form.Input;

            StreamReader sr = new StreamReader(inputFile);
            var m1 = Regex.Matches(sr.ReadToEnd(), @"(?!; *.+)(G|M|T|g|m|t)(\d+)(([ \t]*(?!G|M|g|m)\w('.*'|([-\d\.]*)))*)[ \t]*(;[ \t]*(.*))?|;[ \t]*(.+)");
            sr.Close();
            var gLines = new List<Group[]>();
            foreach (Match match in m1)
            {
                if (match.Value[0] == ';')
                {
                    continue;
                }
                var gr = match.Groups;
                var grops = gr.Values.Where(c => c.Name != "0").ToArray();
                gLines.Add(grops);
                gcode_process(grops);
            }
            fanuc.GenerateFanucFile(LayerPoints);
            
        }
    }
}