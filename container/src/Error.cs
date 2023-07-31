using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor
{
    public class Error
    {
        protected string errorText = string.Empty;
        protected bool error = false;

        public string getErrorText()
        {
            return errorText;
        }

        public bool hasError()
        {
            return error;
        }
    }
}
