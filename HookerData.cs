using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Exam_InputHooker_Karvatyuk
{
    internal class HookerData : INotifyPropertyChanged
    {
        // Properties
        private string inputsStr = string.Empty;
        private string[] keyWordsArray = null;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string InputsStr
        {
            get { return inputsStr; }
            set
            {
                if (inputsStr != value)
                {
                    inputsStr = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] KeyWordsArray
        {
            get { return keyWordsArray; }
            set
            {
                keyWordsArray = value;
                OnPropertyChanged();
            }
        }

        // Shift press keys chages
        public string KeyShift(int vkCode)
        {
            switch (vkCode)
            {
                case 192:
                    return "~";
                case 49:
                    return "!";
                case 50:
                    return "@";
                case 51:
                    return "#";
                case 52:
                    return "$";
                case 53:
                    return "%";
                case 54:
                    return "^";
                case 55:
                    return "&";
                case 56:
                    return "*";
                case 57:
                    return "(";
                case 48:
                    return ")";
                case 189:
                    return "_";
                case 187:
                    return "+";
                case 219:
                    return "{";
                case 221:
                    return "}";
                case 220:
                    return "|";
                case 226:
                    return "|";
                case 186:
                    return ":";
                case 222:
                    return "\"";
                case 188:
                    return "<";
                case 190:
                    return ">";
                case 191:
                    return "?";
                default:
                    return ((Keys)vkCode).ToString();
            }
        }

        // Shift press keys chages if not letters
        public string Simbol(int vkCode)
        {
            switch (vkCode)
            {
                case 192:
                    return "`";
                case 189:
                    return "-";
                case 187:
                    return "=";
                case 219:
                    return "[";
                case 221:
                    return "\\";
                case 220:
                    return "]";
                case 226:
                    return "\\";
                case 186:
                    return ";";
                case 222:
                    return "'";
                case 188:
                    return ",";
                case 190:
                    return ".";
                case 191:
                    return "/";
                default:
                    return " " + ((Keys)vkCode).ToString() + " ";
            }
        }
    }
}
