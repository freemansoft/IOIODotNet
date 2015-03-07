using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class UartDataFrom : IUartDataFrom
    {
        public int UartNum { get; private set; }
        public int NumberOfBytes { get; private set; }
        public byte[] Data { get; private set; }

        public UartDataFrom(int uartNum, int numberOfBytes, byte[] data)
        {
            // TODO: Complete member initialization
            this.UartNum = uartNum;
            this.NumberOfBytes = numberOfBytes;
            this.Data = data;
        }
    }
}
