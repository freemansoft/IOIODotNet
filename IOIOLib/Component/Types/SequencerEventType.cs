/*
 * Copyright 2011 Ytai Ben-Tsvi. All rights reserved.
 * Copyright 2015 Joe Freeman. All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 * 
 *    1. Redistributions of source code must retain the above copyright notice, this list of
 *       conditions and the following disclaimer.
 * 
 *    2. Redistributions in binary form must reproduce the above copyright notice, this list
 *       of conditions and the following disclaimer in the documentation and/or other materials
 *       provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED 'AS IS AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL ARSHAN POURSOHI OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * Event Type.
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
         * This event Type is only sent once, when the sequencer has been closed. It is mostly
         * intended to release a client blocking on {@link Sequencer#waitEvent()}. It is also
         * used if {@link Sequencer#getLastEvent()} is called before any event has been sent.
         */
        CLOSED
    }
}
