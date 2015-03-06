using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOIOLib.Component.Impl
{
    /// <summary>
    /// Abstraction for this feature. See the Java version for an idea of functionality
    /// </summary>
    class CapSense : ICapSense
    {
        public static float DEFAULT_COEF = 25.0F;

        private int pin;
        private float filterCoef;

        public CapSense(int pin, float filterCoef)
        {
            // TODO: Complete member initialization
            this.pin = pin;
            this.filterCoef = filterCoef;
        }
    }
}
