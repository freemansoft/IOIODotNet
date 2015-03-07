using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class UartOpenFrom : IUartOpenFrom
    {
        private int uartNum;

        public UartOpenFrom(int uartNum)
        {
            // TODO: Complete member initialization
            this.uartNum = uartNum;
        }
    }
}
