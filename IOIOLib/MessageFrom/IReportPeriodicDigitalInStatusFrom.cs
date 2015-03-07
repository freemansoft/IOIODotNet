﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface IReportPeriodicDigitalInStatusFrom : IMessageFromIOIO
    {
        int FrameNum { get; }
        bool[] Values { get; }
    }
}
