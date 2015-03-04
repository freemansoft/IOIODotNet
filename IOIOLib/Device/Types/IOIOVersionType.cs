using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    public enum IOIOVersionType
    {
        /** Hardware version. */
        HARDWARE_VER = 0,
        /** Bootloader version. */
        BOOTLOADER_VER = 1,
        /** Application layer firmware version. */
        APP_FIRMWARE_VER = 2,
        /** IOIOLib version. */
        IOIOLIB_VER = 3
    }
}
