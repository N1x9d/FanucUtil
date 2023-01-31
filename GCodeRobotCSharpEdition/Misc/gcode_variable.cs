using GCodeRobotCSharpEdition.Misc;
using System.Collections.Generic;

namespace GCodeRobotCSharpEdition
{
    struct gcode_variable
    {
        public List<Param> parametrs;
        public int feedrate;
        public string command;
        public int commandvalue;
    }
}