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

		public IPulseInputConfigureCommand CreateOpenPulseInput(Component.Types.DigitalInputSpec spec, Component.Types.PulseInputClockRate rate, Component.Types.PulseInputMode mode, bool doublePrecision)
		{
			return new PulseInputConfigureCommand();
		}

		public IPulseInputConfigureCommand CreateOpenPulseInput(int pin, Component.Types.PulseInputMode mode)
		{
			return new PulseInputConfigureCommand();
		}


	}
}
