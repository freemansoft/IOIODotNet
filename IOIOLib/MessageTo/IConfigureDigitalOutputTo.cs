using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    public interface IConfigureDigitalOutputTo : IMesssageToIOIO, IPostMessageTo
    {
        DigitalOutputSpec Spec { get;  }
        bool StartValue { get;  }
    }
}
