using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    public enum ResourceType
    {
        PIN = 0,
        OUTCOMPARE = 1,
        INCAP_SINGLE = 2,
        INCAP_DOUBLE = 3,
        TWI = 4,
        ICSP = 5,
        UART = 6,
        SPI = 7,
        SEQUENCER = 8
    }
}
