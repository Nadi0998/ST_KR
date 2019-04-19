using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST_diplom
{
    class History
    {
        public static void Init()
        {
            var x = System.IO.File.OpenWrite("history.txt");
            x.Close();
        }

        public static void AddLine(string s)
        {
            var writeStream = new System.IO.StreamWriter("history.txt", true);
            if (writeStream != null)
            {
                writeStream.Write(s + "\r\n");
                writeStream.Close();
            }
        }

        public static void OpenHistory()
        {
            System.Diagnostics.Process.Start("history.txt");
        }
    }
}
