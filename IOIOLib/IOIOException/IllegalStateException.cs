using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.IOIOException
{
    public class IllegalStateException : Exception
    {
        public IllegalStateException(string p)
            : base(p)
        {
        }

        public IllegalStateException(string p, Exception e)
            : base(p, e)
        {

        }
    }
}
