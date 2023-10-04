using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace ServerMonitor
{
    public class LogBuilder
    {
        private Guardian[] guards;

        private int okCounter = 0;
        private int informationCounter = 0;
        private int warningCounter = 0;
        private int errorCounter = 0;
        private int criticalCounter = 0;
        private int unknownCounter = 0;

        private StringBuilder information, warning, error, critical, unknown;

        private string dt = DateTimePrinter.printReverse();

        public LogBuilder(Guardian[] guards)
        {
            this.guards = guards;

            information = new StringBuilder();
            warning = new StringBuilder();
            error = new StringBuilder();
            critical = new StringBuilder();

            unknown = new StringBuilder();

        }

        public void build(string logDir)
        {
            StringBuilder logText = new StringBuilder();
            int total = 0;
            int evil = 0;
            foreach (Guardian guard in guards)
            {
                total++;
                string line = dt + " (" + guard.getLevel().toString() + "): " + guard.getName() + " (" + guard.getText().Replace(System.Environment.NewLine, " ") + ")";
                switch (guard.getLevel().getLevel())
                {
                    case 0:
                        okCounter++;
                        break;
                    case 1:
                        informationCounter++;
                        information.AppendLine(line);
                        break;
                    case 2:
                        warningCounter++;
                        evil++;
                        warning.AppendLine(line);
                        break;
                    case 3:
                        errorCounter++;
                        evil++;
                        error.AppendLine(line);
                        break;
                    case 5:
                        criticalCounter++;
                        evil++;
                        critical.AppendLine(line);
                        break;
                    default:
                        unknownCounter++;
                        evil++;
                        unknown.AppendLine(line);
                        break;
                }
            }

            if (evil == 0)
            {
                return;
            }            
            logText.AppendLine();
            if (criticalCounter > 0)
            {
                logText.Append(critical.ToString());
            }
            if (errorCounter > 0)
            {
                logText.Append(error.ToString());
            }
            if (warningCounter > 0)
            {
                logText.Append(warning.ToString());
            }
            if (informationCounter > 0)
            {
                logText.Append(information.ToString());
            }
            if (unknownCounter > 0)
            {
                logText.Append(unknown.ToString());
            }

            string logFilename = Path.Combine(logDir, "monitor.log");
            try
            {
                File.AppendAllText(logFilename, logText.ToString());
            }
            catch (Exception e)
            {
                Log.e(this.GetType().Name, "Exception: " + e.ToString());
            }
        }
    }
}
