using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    public interface IConfigureAnalogInputTo : IMesssageToIOIO, IPostMessageTo
    {
        int Pin { get; }

        Boolean? ChangeNotify { get; }
    }
}
