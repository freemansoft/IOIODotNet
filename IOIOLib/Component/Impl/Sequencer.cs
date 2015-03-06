using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOIOLib.Component.Impl
{
    /// <summary>
    /// Abstraction for this feature. See the Java version for an idea of functionality
    /// </summary>
    class Sequencer : ISequencer
    {
        private Types.ISequencerChannelConfig[] config;

        public Sequencer(Types.ISequencerChannelConfig[] config)
        {
            // TODO: Complete member initialization
            this.config = config;
        }
    }
}
