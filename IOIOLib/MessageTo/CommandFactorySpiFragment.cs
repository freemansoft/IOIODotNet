using IOIOLib.Component.Types;
using IOIOLib.MessageTo.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
	 public partial class IOIOMessageCommandFactory
	{

		public ISpiMasterConfigureCommand createOpenSpiMaster(Component.Types.DigitalInputSpec miso, Component.Types.DigitalOutputSpec mosi, Component.Types.DigitalOutputSpec clk, Component.Types.DigitalOutputSpec[] slaveSelect, Component.Types.SpiMasterConfig config)
		{
			return new SpiMasterConfigureCommand(miso, mosi, clk, slaveSelect, config);
		}

		public ISpiMasterConfigureCommand createOpenSpiMaster(int miso, int mosi, int clk, int[] slaveSelect, Component.Types.SpiMasterRate rate)
		{
			DigitalOutputSpec[] slaveSelectCalc = new DigitalOutputSpec[slaveSelect.Length];
			return new SpiMasterConfigureCommand(new DigitalInputSpec(miso), new DigitalOutputSpec(mosi), new DigitalOutputSpec(clk), slaveSelectCalc, new SpiMasterConfig(rate));
		}

		public ISpiMasterConfigureCommand createOpenSpiMaster(int miso, int mosi, int clk, int slaveSelect, Component.Types.SpiMasterRate rate)
		{
			DigitalOutputSpec[] slaveSelectCalc = new DigitalOutputSpec[1];
			slaveSelectCalc[0] = new DigitalOutputSpec(slaveSelect);
			return new SpiMasterConfigureCommand(new DigitalInputSpec(miso), new DigitalOutputSpec(mosi), new DigitalOutputSpec(clk), slaveSelectCalc, new SpiMasterConfig(rate));
		}



	}
}
