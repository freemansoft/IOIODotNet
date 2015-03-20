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
		public IDigitalInputConfigureCommand CreateConfigureDigitalInput(DigitalInputSpec spec)
		{
			return new DigitalInputConfigureCommand(spec);
		}

		public IDigitalInputConfigureCommand CreateConfigureDigitalInput(int pin)
		{
			return new DigitalInputConfigureCommand(new DigitalInputSpec(pin));
		}

		public IDigitalInputConfigureCommand CreateConfigureDigitalInput(int pin, bool trackChanges)
		{
			return new DigitalInputConfigureCommand(new DigitalInputSpec(pin), trackChanges);
		}

		public IDigitalInputConfigureCommand CreateConfigureDigitalInput(int pin, Component.Types.DigitalInputSpecMode mode)
		{
			return new DigitalInputConfigureCommand(new DigitalInputSpec(pin, mode));
		}

		public IDigitaInputCloseCommand CreateCloseDigitalInput(int pin)
		{
			return new DigitalInputCloseCommand(new DigitalInputSpec(pin));
		}

		public IDigitaInputCloseCommand CreateCloseDigitalInput(DigitalInputSpec spec)
		{
			return new DigitalInputCloseCommand(spec);
		}

	}
}
