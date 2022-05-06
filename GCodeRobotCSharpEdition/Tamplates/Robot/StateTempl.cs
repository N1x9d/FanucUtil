using System.Drawing;
namespace GCodeRobotCSharpEdition.Robot
{
    public struct StateTempl
    {
        public string CurrentState;
        public Color color;
        public StateTempl(string currentState, Color color)
        {
            CurrentState = currentState;
            this.color = color;
        }
    }
}
