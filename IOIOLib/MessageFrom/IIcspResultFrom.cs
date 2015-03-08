using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface IIcspResultFrom : IIcspFrom
    {
        int Size { get; }
        byte[] Data { get; }
    }
}
