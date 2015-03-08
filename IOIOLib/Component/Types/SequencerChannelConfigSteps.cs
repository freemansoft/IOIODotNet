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
     * Configuration for a channel of type steps.
     * <p>
     * Steps channels are channels in which fixed-width pulses are generated with varying frequency,
     * which corresponds to the actuator speed. A good example is a stepper motor in an application
     * which requires position control. Unlike the FM speed channel, steps channels will generate a
     * deterministic number of steps in a given duration, as well as allow changing the time-base on
     * every cue. See {@link ChannelCueSteps} for a discussion on number of steps calculation. A
     * steps channel will idle (not produce any pulses) during a stall event.
     */
    public class SequencerChannelConfigSteps : ISequencerChannelConfig
    {
        /**
         * Specification of the output Pin(s) for this channel.
         */
        public DigitalOutputSpec[] pinSpec;

        /**
         * Constructor.
         * <p>
         *
         * @param pinSpec
         *            See {@link #pinSpec}.
         */
        public SequencerChannelConfigSteps(DigitalOutputSpec[] pinSpec)
        {
            this.pinSpec = pinSpec;
        }
    }
}
