using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.IOIOException
{
    public class OutOfResourceException : Exception
    {
        public OutOfResourceException(string p)
            : base(p)
        {
        }

        public OutOfResourceException(string p, Exception e)
            : base(p, e)
        {

        }
    }
}
