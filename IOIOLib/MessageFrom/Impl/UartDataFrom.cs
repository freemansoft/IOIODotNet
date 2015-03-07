using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class UartDataFrom : IUartDataFrom
    {
        private int uartNum;
        private int numBytes;
        private byte[] data;

        public UartDataFrom(int uartNum, int numBytes, byte[] data)
        {
            // TODO: Complete member initialization
            this.uartNum = uartNum;
            this.numBytes = numBytes;
            this.data = data;
        }
    }
}
