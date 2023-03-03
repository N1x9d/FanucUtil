using System.Collections.Generic;
using System.Linq;
using GCodeRobotCSharpEdition.Tamplates;
using System.Windows.Forms;

namespace GCodeRobotCSharpEdition.Tamplates
{
    public static class LogList
    {
        private static int LogLimit = 50;
        private static List<logHistTamplate> logs = new List<logHistTamplate>();

        public static void Add(int type, string value)
        {
            if (LogLimit <= logs.Count)
            {
                logs.RemoveAt(0);
                logs.Add(new logHistTamplate(type, value));
            }

            if (logs.Count != 0)
            {
                if (logs.Last().Type == type && logs.Last().Value == value)
                {
                }
                else if (logs.Last().Type == 2 && type == 2)
                {
                }
                else
                    logs.Add(new logHistTamplate(type, value));
            }
            else
                logs.Add(new logHistTamplate(type, value));
        }

        public static void AddZ(float z)
        {
            logs.Last().Z = z;
        }

        public static string Print()
        {
            var outStr = "";
            foreach (var log in logs)
            {
                outStr += log.Value + "\r\n";
            }

            return outStr;
        }
    }
}