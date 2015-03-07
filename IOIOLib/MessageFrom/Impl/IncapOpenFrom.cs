using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class IncapOpenFrom : IIncapOpenFrom
    {
        public int IncapNum { get; private set; }

        public IncapOpenFrom(int incapNum)
        {
            // TODO: Complete member initialization
            this.IncapNum = incapNum;
        }
    }
}
