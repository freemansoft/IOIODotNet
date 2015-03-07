using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class ConfigureDigitalOutputTo : IConfigureDigitalOutputTo
    {

        public DigitalOutputSpec Spec { get; private set; }
        public bool StartValue { get; private set; }

        internal ConfigureDigitalOutputTo(Component.Types.DigitalOutputSpec spec, bool startValue)
        {
            this.Spec = spec;
            this.StartValue = startValue;
        }

        public ConfigureDigitalOutputTo(Component.Types.DigitalOutputSpec spec)
        {
            this.Spec = spec;
            this.StartValue = false;
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            outBound.setPinDigitalOut(this.Spec.pin, this.StartValue, this.Spec.mode);
            return true;
        }
    }
}
