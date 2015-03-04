using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * Configuration for a channel of type FM speed.
     * <p>
     * FM speed channels are channels in which fixed-width pulses are generated with varying
     * frequency, which corresponds to the actuator speed. A good example is a stepper motor in an
     * application which requires speed control and not position control (for the latter see
     * {@link ChannelConfigSteps}). An FM speed channel will idle (not produce any pulses) during a
     * stall event.
     */
    public class SequencerChannelConfigFmSpeed : ISequencerChannelConfig
    {
        /**
         * Specification of the output pin(s) for this channel.
         */
        public DigitalOutputSpec[] pinSpec;

        /**
         * The clock rate for this channel (cannot be changed on a per-cue basis).
         */
        public SequencerClock clk;

        /**
         * The width (duration) of each pulse, in time-base units, determined by {@link #clk}. Valid
         * values are [2..65536].
         */
        public int pulseWidth;

        /**
         * Constructor.
         * <p>
         *
         * @param clk
         *            See {@link #clk}.
         * @param pulseWidth
         *            See {@link #pulseWidth}.
         * @param pinSpec
         *            See {@link #pinSpec}.
         */
        public SequencerChannelConfigFmSpeed(SequencerClock clk, int pulseWidth, DigitalOutputSpec[] pinSpec)
        {
            if (pulseWidth < 2 || pulseWidth > (1 << 16))
            {
                throw new ArgumentException("Pulse width must be between [2..65536]");
            }
            this.pinSpec = pinSpec;
            this.clk = clk;
            this.pulseWidth = pulseWidth;
        }
    }
}
