using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor
{
    public class Level
    {
        private int level;
        private string name;
        private string textColor;
        private string bgColor;

        private static string green = "#00CC00";
        private static string yellow = "#CCCC00";
        private static string red = "#CC0000";
        private static string darkred = "#660000";
        private static string black = "#000000";
        private static string grey = "#C0C0C0";
        //private static string white = "#FFFFFF";


        public static Level OK = new Level(0, "OK", black, green);
        public static Level Information = new Level(1, "Information", black, green);
        public static Level Warning = new Level(2, "Warning", black, yellow);
        public static Level Error = new Level(3, "Error", black, red);
        public static Level Critical = new Level(5, "Critical", black, darkred);

        public static Level Unknown = new Level(-1, "Unknown", black, grey);

        public Level(int level, string name, string textColor, string bgColor)
        {
            this.level = level;
            this.name = name;
            this.textColor = textColor;
            this.bgColor = bgColor;
        }

        public static Level parse(string val)
        {
            switch (val.ToLower())
            {
                case "0":
                case "ok":
                    return Level.OK;
                case "1":
                case "information":
                    return Level.Information;
                case "2":
                case "warning":
                    return Level.Warning;
                case "3":
                case "error":
                    return Level.Error;
                case "5":
                case "critical":
                    return Level.Critical;
                default:
                    return Level.Unknown;
            }

        }

        public int compareTo(Level other)
        {
            return Math.Sign(this.level - other.getLevel());
        }

        public int getLevel()
        {
            return level;
        }

        public string toString()
        {
            return name;
        }

        public string getTextColor()
        {
            return textColor;
        }

        public string getBgColor()
        {
            return bgColor;
        }
    }
}
