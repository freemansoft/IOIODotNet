using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * Event type.
     */
    public enum SequencerEventType
    {
        /**
         * The sequencer has been stopped or never started. This will always be accompanied by a
         * numCuesStarted of 0. This event will also be the first event appearing on the event
         * queue to designate the the sequencer has been opened and the cue FIFO is at its full
         * capacity for pushing messages. This is useful if the client wants to pre-fill the
         * FIFO in order to avoid stalls.
         */
        STOPPED,
        /**
         * A new cue has started executing.
         */
        CUE_STARTED,
        /**
         * A cue has ended execution and the sequencer is idle as result of a pause request.
         */
        PAUSED,
        /**
         * A cue has ended execution and the sequencer is idle as result of the queue becoming
         * empty.
         */
        STALLED,
        /**
         * This event type is only sent once, when the sequencer has been closed. It is mostly
         * intended to release a client blocking on {@link Sequencer#waitEvent()}. It is also
         * used if {@link Sequencer#getLastEvent()} is called before any event has been sent.
         */
        CLOSED
    }
}
