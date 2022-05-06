﻿using System;
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
            Value = value;
        }

        public int Type { get; set; } //1 print status, 2 hieght, 3 other
        private string val;
        public String Value { get
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
                return Value;
            else
            {
                string outVal = $"cur z={Z} min z = {MinZ} max z = {MaxZ} avgZ = {AvgZ}";
                return outVal;
            }
        }
    }
}