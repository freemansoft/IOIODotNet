using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    class CapSenseReportFrom : ICapSenseReportFrom
    {
        public int PinNum { get; private set; }
        public int Value { get; private set; }

        internal CapSenseReportFrom(int pinNum, int value)
        {
            this.PinNum = pinNum;
            this.Value = value;
        }
    }
}
