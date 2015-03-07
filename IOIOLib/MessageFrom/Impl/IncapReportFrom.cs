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

        internal IncapReportFrom(int incapNum, int size, byte[] data)
        {
            this.IncapNum = incapNum;
            this.Size = size;
            this.Data = data;
        }
    }
}
