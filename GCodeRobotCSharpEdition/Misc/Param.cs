using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCodeRobotCSharpEdition.Misc
{
    public class Param
    {
        public Param(string type, float value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get;}
        public float Value { get;}
    }
}
