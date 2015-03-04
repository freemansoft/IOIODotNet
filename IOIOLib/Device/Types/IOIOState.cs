using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    /**
     * A state of a IOIO instance.
     */
    public enum IOIOState
    {
        /** Connection not yet established. */
        INIT = 0,
        /** Connected. */
        CONNECTED = 1,
        /** Connection established, incompatible firmware detected. */
        INCOMPATIBLE = 2,
        /** Disconnected. Instance is useless. */
        DEAD = 3
    }
}
