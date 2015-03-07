using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class HandleSpiReportTxStatusFrom : IHandleSpiReportTxStatusFrom
    {
        private int spiNum;
        private int bytesRemaining;

        public HandleSpiReportTxStatusFrom(int spiNum, int bytesRemaining)
        {
            // TODO: Complete member initialization
            this.spiNum = spiNum;
            this.bytesRemaining = bytesRemaining;
        }
    }
}
