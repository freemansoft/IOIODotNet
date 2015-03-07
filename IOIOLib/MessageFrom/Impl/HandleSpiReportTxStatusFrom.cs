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

        internal HandleSpiReportTxStatusFrom(int spiNum, int bytesRemaining)
        {
            this.SpiNum = spiNum;
            this.BytesRemaining = bytesRemaining;
        }
    }
}
