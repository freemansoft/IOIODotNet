using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class SupportedInterfaceFrom : ISupportedInterfaceFrom
    {
        internal bool IsSupported_ = false;

        public SupportedInterfaceFrom(bool isSupported)
        {
            IsSupported_ = isSupported;
        }

        public bool IsSupported()
        {
            return IsSupported_;
        }
    }
}
