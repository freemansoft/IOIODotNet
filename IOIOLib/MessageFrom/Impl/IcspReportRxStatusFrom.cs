using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class IcspReportRxStatusFrom : IIcspReportRxStatusFrom
    {

        public int BytesRemaining { get; private set; }

        public IcspReportRxStatusFrom(int bytesRemaining)
        {
            // TODO: Complete member initialization
            this.BytesRemaining = bytesRemaining;
        }
    }
}
