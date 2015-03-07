using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    public interface IConfigureTwiMasterTo : IMesssageToIOIO, IPostMessageTo
    {
        int TwiNum { get;}
        TwiMasterRate Rate { get;}

        bool SmBus { get;}
    }
}
