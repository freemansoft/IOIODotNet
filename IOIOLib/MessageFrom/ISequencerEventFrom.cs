using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface ISequencerEventFrom : IMessageFromIOIO
    {
        Device.Types.SequencerEvent SeqEvent { get; }
        int Identifier { get; }
    }
}
