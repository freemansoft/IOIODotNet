using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface ISpiDataFrom : IMessageFromIOIO
    {
        int SlaveSelectPin { get; }
        byte[] Data { get; }
        int NumDataBytes { get; }
    }
}
