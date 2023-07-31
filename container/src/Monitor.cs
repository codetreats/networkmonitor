using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServerMonitor
{
    public class Monitor
    {
        private string dataDir;
        private string confDir;
        private string mailDir;
        private string htmlDir;
        private string logDir;

        private List<Guardian> guardians = new List<Guardian>();

        public Monitor(string dataDir, string confDir, string mailDir, string htmlDir, string logDir)
        {
            this.dataDir = dataDir;
            this.confDir = confDir;
            this.mailDir = mailDir;
            this.htmlDir = htmlDir;
            this.logDir = logDir;
        }

        public void run()
        {            
            parse();
            MailBuilder mailBuilder = new MailBuilder(guardians.ToArray());
            mailBuilder.build(mailDir);
            HtmlBuilder htmlBuilder = new HtmlBuilder(guardians.ToArray());
            htmlBuilder.build(htmlDir);
            LogBuilder logBuilder = new LogBuilder(guardians.ToArray());
            logBuilder.build(logDir);
            
            print();    
        }

        private void parse()
        {
            // Eine Liste für jedes Level
            List<Guardian>[] guardians = new List<Guardian>[6];
            for (int i = 0; i < guardians.Length; i++)
            {
                guardians[i] = new List<Guardian>();
            }

            string[] confFilePathes = Directory.GetFiles(confDir, "*.cfg");
            Log.d("Parse configs: ", confDir);
            foreach (string confFilePath in confFilePathes)
            {
                string confFilename = Path.GetFileName(confFilePath);
                Log.d(this.GetType().Name,confFilename);
                Guardian guard = new Guardian(confFilename, confDir, dataDir);
                guard.parse();
                guard.generateOutput();
                if (guard.isEnabled())
                {                    
                    guardians[getArrayPos(guard.getLevel())].Add(guard);
                }
            }

            // Sortiere alle Guardians nach Level ein => Critical zuerst
            for (int i = 0; i < guardians.Length; i++)
            {
                foreach (Guardian guard in guardians[i])
                {
                    this.guardians.Add(guard);
                }
            }
        }

        private int getArrayPos(Level level)
        {
            int l = level.getLevel();
            switch (l)
            {
                case 5:
                    return 0;
                case 3:
                    return 1;
                case 2:
                    return 2;
                case 1:
                    return 3;
                case 0:
                    return 4;
                default:
                    return 5;
            }
        }

        private void print()
        {
            foreach (Guardian guard in guardians)
            {
                Log.d(this.GetType().Name, guard.getName());
                Log.d(this.GetType().Name,guard.getDateTime().ToString());
                Log.d(this.GetType().Name,guard.getLevel().toString());
                Log.d(this.GetType().Name,guard.getText());
            }
        }
    }
}
