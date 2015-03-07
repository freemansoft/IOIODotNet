using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class IcspResultFrom : IIcspResultFrom
    {
        private int size;
        private byte[] data;

        public IcspResultFrom(int size, byte[] data)
        {
            // TODO: Complete member initialization
            this.size = size;
            this.data = data;
        }
    }
}
