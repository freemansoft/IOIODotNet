using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class IcspResultFrom : IIcspResultFrom
    {

        public int Size { get; private set; }
        public byte[] Data { get; private set; }

        public IcspResultFrom(int size, byte[] data)
        {
            // TODO: Complete member initialization
            this.Size = size;
            this.Data = data;
        }
    }
}
