using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /**
     * A clock rate selection, which implies a time-base.
     */
    public enum SequencerClock
    {
        /** 16 MHz (62.5ns time-base). */
        CLK_16M,
        /** 2 MHz (0.5us time-base). */
        CLK_2M,
        /** 250 KHz (4us time-base). */
        CLK_250K,
        /** 62.5 KHz (16us time-base). */
        CLK_62K5
    }
}
