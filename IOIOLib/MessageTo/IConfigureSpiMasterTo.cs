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
        DigitalInputSpec Miso { get; set; }
        DigitalOutputSpec Mosi { get; set; }

        DigitalOutputSpec Clock { get; set; }

        DigitalOutputSpec[] SlaveSelect { get; set; }

        SpiMasterConfig Rate { get; set; }

    }
}
