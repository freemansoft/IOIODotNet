using IOIOLib.Device.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class SequencerEventFrom : ISequencerEventFrom
    {

        public Device.Types.SequencerEvent SeqEvent { get; private set; }
        public int Identifier { get; private set; }
        public SequencerEventFrom(Device.Types.SequencerEvent seqEvent, int arg)
        {
            // TODO: Complete member initialization
            this.SeqEvent = seqEvent;
            this.Identifier = arg;
        }
    }
}
