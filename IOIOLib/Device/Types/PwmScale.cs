using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    public class PwmScale
    {
        internal static PwmScale Scale1X = new PwmScale(1, 0);
        internal static PwmScale Scale8X = new PwmScale(8, 3);
        internal static PwmScale Scale64X = new PwmScale(64, 2);
        internal static PwmScale Scale256X = new PwmScale(256, 1);

        internal int scale { get; set; }
        internal int encoding { get; set; }

        PwmScale(int scale, int encoding)
        {
            this.scale = scale;
            this.encoding = encoding;
        }
    }
}
