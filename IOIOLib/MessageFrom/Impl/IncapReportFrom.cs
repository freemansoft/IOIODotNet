using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class IncapReportFrom : IIncapReportFrom
    {
        private int incapNum;
        private int size;
        private byte[] data;

        public IncapReportFrom(int incapNum, int size, byte[] data)
        {
            // TODO: Complete member initialization
            this.incapNum = incapNum;
            this.size = size;
            this.data = data;
        }
    }
}
