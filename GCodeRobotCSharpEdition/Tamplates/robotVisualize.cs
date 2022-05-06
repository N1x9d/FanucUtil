using GCodeRobotCSharpEdition.Robot;

namespace GCodeRobotCSharpEdition
{
    public struct robotVisualize
    {
       public RobotTamplate robot;
       public Form2 form;
        public robotVisualize(RobotTamplate r,Form2 f)
        {
            robot = r;
            form = f;
        }
    }
}
