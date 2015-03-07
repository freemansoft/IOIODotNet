using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class ReportDigitalInStatusFrom : IReportDigitalInStatusFrom
    {
        public int Pin { get; private set; }
        public bool Level { get; private set; }

        internal ReportDigitalInStatusFrom(int pin, bool level)
        {
            this.Pin = pin;
            this.Level = level;
        }
    }
}
