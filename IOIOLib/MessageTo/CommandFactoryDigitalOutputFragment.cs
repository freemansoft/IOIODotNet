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

		public IDigitalOutputConfigureCommand CreateDigitalOutputConfigure(Component.Types.DigitalOutputSpec spec, bool startValue)
		{
			return new DigitalOutputConfigureCommand(spec, startValue);
		}

		public IDigitalOutputConfigureCommand CreateDigitalOutputConfigure(int pin, Component.Types.DigitalOutputSpecMode mode, bool startValue)
		{
			return new DigitalOutputConfigureCommand(new DigitalOutputSpec(pin, mode), startValue);
		}

		public IDigitalOutputConfigureCommand CreateDigitalOutputConfigure(int pin, bool startValue)
		{
			return new DigitalOutputConfigureCommand(new DigitalOutputSpec(pin), startValue);
		}

		public IDigitalOutputConfigureCommand CreateDigitalOutputConfigure(int pin)
		{
			return new DigitalOutputConfigureCommand(new DigitalOutputSpec(pin), false);
		}


		/// <summary>
		/// sets the value on an already configured pin
		/// </summary>
		/// <param name="pin"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public IDigitalOutputValueSetCommand CreateDigitalOutputCommandSet(int pin, bool level)
		{
			return new DigitalOutputSetValueCommand(new DigitalOutputSpec(pin), level);
		}

        public IDigitalOutputValueSetCommand CreateDigitalOutputCommandSet(DigitalOutputSpec spec, bool level)
        {
            return new DigitalOutputSetValueCommand(spec, level);
        }

        public IDigitalOutputCloseCommand CreateDigitalOutputCommandClose(int pin)
		{
			return new DigitalOutputCloseCommand(new DigitalOutputSpec(pin));
		}

		public IDigitalOutputCloseCommand CreateDigitalOutputCommandClose(DigitalOutputSpec spec)
		{
			return new DigitalOutputCloseCommand(spec);
		}
	}
}
