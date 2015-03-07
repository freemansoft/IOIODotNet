using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class StateHandleI2cReportTxStatusFrom : IStateHandleI2cReportTxStatusFrom
    {
        private int spiNum;

        public StateHandleI2cReportTxStatusFrom(int spiNum)
        {
            // TODO: Complete member initialization
            this.spiNum = spiNum;
        }
    }
}
