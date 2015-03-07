using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    class ConfigureAnalogInputTo : IConfigureAnalogInputTo
    {
        public  int Pin { get; private set; }

        public Boolean? ChangeNotify { get; private set; }


        internal ConfigureAnalogInputTo(int pin)
        {
            this.Pin = pin;
            this.ChangeNotify = null;
        }

        /// <summary>
        /// create an inbound analong channel that samples data
        /// </summary>
        /// <param name="Pin"></param>
        /// <param name="notifyOnChange">at a 1khz rate</param>
        public ConfigureAnalogInputTo(int pin, bool notifyOnChange)
        {
            this.Pin = pin;
            ChangeNotify = notifyOnChange;
        }


        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            outBound.setPinAnalogIn(this.Pin);
            if (ChangeNotify.HasValue && ChangeNotify.Value)
            {
                outBound.setAnalogInSampling(this.Pin, ChangeNotify.Value);
            }
            return true;
        }
    }
}
