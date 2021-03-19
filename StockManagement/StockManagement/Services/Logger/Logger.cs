using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace StockManagement.Services.Logger
{
    public static class Logger
    {
        public static void logWrite(string msg)
        {
            string path = HttpRuntime.AppDomainAppPath;
            string currentLocation = $"{path}log.txt";
            try
            {
                using (StreamWriter w = File.AppendText(currentLocation))
                {
                    string str_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    w.WriteLine($"{str_date} ---- {msg}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            

        }
    }
}