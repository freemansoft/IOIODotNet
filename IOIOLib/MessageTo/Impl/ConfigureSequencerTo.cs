using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class ConfigureSequencerTo : IConfigureSequencerTo
    {
        public ISequencerChannelConfig[] ChannelConfiguration { get; private set; }

        internal ConfigureSequencerTo(ISequencerChannelConfig[] config)
        {
            this.ChannelConfiguration = config;
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
