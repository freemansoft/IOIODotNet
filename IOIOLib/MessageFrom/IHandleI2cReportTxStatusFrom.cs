using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface IHandleI2cReportTxStatusFrom : IMessageFromIOIO
    {
        int I2cNum { get; }
    }
}
