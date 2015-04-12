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
		public IDigitalInputConfigureCommand CreateDigitalInputConfigure(DigitalInputSpec spec)
		{
			return new DigitalInputConfigureCommand(spec);
		}

        public IDigitalInputConfigureCommand CreateDigitalInputConfigure(DigitalInputSpec spec, bool trackChanges)
        {
            return new DigitalInputConfigureCommand(spec, trackChanges);
        }

        public IDigitalInputConfigureCommand CreateDigitalInputConfigure(int pin)
		{
			return new DigitalInputConfigureCommand(new DigitalInputSpec(pin));
		}

		public IDigitalInputConfigureCommand CreateDigitalInputConfigure(int pin, bool trackChanges)
		{
			return new DigitalInputConfigureCommand(new DigitalInputSpec(pin), trackChanges);
		}

		public IDigitalInputConfigureCommand CreateDigitalInputConfigure(int pin, Component.Types.DigitalInputSpecMode mode)
		{
			return new DigitalInputConfigureCommand(new DigitalInputSpec(pin, mode));
		}

		public IDigitaInputCloseCommand CreateDigitalInputClose(int pin)
		{
			return new DigitalInputCloseCommand(new DigitalInputSpec(pin));
		}

		public IDigitaInputCloseCommand CreateDigitalInputClose(DigitalInputSpec spec)
		{
			return new DigitalInputCloseCommand(spec);
		}

	}
}
