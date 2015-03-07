using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class ReportAnalogPinValuesFrom : IReportAnalogPinValuesFrom
    {
        public int Pin { get; private set; }
        public int Value { get; private set; }

        internal ReportAnalogPinValuesFrom(int pin, int value)
        {
            this.Pin = pin;
            this.Value = value;
        }
    }
}
