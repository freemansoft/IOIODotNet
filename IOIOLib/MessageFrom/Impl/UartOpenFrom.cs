using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class UartOpenFrom : IUartOpenFrom
    {
        public int UartNum { get; private set; }

        public UartOpenFrom(int uartNum)
        {
            this.UartNum = uartNum;
        }
    }
}
