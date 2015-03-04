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
         * represented by a voltage of Vdd on the pin and a logical "LOW" by
         * a voltage of 0 (ground).
         */
        NORMAL,
        /**
         * Pin operates in open-drain mode, i.e. a logical "HIGH" is
         * represented by a high impedance on the pin (as if it is
         * disconnected) and a logical "LOW" by a voltage of 0 (ground).
         * This mode is most commonly used for generating 5V logical signal
         * on a 3.3V pin: 5V tolerant pins must be used; a pull-up resistor
         * is connected between the pin and 5V, and the pin is used in open-
         * drain mode.
         */
        OPEN_DRAIN,
    }
}
