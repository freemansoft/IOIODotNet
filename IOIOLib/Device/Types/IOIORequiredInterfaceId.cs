using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    /// <summary>
    /// Really the firmware (interface) version major code string
    /// </summary>
    internal class IOIORequiredInterfaceId
    {
        public  static byte[] REQUIRED_INTERFACE_ID =
            System.Text.Encoding.ASCII.GetBytes("IOIO0005");
    }
}
