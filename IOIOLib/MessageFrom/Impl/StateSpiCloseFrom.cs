using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    class StateSpiCloseFrom : IStateSpiCloseFrom
    {
        private int spiNum;

        public StateSpiCloseFrom(int spiNum)
        {
            // TODO: Complete member initialization
            this.spiNum = spiNum;
        }
    }
}
