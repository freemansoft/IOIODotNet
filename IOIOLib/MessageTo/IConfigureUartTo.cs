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

        DigitalInputSpec RXSpec { get; set; }
        DigitalOutputSpec TXSpec { get; set; }

        UartParity Parity { get; set; }

        int UartNum { get; set; }

        UartStopBits StopBits { get; set; }

        int Rate { get; set; }

        bool Speed4x { get; set; }
    }
}
