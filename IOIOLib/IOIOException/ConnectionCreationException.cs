using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.IOIOException
{
    public class ConnectionCreationException : Exception
    {
        public ConnectionCreationException(string p)
            : base(p)
        {
        }

        public ConnectionCreationException(string p, Exception e)
            : base(p, e)
        {

        }
    }
}
