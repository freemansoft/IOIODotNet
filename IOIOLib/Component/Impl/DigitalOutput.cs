using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOIOLib.Component.Impl
{
    /// <summary>
    /// Abstraction for this feature. See the Java version for an idea of functionality
    /// </summary>
    public class DigitalOutput : IDigitalOutput
    {
        private Types.DigitalOutputSpec spec;
        private bool startValue;

        public DigitalOutput(Types.DigitalOutputSpec spec, bool startValue)
        {
            // TODO: Complete member initialization
            this.spec = spec;
            this.startValue = startValue;
        }
    }
}
