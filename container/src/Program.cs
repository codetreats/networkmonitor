using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServerMonitor
{
    class Program
    {
        public static readonly bool DEBUG = true;

        static void Main(string[] args)
        {
            string dataDir;
            string confDir;
            string htmlDir;
            string mailDir;
            string logDir;

            if (args.Length != 3 || !Directory.Exists(args[0]) || !Directory.Exists(args[1]) || !Directory.Exists(args[2]))
            {
                Log.d("Program","Usage: ServerMonitor.exe ROOTDIR HTMLDIR LOGDIR");
                return;
            }
            dataDir = Path.Combine(args[0], "data");
            confDir = Path.Combine(args[0], "config");
            mailDir = Path.Combine(args[0], "mail");            
            htmlDir = args[1];
            logDir = args[2];

            if (!Directory.Exists(dataDir))
            {                
                Log.e("Program", "DataDir " + dataDir + " not exists");
                return;
            }
            
            if (!Directory.Exists(confDir))
            {
                Log.e("Program", "ConfDir " + confDir + " not exists");
                return;
            }

            if (!Directory.Exists(mailDir))
            {
                Log.e("Program", "MailDir " + mailDir + " not exists");
                return;
            }

            Monitor monitor = new Monitor(dataDir, confDir, mailDir, htmlDir, logDir);
            monitor.run();

            Log.d("Program", "Fertig");


            if (DEBUG)
            {
                Console.ReadKey();
            }
        }
    }
}
