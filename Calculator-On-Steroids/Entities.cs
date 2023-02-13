using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Calculator_On_Steroids
{
    public class Entities
    {
        public enum Arithmetic
        {
            Add,
            Subtract,
            Multiply,
            Divide,
        }

        public enum MessageType
        {
            Success,
            Stop,
            Error
        }
        public enum FileTypes
        {
            CSVType,
            XMLType
        }
        public struct FileParam
        {
            public int HistoryID;
            public string HistoryAction;
            public string HistoryValue;
        }

        public struct Actions
        {
            public static OpenFileDialog OFD = new OpenFileDialog();
            public static SaveFileDialog SFD = new SaveFileDialog();
            public static FolderBrowserDialog FBD = new FolderBrowserDialog();

            public static string SavePath= "";

            public static string FilePath = "";
            public static string Filetype(FileTypes FileTypes)
            {
                switch (FileTypes)
                {
                    case FileTypes.CSVType:
                        return "CSV files (*.csv)|*.csv";
                    case FileTypes.XMLType:
                        return "XML files (*.xml)|*.xml";
                    default:
                        return "";
                }
            }
        }
    }
}
