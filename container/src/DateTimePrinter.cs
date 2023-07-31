using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor
{
    public static class DateTimePrinter
    {
        public static string print(DateTime dt)
        {
            return String.Format("{0:dd.MM.yyyy HH:mm:ss}", dt);
        }

        public static string print()
        {
            return print(DateTime.Now);
        }

        public static string printReverse(DateTime dt)
        {
            return String.Format("{0:yyyy-MM-dd HH:mm:ss}", dt);
        }

        public static string printReverse()
        {
            return printReverse(DateTime.Now);
        }
    }
}
