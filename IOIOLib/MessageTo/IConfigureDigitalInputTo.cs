using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    public interface IConfigureDigitalInputTo : IMesssageToIOIO, IPostMessageTo
    {
        DigitalInputSpec Spec { get; }

        Boolean? ChangeNotify { get; }
    }
}
