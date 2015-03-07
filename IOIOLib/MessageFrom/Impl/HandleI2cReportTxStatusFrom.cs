using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class HandleI2cReportTxStatusFrom : IHandleI2cReportTxStatusFrom
    {
        private int spiNum;

        public HandleI2cReportTxStatusFrom(int spiNum)
        {
            // TODO: Complete member initialization
            this.spiNum = spiNum;
        }
    }
}
