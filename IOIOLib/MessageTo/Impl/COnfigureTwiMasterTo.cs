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

        public ConfigureTwiMasterTo(int twiNum, TwiMasterRate rate, bool smbus)
        {
            // TODO: Complete member initialization
            this.TwiNum = twiNum;
            this.Rate = rate;
            this.SmBus = smbus;
        }
        public int TwiNum { get; set; }
        public TwiMasterRate Rate { get; set; }
        public bool SmBus { get; set; }


        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
