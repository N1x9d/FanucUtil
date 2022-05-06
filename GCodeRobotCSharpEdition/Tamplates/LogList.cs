using System.Collections.Generic;
using System.Linq;
using GCodeRobotCSharpEdition.Tamplates;
using System.Windows.Forms;
namespace GCodeRobotCSharpEdition.Tamplates
{
    public class LogList
    {
        private int LogLimit;
        private List<logHistTamplate> logs;

        public LogList(int logLimit = 50)
        {
            LogLimit = logLimit;
            logs = new List<logHistTamplate>();
        }
        public void Add(int type, string value)
        {
            if (LogLimit <= logs.Count)
                logs.RemoveAt(0);
            if(logs.Count!=0)
            {
                if (logs.Last().Type == type && logs.Last().Value == value)
                    return;
                else if (logs.Last().Type == 2 && type == 2)
                    return;
            }
            logs.Add(new logHistTamplate(type, value));
        }
        public void AddZ(float z)
        {
            logs.Last().Z = z;
        }

        public string Print()
        {
            var outStr = "";
            foreach(var log in logs)
            {
                outStr += log.Value + "\r\n";
            }
            return outStr;  
        }
    }
}
