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
