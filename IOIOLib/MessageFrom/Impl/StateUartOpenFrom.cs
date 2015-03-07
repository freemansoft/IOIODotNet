using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class StateUartOpenFrom : IStateUartOpenFrom
    {
        private int uartNum;

        public StateUartOpenFrom(int uartNum)
        {
            // TODO: Complete member initialization
            this.uartNum = uartNum;
        }
    }
}
