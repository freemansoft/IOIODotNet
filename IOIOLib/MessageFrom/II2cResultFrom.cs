using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface II2cResultFrom : II2cFrom
    {
        int Size { get; }
        byte[] Data { get; }
    }
}
