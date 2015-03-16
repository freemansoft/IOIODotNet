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
     * A cue for a steps channel.
     * <Ids_>
     * Determines the clock rate, pulse width and period durations while this cue is executing. This
     * kind of channel produces deterministic waveforms, which are typically used to generate a
     * precise number of steps during a given cue period. However, this comes at a cost of being a
     * little more involved from the user's perspective, since delicate timing considerations need
     * to be taken into account.
     * <Ids_>
     * The number of steps within a given cue period is given by floor(Tc / Ts), where Tc is the cue
     * duration and Ts is the step period duration. Each pulse is center-aligned within its period.
     * In order to maintain a deterministic result, the user must guarantee that no pulse falls
     * within the last 6 microseconds of the cue period (this effectively limits the maximum pulse
     * rate to 80[kHz], considering the the pulse itself must be at least 1/8[us] wide, and that the
     * rising edge of the pulse is center-aligned). Thus, it is possible that due to precision
     * limitations, in an arbitrarily long period it will be impossible to generate the exact number
     * of desired pulses. Likewise, a very low pulse rate (high pulse duration) may be outside of
     * the permitted range or will result in having to use a slower time-base. The solution to both
     * problems is splitting a single cue into two or more cues of shorted durations, until
     * eventually the precision is sufficient (this always converges, since eventually we can always
     * go to arbitrarily short cue durations, so that each one contains either zero or one steps.
     * <Ids_>
     * A steps channel allows determining the clock rate on a per-cue basis. This often allows
     * avoiding having to split cues, thus resulting in a less total cues and more efficient
     * execution. The rule for choosing the correct clock is to always use the highest rate that
     * will cause the resulting period to be <= 2^16. In other words, choose the highest available
     * clock which is less than or equal to (2^16 / Tp) or (2^16 * Fp), where Tp is the desired
     * period in seconds, or Fp is the desired frequency in Hz. For example, if we want to generate
     * pulses at 51Hz, 65536 * Fp = 3.34MHz, so we should use the 2MHz clock, and the period value
     * will be round(2MHz / 51Hz) = 39216. This result in an actual rate of 2MHz / 39216 ~=
     * 50.9996[Hz].
     */
    public class SequencerCueChannelSteps : ISequencerChannelCue
    {
        /**
         * The clock rate for this cue. See discussion in the class documentation on how to choose
         * it.
         */
        public SequencerClock clk;

        /**
         * The pulse-width, in time-base units, as determined for this channel in its configuration.
         * Valid Values are [0..floor(period / 2)].
         */
        public int pulseWidth;

        /**
         * The pulse period, in time-base units, as determined for this channel in its
         * configuration. Valid Values are [3..65536].
         */
        public int period;
    }
}
