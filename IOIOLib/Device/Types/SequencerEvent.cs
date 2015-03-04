using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    public enum SequencerEvent
    {
        PAUSED = 0,
        STALLED = 1,
        OPENED = 2,
        NEXT_CUE = 3,
        STOPPED = 4,
        CLOSED = 5

    }
}
