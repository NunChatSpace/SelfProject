using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                Log(args[0], w);
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            string str_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            w.WriteLine($"{str_date}, {logMessage}");
        }
    }
}
