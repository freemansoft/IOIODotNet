using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class ReportAnalogPinValuesFrom : IReportAnalogPinValuesFrom
    {
        internal int Pin;
        internal int Value;

        public ReportAnalogPinValuesFrom(int pin, int value)
        {
            // TODO: Complete member initialization
            this.Pin = pin;
            this.Value = value;
        }
    }
}
