﻿using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class ConfigureSpiMasterTo : IConfigureSpiMasterTo
    {

        /// <summary>
        /// Do we even need all these parameters?  Isn't there only one SpiMaster on the board
        /// </summary>
        /// <param name="miso"></param>
        /// <param name="mosi"></param>
        /// <param name="clock"></param>
        /// <param name="slaveSelect"></param>
        /// <param name="rate"></param>
        public ConfigureSpiMasterTo(DigitalInputSpec miso, DigitalOutputSpec mosi, DigitalOutputSpec clock, DigitalOutputSpec[] slaveSelect, SpiMasterConfig rate)
        {
            this.Miso = miso;
            this.Mosi = mosi;
            this.Clock = Clock;
            this.SlaveSelect = slaveSelect;
            this.Rate = rate;
            throw new NotImplementedException("Post(IOpenSpiMasterTo) not tied together in outgoing protocol");
        }
        public DigitalInputSpec Miso { get; set; }
        public DigitalOutputSpec Mosi { get; set; }

        public DigitalOutputSpec Clock { get; set; }

        public DigitalOutputSpec[] SlaveSelect { get; set; }

        public SpiMasterConfig Rate { get; set; }



        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
