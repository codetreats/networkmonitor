using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServerMonitor
{
    public class Report : Error
    {
        private string filename;

        private DateTime dateTime;
        private Level level;
        private string text = string.Empty;

        public Report(string filename)
        {
            this.filename = filename;
        }

        public void parse()
        {
            parse(false);
        }

        public void parse(bool redo)
        {
            string[] lines;
            string line = "";
            if (redo)
            {
                System.Threading.Thread.Sleep(500);
            }            
            if (!File.Exists(filename))
            {
                // If it is the first time
                if (!redo)
                {
                    // Sleep 5 seconds and parse again
                    parse(true);                    
                }
                else
                {
                    // Otherwise send an error
                    error = true;
                    errorText = "Datei " + filename + " existiert nicht!";
                    Log.e("Report", errorText);                    
                }
                return;
            }

            try
            {
                lines = File.ReadAllLines(filename);
                foreach (string l in lines) 
                {
                    line += l + "|";
                }
            }
            catch (Exception e)
            {
                // If it is the first time
                if (!redo)
                {
                    // Sleep 5 seconds and parse again
                    parse(true);
                }
                else
                {
                    // Otherwise send an error
                    error = true;
                    errorText = "Fehler beim Oeffnen von " + filename + ": " + e.ToString();
                    Log.e("Report", errorText);
                }
                return;
            }

            if (lines.Length < 2)
            {
                // If it is the first time
                if (!redo)
                {
                    // Sleep 5 seconds and parse again
                    parse(true);
                }
                else
                {
                    // Otherwise send an error
                    error = true;
                    errorText = "Ungueltiges Format: " + filename;
                    Log.e("Report", errorText + "(" + lines.Length + " lines:" + line + ")");
                }
                return;
            }
            dateTime = parseDate(lines[0]);
            if (error)
            {
                // If it is the first time
                if (!redo)
                {
                    // Sleep 5 seconds and parse again
                    parse(true);
                }
                return;
            }
            level = Level.parse(lines[1]);
            StringBuilder sb = new StringBuilder();
            for (int i = 2; i < lines.Length; i++)
            {
                sb.AppendLine(lines[i]);
            }
            text = sb.ToString();
        }

        private DateTime parseDate(string str)
        {
            try
            {
                string date = str.Trim().Split(' ')[0].Trim();
                string time = str.Trim().Split(' ')[1].Trim();

                Log.d(this.GetType().Name, date);
                Log.d(this.GetType().Name, time);

                int year = Convert.ToInt32(date.Split('-')[0]);
                int month = Convert.ToInt32(date.Split('-')[1]);
                int day = Convert.ToInt32(date.Split('-')[2]);

                int hour = Convert.ToInt32(time.Split(':')[0]);
                int minute = Convert.ToInt32(time.Split(':')[1]);
                int second = Convert.ToInt32(time.Split(':')[2]);

                return new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception e)
            {
                error = true;
                errorText = "Ungueltiges Zeitformat";
                Log.e(this.GetType().Name, e.ToString());
                return DateTime.Now;
            }
        }

        public DateTime getDateTime()
        {
            return dateTime;
        }

        public Level getLevel()
        {
            return level;
        }

        public string getText()
        {
            return text;
        }
    }


}
