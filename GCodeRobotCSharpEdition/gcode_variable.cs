﻿namespace GCodeRobotCSharpEdition
{
    struct gcode_variable
    {
        public float x, y, z, e, a, b, c;
        public int feedrate;
        public string command;
        public int commandvalue;
        public string flags;
    }
}