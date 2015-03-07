using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class StateReportDigitalInStatusFrom : IStateReportDigitalInStatusFrom
    {
        private int pin;
        private bool level;

        public StateReportDigitalInStatusFrom(int pin, bool level)
        {
            // TODO: Complete member initialization
            this.pin = pin;
            this.level = level;
        }
    }
}
