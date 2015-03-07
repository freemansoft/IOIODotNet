using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class StateCapSenseSamplingFrom : IStateCapSenseSamplingFrom
    {
        private int pinNum;
        private bool enable;

        public StateCapSenseSamplingFrom(int pinNum, bool enable)
        {
            // TODO: Complete member initialization
            this.pinNum = pinNum;
            this.enable = enable;
        }
    }
}
