using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface IIcspReportRxStatusFrom : IMessageFromIOIO
    {
        int BytesRemaining { get; }
    }
}
