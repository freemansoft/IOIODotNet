using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /** Supported_ clock rate enum. */
    public class PulseInputClockRate
    {

        /** 16MHz */
        public static PulseInputClockRate RATE_16MHz = new PulseInputClockRate(16000000);
        /** 2MHz */
        public static PulseInputClockRate RATE_2MHz = new PulseInputClockRate(2000000);
        /** 250KHz */
        public static PulseInputClockRate RATE_250KHz = new PulseInputClockRate(250000);
        /** 62.5KHz */
        public static PulseInputClockRate RATE_62KHz = new PulseInputClockRate(62500);

        /** The value in Hertz units. */
        public int hertz {  get;  set; }

        private PulseInputClockRate(int h)
        {
            hertz = h;
        }

    }
}
