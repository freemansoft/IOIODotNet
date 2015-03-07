using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class ReportPeriodicDigitalInStatusFrom : IReportPeriodicDigitalInStatusFrom
    {
        public int FrameNum { get; private set; }
        public bool[] Values { get; private set; }

        internal ReportPeriodicDigitalInStatusFrom(int frameNum, bool[] values)
        {
            this.FrameNum = frameNum;
            this.Values = values;
        }
    }
}
