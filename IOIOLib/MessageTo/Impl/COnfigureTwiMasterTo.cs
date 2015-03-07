using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class ConfigureTwiMasterTo : IConfigureTwiMasterTo
    {
        public int TwiNum { get; private set; }
        public TwiMasterRate Rate { get; private set; }
        public bool SmBus { get; private set; }

        internal ConfigureTwiMasterTo(int twiNum, TwiMasterRate rate, bool smbus)
        {
            // TODO: Complete member initialization
            this.TwiNum = twiNum;
            this.Rate = rate;
            this.SmBus = smbus;
        }


        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
