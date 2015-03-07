using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class IncapCloseFrom : IIncapCloseFrom
    {
        public int IncapNum { get; private set; }

        internal IncapCloseFrom(int incapNum)
        {
            this.IncapNum = incapNum;
        }
    }
}
