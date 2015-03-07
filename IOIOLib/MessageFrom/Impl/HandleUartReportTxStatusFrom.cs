using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class HandleUartReportTxStatusFrom : IHandleUartReportTxStatusFrom
    {
        private int uartNum;
        private int bytesRemaining;

        public HandleUartReportTxStatusFrom(int uartNum, int bytesRemaining)
        {
            // TODO: Complete member initialization
            this.uartNum = uartNum;
            this.bytesRemaining = bytesRemaining;
        }
    }
}
