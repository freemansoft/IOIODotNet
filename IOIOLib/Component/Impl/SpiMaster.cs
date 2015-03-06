using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOIOLib.Component.Impl
{
    /// <summary>
    /// Abstraction for this feature. See the Java version for an idea of functionality
    /// </summary>
    public class SpiMaster : ISpiMaster
    {
        private Types.DigitalInputSpec miso;
        private Types.DigitalOutputSpec mosi;
        private Types.DigitalOutputSpec clk;
        private Types.DigitalOutputSpec[] slaveSelect;
        private Types.SpiMasterConfig config;

        public SpiMaster(Types.DigitalInputSpec miso, Types.DigitalOutputSpec mosi, Types.DigitalOutputSpec clk, Types.DigitalOutputSpec[] slaveSelect, Types.SpiMasterConfig config)
        {
            // TODO: Complete member initialization
            this.miso = miso;
            this.mosi = mosi;
            this.clk = clk;
            this.slaveSelect = slaveSelect;
            this.config = config;
        }
    }
}
