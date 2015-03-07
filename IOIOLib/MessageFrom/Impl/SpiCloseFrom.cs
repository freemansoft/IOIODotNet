using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class SpiCloseFrom : ISpiCloseFrom
    {
        public int SpiNum { get; private set; }

        internal SpiCloseFrom(int spiNum)
        {
            this.SpiNum = spiNum;
        }
    }
}
