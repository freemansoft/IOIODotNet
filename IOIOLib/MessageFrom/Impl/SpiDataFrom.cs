using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class SpiDataFrom : ISpiDataFrom
    {
        public int SpiNum { get; private set; }

        public int SlaveSelectPin { get; private set; }
        public byte[] Data { get; private set; }
        public int NumDataBytes { get; private set; }

        public SpiDataFrom(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            // TODO: Complete member initialization
            this.SpiNum = spiNum;
            this.SlaveSelectPin = ssPin;
            this.Data = data;
            this.NumDataBytes = dataBytes;
        }
    }
}
