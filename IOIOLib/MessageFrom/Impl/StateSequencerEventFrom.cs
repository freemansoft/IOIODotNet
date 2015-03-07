using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class StateSequencerEventFrom : IStateSequencerEventFrom
    {
        private Device.Types.SequencerEvent seqEvent;
        private int arg;

        public StateSequencerEventFrom(Device.Types.SequencerEvent seqEvent, int arg)
        {
            // TODO: Complete member initialization
            this.seqEvent = seqEvent;
            this.arg = arg;
        }
    }
}
