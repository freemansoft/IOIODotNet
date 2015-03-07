using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class ReportDigitalInStatusFrom : IReportDigitalInStatusFrom
    {
        internal int Pin;
        internal bool Level;

        public ReportDigitalInStatusFrom(int pin, bool level)
        {
            // TODO: Complete member initialization
            this.Pin = pin;
            this.Level = level;
        }
    }
}
