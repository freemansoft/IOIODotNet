using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class StatePeriodicDigitalSamplingFrom : IStatePeriodicDigitalSamplingFrom
    {
        private int pin;
        private int freqScale;

        public StatePeriodicDigitalSamplingFrom(int pin, int freqScale)
        {
            // TODO: Complete member initialization
            this.pin = pin;
            this.freqScale = freqScale;
        }
    }
}
