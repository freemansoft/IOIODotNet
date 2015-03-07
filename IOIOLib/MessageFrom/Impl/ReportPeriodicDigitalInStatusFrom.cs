using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class ReportPeriodicDigitalInStatusFrom : IReportPeriodicDigitalInStatusFrom
    {
        private int frameNum;
        private bool[] values;

        public ReportPeriodicDigitalInStatusFrom(int frameNum, bool[] values)
        {
            // TODO: Complete member initialization
            this.frameNum = frameNum;
            this.values = values;
        }
    }
}
