using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface ICapSenseReportFrom : IMessageFromIOIO
    {
        int PinNum { get; }
        int Value { get; }
    }
}
