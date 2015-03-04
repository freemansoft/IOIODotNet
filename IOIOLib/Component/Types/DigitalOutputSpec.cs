using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    public class DigitalOutputSpec
    {
        /** Output pin mode. */

        /** The pin number, as labeled on the board. */
        public int pin { get; set; }
        /** The pin mode. */
        public DigitalOutputSpecMode mode { get; set; }

        /**
         * Constructor.
         * 
         * @param pin
         *            Pin number, as labeled on the board.
         * @param mode
         *            Pin mode.
         */
        public DigitalOutputSpec(int pin, DigitalOutputSpecMode mode)
        {
            this.pin = pin;
            this.mode = mode;
        }

        /**
         * Shorthand for Spec(pin, Mode.NORMAL).
         * 
         * @see DigitalOutput.Spec#Spec(int, DigitalOutput.Spec.Mode)
         */
        public DigitalOutputSpec(int pin) :
            this(pin, DigitalOutputSpecMode.NORMAL)
        {

        }
    }
}
