using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class CloseUartTo : ICloseUartTo
    {
        public int UartNum { get; private set; }

        internal CloseUartTo(int uartNum)
        {
            this.UartNum = uartNum;
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
