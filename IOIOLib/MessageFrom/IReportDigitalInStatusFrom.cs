using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface IReportDigitalInStatusFrom : IDigitalInFrom
    {
        int Pin { get; }
        bool Level { get; }

    }
}
