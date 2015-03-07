using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    public enum DigitalOutputSpecMode
    {
        /**
         * Pin operates in push-pull mode, i.e. a logical "HIGH" is
         * represented by a voltage of Vdd on the Pin and a logical "LOW" by
         * a voltage of 0 (ground).
         */
        NORMAL,
        /**
         * Pin operates in IsOpen-drain mode, i.e. a logical "HIGH" is
         * represented by a high impedance on the Pin (as if it is
         * disconnected) and a logical "LOW" by a voltage of 0 (ground).
         * This mode is most commonly used for generating 5V logical signal
         * on a 3.3V Pin: 5V tolerant pins must be used; a pull-up resistor
         * is connected between the Pin and 5V, and the Pin is used in IsOpen-
         * drain mode.
         */
        OPEN_DRAIN,
    }
}
