using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServerMonitor
{
    public class Guardian : Error
    {
        private string dataDir;
        private string confDir;
        private string confFilename;

        private Configuration conf;
        private Report report;

        private bool enabled = true;
        private string name = string.Empty;
        private Level combinedLevel;
        private string text;
        private DateTime lastUpdate;

        public Guardian(string confFilename, string confDir, string dataDir)
        {
            this.confFilename = confFilename;
            this.confDir = confDir;
            this.dataDir = dataDir;

            this.conf = new Configuration(Path.Combine(confDir,confFilename));
            this.report = new Report(Path.Combine(dataDir, getReportFilename()));
        }

        public void parse()
        {
            conf.parse();
            report.parse();
        }

        private string getReportFilename()
        {
            if (!confFilename.EndsWith(".cfg") || confFilename.Length < 5)
            {
                error = true;
                errorText = "Invalid file: " + confFilename;
                return null;
            }
            string filenameWithoutEnd = confFilename.Substring(0, confFilename.Length - 4);
            return filenameWithoutEnd + ".txt";
        }

        public void generateOutput()
        {
            Level reportLevel = Level.Unknown;
            Level timerLevel = Level.Unknown;

            string reportText = string.Empty;
            string timerText = string.Empty;

            // Guardian OK?
            if (this.hasError())
            {
                combinedLevel = Level.Error;
                lastUpdate = DateTime.Now;
                text = this.getErrorText();
                Log.e(this.GetType().Name, "Guardian has errors");
                return;
            }

            // Config OK?
            if (conf.hasError())
            {
                combinedLevel = Level.Error;
                lastUpdate = DateTime.Now;
                text = conf.getErrorText();
                Log.e(this.GetType().Name, "Config has errors");
                return;
            }

            name = conf.getName();
            enabled = conf.isEnabled();

            // Report OK?
            if (report.hasError())
            {
                combinedLevel = Level.Error;
                lastUpdate = DateTime.Now;
                text = report.getErrorText();
                Log.e(this.GetType().Name, "Report has errors");
                return;
            }

            reportLevel = report.getLevel();
            reportText = report.getText();
            lastUpdate = report.getDateTime();

            Log.d(this.GetType().Name, "ReportLevel: " + reportLevel.toString());
            
            // Timer abgelaufen?
            TimeSpan ts = DateTime.Now - report.getDateTime();
            int minutes = (int)Math.Ceiling(ts.TotalMinutes);

            
            timerLevel = conf.getLevel(minutes);
            if (timerLevel.compareTo(Level.OK) > 0)
            {
                timerText = conf.getMessage(minutes);
            }
            lastUpdate = report.getDateTime();
            
            // Merge
            switch(reportLevel.compareTo(timerLevel))
            {
                // reportLevel < timerLevel
                case -1:
                    combinedLevel = timerLevel;
                    text = timerText;
                    break;
                // reportLevel >= timerLevel
                case 0:
                    // Kein break!
                case +1:
                    combinedLevel = reportLevel;
                    text = reportText;
                    break;
                // Sollte niemals eintreten
                default:
                    break;
            }

        }

        public string getName()
        {
            return name;
        }

        public Level getLevel()
        {
            return combinedLevel;
        }

        public string getText()
        {
            return text;
        }

        public DateTime getDateTime()
        {
            return lastUpdate;
        }

        public bool isEnabled()
        {
            return enabled;
        }
    }
}
