namespace GCodeRobotCSharpEdition.Misc
{
    public class InputParametrs
    {
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
        public string w { get; set; }
        public string p { get; set; }

        public string r { get; set; }
        public string PUF { get; set; }
        public string PUT { get; set; }
        public string RUF { get; set; }
        public string RUT { get; set; }
        public string nm { get; set; }

        public string CheckDist { get; set; }
        public string wm { get; set; }
        public string j1 { get; set; }
        public string j4 { get; set; }
        public string j6 { get; set; }
        public string WS { get; set; }
        public string WE { get; set; }

        public string j1o { get; set; }
        public string j2o { get; set; }
        public string W { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public bool Arc_disable { get; set; }

        public bool Chechs { get; set; }        
        public bool Auto_arc { get; set; }
        public bool WELD_SPEED { get; set; }
        public bool WSE { get; set; }
        public bool WEE { get; set; }
        public bool RO { get; set; }

        public InputParametrs(Form1 form)
        {
            x = form.X;
            y = form.Y;
            z = form.Z;
            r = form.R;
            PUF = form.PUf;
            PUT = form.PUt;
            RUF = form.RUf;
            RUT = form.RUt;
            w = form.W;
            p = form.P;
            nm = form.Tn;
            wm = form.Tw;
            WS = form.DefDegree.ToString();
            WE = form.WaveIndex.ToString();
            W = form.Wrist.Contains("Flip") ? "F" : "N";
            A = form.Arm.Contains("Up") ? "U" : "D";
            B = form.Base.Contains("Front") ? "T" : "B";
            Arc_disable = form.noArc;
            Auto_arc = form.AutoArc;
            WELD_SPEED = form.WeldSpeed;
            WSE = form.WieldShield;
            WEE = form.GetWave;
            RO = form.GetRO;
            CheckDist = form.CheckDist;
            Chechs = form.Chechs;
            j1o = form.j1o;
            j2o = form.j2o;

            j1 = form.j1;
            j4 = form.j4;
            j6 = form.j6;
        }
    }
}
