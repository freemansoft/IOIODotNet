using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class IncapReportFrom : IIncapReportFrom
    {
        public int IncapNum { get; private set; }

        public int Size { get; private set; }
        public byte[] Data { get; private set; }

        public IncapReportFrom(int incapNum, int size, byte[] data)
        {
            // TODO: Complete member initialization
            this.IncapNum = incapNum;
            this.Size = size;
            this.Data = data;
        }
    }
}
