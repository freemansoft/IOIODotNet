using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    internal class ScaleDivider
    {
        internal static int[] ScaleDiv = new int[] {
        0x1F,  // 31.25
        0x1E,  // 35.714
        0x1D,  // 41.667
        0x1C,  // 50
        0x1B,  // 62.5
        0x1A,  // 83.333
        0x17,  // 125
        0x16,  // 142.857
        0x15,  // 166.667
        0x14,  // 200
        0x13,  // 250
        0x12,  // 333.333
        0x0F,  // 500
        0x0E,  // 571.429
        0x0D,  // 666.667
        0x0C,  // 800
        0x0B,  // 1000
        0x0A,  // 1333.333
        0x07,  // 2000
        0x06,  // 2285.714
        0x05,  // 2666.667
        0x04,  // 3200
        0x03,  // 4000
        0x02,  // 5333.333
        0x01   // 8000
    };
    }
}