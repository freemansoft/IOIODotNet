using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOIOLib.Component.Impl
{
    /// <summary>
    /// Abstraction for this feature. See the Java version for an idea of functionality
    /// </summary>
    class PwmOutput : IPwmOutput
    {
        private Types.DigitalOutputSpec spec;
        private int freqHz;

        public PwmOutput(Types.DigitalOutputSpec spec, int freqHz)
        {
            // TODO: Complete member initialization
            this.spec = spec;
            this.freqHz = freqHz;
        }
    }
}
