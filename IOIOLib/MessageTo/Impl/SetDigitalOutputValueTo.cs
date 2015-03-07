using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class SetDigitalOutputValueTo : ISetDigitalOutputValueTo
    {
        private int Pin;
        private bool Level;


        public SetDigitalOutputValueTo(int pin, bool level)
        {
            // TODO: Complete member initialization
            this.Pin = pin;
            this.Level = level;
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            outBound.setDigitalOutLevel(Pin, Level);
            return true;
        }
    }
}
