using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServerMonitor
{
    public static class Log
    {
        private static readonly string errorLog = "/monitor/logs/error.log";
        private static readonly string debugLog = "/monitor/logs/debug.log";

        public static void d(string tag, string text)
        {
            if (!Program.DEBUG)
            {
                return;
            }
            Console.WriteLine(tag + " (DEBUG): " + text);
            writeLog(debugLog, tag, text);
        }

        public static void i(string tag, string text)
        {
            Console.WriteLine(tag + " (INFO): " + text);
            writeLog(debugLog, tag, text);
        }

        public static void e(string tag, string text)
        {
            Console.WriteLine(tag + " (ERROR): " + text);
            writeLog(errorLog, tag, text);
        }

        public static void writeLog(string filename, string tag, string text)
        {
            try
            {
                File.AppendAllText(filename, DateTimePrinter.print() + " - " + tag + ": " + text + "\n");
            }
            catch (Exception)
            {
            }
        }
    }
}
