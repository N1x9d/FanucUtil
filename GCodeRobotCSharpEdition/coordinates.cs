using GCodeRobotCSharpEdition.Misc;

namespace GCodeRobotCSharpEdition
{
    public struct coordinates
    {
        public float x, y, z, e, a, b, c;
        public int feedrate;
        public PrintStatesEnum states;
    }
    public struct point
    {
        public bool movment;
        public coordinates coord;
        public positioner_variable positioner;
    }
}