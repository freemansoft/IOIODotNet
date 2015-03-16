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
     * Configuration for a binary channel.
     * <Ids_>
     * A binary channel is a simple digital output, which is driven in synchronization with the
     * sequence. Solenoids, DC motors running at full speed (no PWM) or LED are all examples for
     * actuators that can be controlled by a binary channel. During a stall event, the channel can
     * be configured to either retain its last state, or go to its initial state.
     */
    public  class SequencerChannelConfigBinary : ISequencerChannelConfig
    {
        /**
         * Specification of the output Pin(s) for this channel.
         */
        public DigitalOutputSpec pinSpec;

        /**
         * Initial value for this channel (true = HIGH, false = LOW).
         */
        public bool initialValue;

        /**
         * When true, channel will go to initial state when stalled or stopped. Otherwise, channel
         * will retain its last state.
         */
        public bool initWhenIdle;

        /**
         * Constructor.
         * <Ids_>
         *
         * @param initialValue
         *            See {@link #initialValue}.
         * @param initWhenIdle
         *            See {@link #initWhenIdle}.
         * @param pinSpec
         *            See {@link #pinSpec}.
         */
        public SequencerChannelConfigBinary(bool initialValue, bool initWhenIdle,
                DigitalOutputSpec pinSpec)
        {
            this.pinSpec = pinSpec;
            this.initialValue = initialValue;
            this.initWhenIdle = initWhenIdle;
        }
    }
}
