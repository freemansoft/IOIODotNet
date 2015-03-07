using IOIOLib.Device.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    public interface IConfigurePwmOutputTo : IMesssageToIOIO, IPostMessageTo
    {

        bool ShouldSetDutyCycle { get; }

        int Pin { get; }

        bool Enable { get; }

        int PwmNumber { get; }

        float DutyCycle { get; }

        int Period { get; }
        PwmScale Scale { get; }
    }
}
