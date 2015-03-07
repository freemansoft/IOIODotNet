using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    public interface IConfigureSequencerTo : IMesssageToIOIO, IPostMessageTo
    {
        ISequencerChannelConfig[] ChannelConfiguration { get; }
    }
}
