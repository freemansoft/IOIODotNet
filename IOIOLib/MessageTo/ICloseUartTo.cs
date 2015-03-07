using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    public interface ICloseUartTo : IMesssageToIOIO, IPostMessageTo
    {
        int UartNum { get; set; }
    }
}
