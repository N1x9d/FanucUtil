using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCodeRobotCSharpEdition.Misc
{
    public class OutputFileOptions
    {
        public OutputFileOptions(bool checkLayer, string esplit, bool laserPass, string fName, string outputFile)
        {
            CheckLayer = checkLayer;
            Esplit = esplit;
            LaserPass = laserPass;
            FName = fName;
            OutputFile = outputFile;
        }

        public bool CheckLayer { get; set; }
        public string Esplit { get; set; }

        public bool LaserPass { get; set; }
        public string FName { get; set; }
        public string OutputFile { get; set; }
    }
}
