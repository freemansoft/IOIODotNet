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
        public ConfigureSequencerTo(ISequencerChannelConfig[] config)
        {
            this.ChannelConfiguration = config;
        }
        public ISequencerChannelConfig[] ChannelConfiguration { get; set; }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
