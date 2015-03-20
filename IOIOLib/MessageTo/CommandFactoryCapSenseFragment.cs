using IOIOLib.Component.Types;
using IOIOLib.Device.Types;
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

		public ICapSenseConfigureCommand createOpenCapSense(int pin)
		{
			return new CapSenseConfigureCommand(pin, CapSenseCoefficients.DEFAULT_COEF);
		}

		public ICapSenseConfigureCommand createOpenCapSense(int pin, float filterCoef)
		{
			return new CapSenseConfigureCommand(pin, filterCoef);
		}



	}
}
