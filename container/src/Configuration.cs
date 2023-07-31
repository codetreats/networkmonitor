using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServerMonitor
{
    public class Configuration : Error
    {
        private string filename;

        private string name = string.Empty;
        private bool enabled = true;
        private int warningInterval = Int32.MaxValue;
        private int errorInterval = Int32.MaxValue;
        private int criticalInterval = Int32.MaxValue;

        public Configuration(string filename)
        {
            this.filename = filename;
        }

        public void parse()
        {
            string[] lines;
            if (!File.Exists(filename))
            {
                error = true;
                errorText = "Datei " + filename + " existiert nicht!";
                Log.e("Configuration", errorText);
                return;
            }

            try
            {
                lines = File.ReadAllLines(filename);
            }
            catch (Exception e)
            {
                error = true;
                errorText = "Fehler beim Oeffnen von " + filename + ": " + e.ToString();
                Log.e("Configuration", errorText);
                return;
            }

            foreach (string line in lines)
            {
                parseLine(line);
            }
        }

        private void parseLine(string line)
        {
            // Ignore empty lines and comments
            if (line.Trim().Length <= 0 || line.StartsWith("#"))
            {
                return;
            }
            string[] parts = line.Split('=');
            if (parts.Length != 2)
            {
                error = true;
                errorText = "Ungueltige Konfiguration: " + filename;
                Log.e("Configuration", errorText + "(" + line + " is invalid)");
                return;
            }

            string name = parts[0].Trim().ToLower();
            string val = parts[1].Trim().ToLower();

            Log.d(this.GetType().Name, "Parse: " + line);

            switch (name)
            {
                case "enabled":
                    this.enabled = parseBool(val);
                    break;
                case "name":
                    this.name = val;
                    break;
                case "warninginterval":
                    this.warningInterval = parseInt(val);
                    break;
                case "errorinterval":
                    this.errorInterval = parseInt(val);
                    break;
                case "criticalinterval":
                    this.criticalInterval = parseInt(val);
                    break;
                default:
                    break;
            }
        }

        private int parseInt(string text)
        {
            int result = -1;
            try
            {
                result = Convert.ToInt32(text);
            }
            catch (Exception)
            {
            }

            return result;
        }

        private bool parseBool(string text)
        {
            if (text.ToLower() == "true" || text.ToLower() == "ja" || text.ToLower() == "yes" || text == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string getName()
        {
            if (name.Equals(string.Empty))
            {
                return Path.GetFileNameWithoutExtension(filename);
            }
            return name;
        }

        /*
         * rivate string name = string.Empty;
        private int interval = -1;
        private string timerErrorMessage = string.Empty;
        private Level 
         * */
        
        public string getMessage(int minutes)
        {
            return getName() + " hat sich mehr als " + getTime(minutes) + " nicht gemeldet!";
        }

        public string getTime(int minutes)
        {
            if (minutes < 60)
            {
                return minutes + " Minuten";
            }
            int hours = (int)Math.Floor(minutes / 60.0);
            minutes = minutes - (hours * 60);
            return hours + " Stunden und " + minutes + " Minuten";
        }

        public Level getLevel(int minutes)
        {
            if (minutes > criticalInterval)
            {
                return Level.Critical;
            }
            if (minutes > errorInterval)
            {
                return Level.Error;
            }
            if (minutes > warningInterval)
            {
                return Level.Warning;
            }
            return Level.OK;
        }

        public bool isEnabled()
        {
            return enabled;
        }
    }
}
