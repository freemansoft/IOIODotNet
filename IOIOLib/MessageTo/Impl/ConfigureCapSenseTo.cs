using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class ConfigureCapSenseTo : IConfigureCapSenseTo
    {
        public int Pin { get; private set; }
        public float FilterCoefficent { get; private set; }

        internal ConfigureCapSenseTo(int pin, float filterCoefficent)
        {
            this.Pin = pin;
            this.FilterCoefficent = filterCoefficent;
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
