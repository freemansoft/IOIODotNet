using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Impl
{
    /// <summary>
    /// Abstraction for this feature. See the Java version for an idea of functionality
    /// </summary>
    public class DigitalInput : IDigitalInput
    {
        private DigitalInputSpec spec;


        public DigitalInput(DigitalInputSpec spec)
        {
            // TODO: Complete member initialization
            this.spec = spec;
        }
    }
}
