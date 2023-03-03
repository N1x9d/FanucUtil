using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCodeRobotCSharpEdition.Tamplates
{
    public class SendTampalte
    {
        string Current_file_path = "None"; // директория текущего файла
        string Next_file_path = "None"; // директория следующего файла при неприрывной печати
        bool Is_start = false; //начать печать
        bool Stop_after_layer = false; // ожидать подтверждения продолжения печати
        bool Is_continuous = false; // если печать не по слоям и в рамках одного файла высота меняется
        bool Is_last_file = false; // true если последний файл в списке

        public string current_file_path
        {
            get { return $"current_file_path${Current_file_path}"; }
            set { Current_file_path = value; }
        }

        public string is_start
        {
            get { return $"is_start${Is_start.ToString()}"; }
            set { Is_start = Convert.ToBoolean(value); }
        }

        public string stop_after_layer
        {
            get { return $"stop_after_layer${Stop_after_layer.ToString()}"; }
            set { Stop_after_layer = Convert.ToBoolean(value); }
        }

        public string is_continuous
        {
            get { return $"is_continuous${Is_continuous.ToString()}"; }
            set { Is_continuous = Convert.ToBoolean(value); }
        }


        public string is_last_file
        {
            get { return $"is_last_file${Is_last_file.ToString()}"; }
            set { Is_last_file = Convert.ToBoolean(value); }
        }

        public string next_file_path
        {
            get { return $"next_file_path${Next_file_path}"; }
            set { Next_file_path = value; }
        }

        public string convertToString()
        {
            return current_file_path + ";" + is_start + ";" + stop_after_layer + ";" + is_continuous + ";" +
                   is_last_file + ";" + next_file_path;
        }

        public void dropToDef()
        {
            Current_file_path = "None";
            Next_file_path = "None";
            Is_start = false;
            Stop_after_layer = false;
            Is_continuous = false;
            Is_last_file = false;
        }
    }
}