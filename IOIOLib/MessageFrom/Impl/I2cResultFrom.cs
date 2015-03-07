using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class I2cResultFrom : II2cResultFrom
    {

        public int I2cNum { get; private set; }

        public int Size { get; private set; }
        public byte[] Data { get; private set; }

        internal I2cResultFrom(int i2cNum, int size, byte[] data)
        {
            this.I2cNum = i2cNum;
            this.Size = size;
            this.Data = data;
        }
    }
}
