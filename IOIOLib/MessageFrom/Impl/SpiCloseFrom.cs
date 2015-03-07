using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    class SpiCloseFrom : ISpiCloseFrom
    {
        private int spiNum;

        public SpiCloseFrom(int spiNum)
        {
            // TODO: Complete member initialization
            this.spiNum = spiNum;
        }
    }
}
