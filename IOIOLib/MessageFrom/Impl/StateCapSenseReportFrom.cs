using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    class StateCapSenseReportFrom : IStateCapSenseReportFrom
    {
        private int pinNum;
        private int value;

        public StateCapSenseReportFrom(int pinNum, int value)
        {
            // TODO: Complete member initialization
            this.pinNum = pinNum;
            this.value = value;
        }
    }
}
