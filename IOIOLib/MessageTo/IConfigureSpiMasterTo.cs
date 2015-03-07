using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    public interface IConfigureSpiMasterTo : IMesssageToIOIO, IPostMessageTo
    {
        DigitalInputSpec Miso { get; }
        DigitalOutputSpec Mosi { get; }

        DigitalOutputSpec Clock { get; }

        DigitalOutputSpec[] SlaveSelect { get; }

        SpiMasterConfig Rate { get; }

    }
}
