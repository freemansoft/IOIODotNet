using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * A digital input pin specification, used when opening digital inputs.
     */
    public class DigitalInputSpec
    {
        /** The pin number, as labeled on the board. */
        public int pin;
        /** The pin mode. */
        public DigitalInputSpecMode mode;

        /**
         * Constructor.
         * 
         * @param pin
         *            Pin number, as labeled on the board.
         * @param mode
         *            Pin mode.
         */
        public DigitalInputSpec(int pin, DigitalInputSpecMode mode)
        {
            this.pin = pin;
            this.mode = mode;
        }

        /** Shorthand for Spec(pin, Mode.FLOATING). */
        public DigitalInputSpec(int pin) :
            this(pin, DigitalInputSpecMode.FLOATING)
        {
        }
    }
}
