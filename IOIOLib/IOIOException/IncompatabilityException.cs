using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.IOIOException
{
    public class IncompatibilityException : Exception
    {
        public IncompatibilityException(string p)
            : base(p)
        {
        }

        public IncompatibilityException(string p, Exception e)
            : base(p, e)
        {

        }
    }
}
