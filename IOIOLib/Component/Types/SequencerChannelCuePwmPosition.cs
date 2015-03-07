using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * A cue for a PWM position channel.
     * <p>
     * Determines what is going to be the pulse width will while cue is executing.
     */
    public class SequencerChannelCuePwmPosition : ISequencerChannelCue
    {
        /**
         * The pulse-width, in time-base units, as determined for this channel in its configuration.
         * Valid Values are 0 or [2..65536].
         */
        public int pulseWidth;
    }
}
