using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * A cue for a binary channel.
     * <p>
     * This cue determines whether the output should be high or low while the cue is executing.
     */
    public  class SequencerCueBinary : ISequencerChannelCue
    {
        /**
         * The desired output state (true = high, false = low).
         */
        public bool value;
    }
}
