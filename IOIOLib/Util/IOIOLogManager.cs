using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOIOLib.Util.Impl;

namespace IOIOLib.Util
{
    internal class IOIOLogManager
    {
        internal static IOIOLog GetLogger(Type type)
        {
            return new IOIOLogImpl(type);
        }
    }
}
