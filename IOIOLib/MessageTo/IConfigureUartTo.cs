using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    public interface IConfigureUartTo : IMesssageToIOIO
    {

        DigitalInputSpec RXSpec { get; }
        DigitalOutputSpec TXSpec { get; }

        UartParity Parity { get; }

        int UartNum { get; }

        UartStopBits StopBits { get; }

        int Rate { get; }

        bool Speed4x { get; }
    }
}
