namespace GCodeRobotCSharpEdition
{
    public struct coordinates
    {
        public float x, y, z, e, a, b, c;
        public int feedrate;
        public string states;
    }
    public struct point
    {
        public bool movment;
        public coordinates coord;
        public positioner_variable positioner;
    }
}