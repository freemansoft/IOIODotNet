using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOIOLib.Component.Impl
{
    /// <summary>
    /// Abstraction for this feature. See the Java version for an idea of functionality
    /// </summary>
    class Uart : IUart
    {
        private Types.DigitalInputSpec rx;
        private Types.DigitalOutputSpec tx;
        private int baud;
        private Types.UartParity parity;
        private Types.UartStopBits stopbits;

        public Uart(Types.DigitalInputSpec rx, Types.DigitalOutputSpec tx, int baud, Types.UartParity parity, Types.UartStopBits stopbits)
        {
            // TODO: Complete member initialization
            this.rx = rx;
            this.tx = tx;
            this.baud = baud;
            this.parity = parity;
            this.stopbits = stopbits;
        }
    }
}
