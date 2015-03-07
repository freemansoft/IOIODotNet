using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class RegisterPeriodicDigitalSamplingFrom : IRegisterPeriodicDigitalSamplingFrom
    {

        public int Pin { get; private set; }
        public int FrequencyScale { get; private set; }

        internal RegisterPeriodicDigitalSamplingFrom(int pin, int freqScale)
        {
            this.Pin = pin;
            this.FrequencyScale = freqScale;
        }
    }
}
