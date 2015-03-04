using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * Configuration for a binary channel.
     * <p>
     * A binary channel is a simple digital output, which is driven in synchronization with the
     * sequence. Solenoids, DC motors running at full speed (no PWM) or LED are all examples for
     * actuators that can be controlled by a binary channel. During a stall event, the channel can
     * be configured to either retain its last state, or go to its initial state.
     */
    public  class SequencerChannelConfigBinary : ISequencerChannelConfig
    {
        /**
         * Specification of the output pin(s) for this channel.
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
         * <p>
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
