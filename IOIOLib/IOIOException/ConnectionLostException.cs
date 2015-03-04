using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.IOIOException
{
    public class ConnectionLostException : Exception
    {
        public ConnectionLostException(string p)
            : base(p)
        {
        }

        public ConnectionLostException(string p, Exception e)
            : base(p, e)
        {

        }
    }
}
