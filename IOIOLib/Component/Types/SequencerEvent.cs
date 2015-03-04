using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    public class SequencerEvent
    {
        /**
         * The event type.
         */
        public SequencerEventType type;

        /**
         * This gives a counter of the number cues started execution until now, which is
         * synchronized with the events. When the first cue begins execution, the event will be
         * CUE_STARTED and numCuesStarted will be 1.
         */
        public int numCuesStarted;

        /**
         * Constructor.
         */
        public SequencerEvent(SequencerEventType t, int n)
        {
            type = t;
            numCuesStarted = n;
        }
    }
}
