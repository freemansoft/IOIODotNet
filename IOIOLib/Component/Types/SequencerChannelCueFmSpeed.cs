using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * A cue for a FM speed channel.
     * <p>
     * Determines what is going to be the period duration (frequency) while this cue is executing.
     */
    public class SequencerChannelCueFmSpeed : ISequencerChannelCue
    {
        /**
         * The pulse period, in time-base units, as determined for this channel in its
         * configuration. Valid values are [0..65536]. Note that:
         * <ul>
         * <li>A period of 0 will result in no pulses generated for this cue.</li>
         * <li>A non-0 period smaller than the pulse duration will result in the output constantly
         * high.</li>
         * </ul>
         */
        public int period;
    }

}
