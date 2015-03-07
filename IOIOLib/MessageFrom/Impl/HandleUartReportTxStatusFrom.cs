using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class HandleUartReportTxStatusFrom : IHandleUartReportTxStatusFrom
    {
        public int UartNum { get; private set; }
        public int BytesRemaining { get; private set; }

        public HandleUartReportTxStatusFrom(int uartNum, int bytesRemaining)
        {
            // TODO: Complete member initialization
            this.UartNum = uartNum;
            this.BytesRemaining = bytesRemaining;
        }
    }
}
