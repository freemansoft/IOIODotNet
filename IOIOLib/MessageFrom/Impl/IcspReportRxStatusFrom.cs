using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class IcspReportRxStatusFrom : IIcspReportRxStatusFrom
    {
        private int bytesRemaining;

        public IcspReportRxStatusFrom(int bytesRemaining)
        {
            // TODO: Complete member initialization
            this.bytesRemaining = bytesRemaining;
        }
    }
}
