using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * Configuration for a channel of type PWM speed.
     * <p>
     * PWM speed channels are channels in which a PWM signal is generated, and the pulse width
     * controls the speed of the actuator. A good example is a DC motor. The main difference from a
     * PWM position channel is that the position channel will go back to its initial pulse width
     * during a stall event.
     */
    public class SequencerChannelConfigPwmSpeed : ISequencerChannelConfig
    {
        /**
         * Specification of the output Pin(s) for this channel.
         */
        public DigitalOutputSpec[] pinSpec;

        /**
         * The clock rate for this channel (cannot be changed on a per-cue basis).
         */
        public SequencerClock clk;

        /**
         * The PWM period, in time-base units, determined by {@link #clk}. Valid values are
         * [2..65536].
         */
        public int period;

        /**
         * The initial pulse width (before any cue is executed), in time-base units, determined by
         * {@link #clk}. Valid values are 0 or [2..65536]. Also used in the event of a stall.
         */
        public int initialPulseWidth;

        /**
         * Constructor.
         * <p>
         *
         * @param clk
         *            See {@link #clk}.
         * @param period
         *            See {@link #period}.
         * @param initialPulseWidth
         *            See {@link #initialPulseWidth}.
         * @param pinSpec
         *            See {@link #pinSpec}.
         */
        public SequencerChannelConfigPwmSpeed(SequencerClock clk, int period, int initialPulseWidth,
                DigitalOutputSpec[] pinSpec)
        {
            if (period < 2 || period > (1 << 16))
            {
                throw new ArgumentException("Period width must be between [2..65536]");
            }
            if (initialPulseWidth != 0 && (initialPulseWidth < 2 || initialPulseWidth > (1 << 16)))
            {
                throw new ArgumentException(
                        "Initial pulse width must be 0 or between [2..65536]");
            }
            this.pinSpec = pinSpec;
            this.clk = clk;
            this.period = period;
            this.initialPulseWidth = initialPulseWidth;
        }
    }
}
