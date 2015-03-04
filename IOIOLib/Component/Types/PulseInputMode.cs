using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /** An enumeration for describing the module's operating mode. */
    public class PulseInputMode
    {
        /** Positive pulse measurement (rising-edge-to-falling-edge). */
        public static PulseInputMode POSITIVE = new PulseInputMode(1);
        /** Negative pulse measurement (falling-edge-to-rising-edge). */
        public static PulseInputMode NEGATIVE = new PulseInputMode(1);
        /** Frequency measurement (rising-edge-to-rising-edge). */
        public static PulseInputMode FREQ = new PulseInputMode(1);
        /** Frequency measurement (rising-edge-to-rising-edge) with 4x scaling. */
        public static PulseInputMode FREQ_SCALE_4 = new PulseInputMode(4);
        /** Frequency measurement (rising-edge-to-rising-edge) with 16x scaling. */
        public static PulseInputMode FREQ_SCALE_16 = new PulseInputMode(16);

        /** The scaling factor as an integer. */
        public int scaling {  get;  set; }

        private PulseInputMode(int s)
        {
            scaling = s;
        }
    }
}
