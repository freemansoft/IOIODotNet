using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    class ConfigureAnalogInputTo : IConfigureAnalogInputTo
    {
        internal int Pin { get; set; }

        internal Boolean? ChangeNotify = null;


        public ConfigureAnalogInputTo(int pin)
        {
            this.Pin = pin;
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
