using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class CapSenseSamplingFrom : ICapSenseSamplingFrom
    {


        public int PinNum { get; private set; }
        public bool IsEnabled { get; private set; }


        public CapSenseSamplingFrom(int pinNum, bool isEnabled)
        {
            // TODO: Complete member initialization
            this.PinNum = pinNum;
            this.IsEnabled = isEnabled;
        }
    }
}
