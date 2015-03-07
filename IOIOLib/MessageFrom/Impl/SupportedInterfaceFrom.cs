using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class SupportedInterfaceFrom : ISupportedInterfaceFrom
    {
        public bool IsSupported { get; private set; }

        public SupportedInterfaceFrom(bool isSupported)
        {
            IsSupported = isSupported;
        }
    }
}
