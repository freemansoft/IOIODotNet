using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.IOIOException
{
    public class ProtocolError : Exception
    {
        public ProtocolError(string p)
            : base(p)
        {
        }

        public ProtocolError(string p, Exception e)
            : base(p, e)
        {

        }

    }
}
