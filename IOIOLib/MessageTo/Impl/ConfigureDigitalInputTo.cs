using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class ConfigureDigitalInputTo : IConfigureDigitalInputTo
    {
        public DigitalInputSpec Spec { get; private set; }

        public Boolean? ChangeNotify { get; private set; }


        internal ConfigureDigitalInputTo(DigitalInputSpec digitalInputSpec)
        {
            this.Spec = digitalInputSpec;
            ChangeNotify = null;
        }

        /// <summary>
        /// Create an inbound digital port that notifies on state change
        /// </summary>
        /// <param name="digitalInputSpec"></param>
        /// <param name="notifyOnChange"></param>
        public ConfigureDigitalInputTo(DigitalInputSpec digitalInputSpec, bool notifyOnChange)
        {
            this.Spec = digitalInputSpec;
            ChangeNotify = notifyOnChange;
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            outBound.setPinDigitalIn(this.Spec.pin, this.Spec.mode);
            if (ChangeNotify.HasValue && ChangeNotify.Value)
            {
                outBound.setChangeNotify(this.Spec.pin, ChangeNotify.Value);
            }
            return true;

        }
    }
}
