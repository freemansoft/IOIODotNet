using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class UartCloseFrom : IUartCloseFrom
    {
        public int UartNum { get; private set; }

        public UartCloseFrom(int uartNum)
        {
            this.UartNum = uartNum;
        }
    }
}
