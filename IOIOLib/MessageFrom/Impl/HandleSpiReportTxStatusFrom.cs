using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class HandleSpiReportTxStatusFrom : IHandleSpiReportTxStatusFrom
    {
        public int SpiNum { get; private set; }
        public int BytesRemaining { get; private set; }

        public HandleSpiReportTxStatusFrom(int spiNum, int bytesRemaining)
        {
            // TODO: Complete member initialization
            this.SpiNum = spiNum;
            this.BytesRemaining = bytesRemaining;
        }
    }
}
