using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * A digital input Pin specification, used when opening digital inputs.
     */
    public class DigitalInputSpec
    {
        /** The Pin number, as labeled on the board. */
        public int pin;
        /** The Pin mode. */
        public DigitalInputSpecMode mode;

        /**
         * Constructor.
         * 
         * @param Pin
         *            Pin number, as labeled on the board.
         * @param mode
         *            Pin mode.
         */
        public DigitalInputSpec(int pin, DigitalInputSpecMode mode)
        {
            this.pin = pin;
            this.mode = mode;
        }

        /** Shorthand for Spec(Pin, Mode.FLOATING). */
        public DigitalInputSpec(int pin) :
            this(pin, DigitalInputSpecMode.FLOATING)
        {
        }
    }
}
