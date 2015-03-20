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
		public IAnalogInputConfigureCommand CreateConfigureAnalogInput(int pin)
		{
			return new AnalogInputConfigureCommand(pin);
		}

		public IAnalogInputConfigureCommand CreateConfigureAnalogInput(int pin, bool notifyChange)
		{
			return new AnalogInputConfigureCommand(pin, notifyChange);
		}

		public IAnalogInputConfigureCommand CreateCloseAnalogInput(int pin)
		{
			return new AnalogInputConfigureCommand(pin);

		}
	}
}
