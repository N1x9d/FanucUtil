using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GCodeRobotCSharpEdition.Tamplates
{
    public class logHistTamplate
    {
        public logHistTamplate(int type, string value)
        {
            Type = type;
            val = value;
        }

        public int Type { get; set; } //1 print status, 2 hieght, 3 other
        private string val ="";
        public string Value { 
            get
            {
                val = GetString();
                return val;
            }

            set
            {
                val = value;
            }
        }

        private float z;
        public float Z {
            get 
            { 
                return z; 
            } 
            set 
            { 
                z=value;
                if (z < MinZ)
                    MinZ = z;
                else if (z > MaxZ)
                    MaxZ = z;
                AvgZ = (AvgZ + z) / 2;
            } 
        }

        private float MinZ { get; set; } = 1000;

        private float MaxZ { get; set; } = -1;

        private float AvgZ { get; set; }

        private string GetString()
        {
            if (Type == 1 || Type == 3)
                return val;
            else
            {
                string outVal = $"cur z={Z}"+Environment.NewLine+ $"min z = {MinZ}" + Environment.NewLine + $"max z = {MaxZ}" + Environment.NewLine + $"avgZ = {AvgZ}";
                return outVal;
            }
        }
    }
}
