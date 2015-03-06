using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOIOLib.Component.Impl
{
    /// <summary>
    /// Abstraction for this feature. See the Java version for an idea of functionality
    /// </summary>
    public class AnalogInput : IAnalogInput
    {
        private int pin;

        public AnalogInput(int pin)
        {
            // TODO: Complete member initialization
            this.pin = pin;
        }
    }
}
