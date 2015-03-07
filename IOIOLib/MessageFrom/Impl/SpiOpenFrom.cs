using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class SpiOpenFrom : ISpiOpenFrom
    {
        public int SpiNum { get; private set; }

        public SpiOpenFrom(int spiNum)
        {
            this.SpiNum = spiNum;
        }
    }
}
