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

		public IDigitalOutputConfigureCommand CreateConfigureDigitalOutput(Component.Types.DigitalOutputSpec spec, bool startValue)
		{
			return new DigitalOutputConfigureCommand(spec, startValue);
		}

		public IDigitalOutputConfigureCommand CreateConfigureDigitalOutput(int pin, Component.Types.DigitalOutputSpecMode mode, bool startValue)
		{
			return new DigitalOutputConfigureCommand(new DigitalOutputSpec(pin, mode), startValue);
		}

		public IDigitalOutputConfigureCommand CreateConfigureDigitalOutput(int pin, bool startValue)
		{
			return new DigitalOutputConfigureCommand(new DigitalOutputSpec(pin), startValue);
		}

		public IDigitalOutputConfigureCommand CreateConfigureDigitalOutput(int pin)
		{
			return new DigitalOutputConfigureCommand(new DigitalOutputSpec(pin), false);
		}


		/// <summary>
		/// sets the value on an already configured pin
		/// </summary>
		/// <param name="pin"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public IDigitalOutputValueSetCommand CreateSetDigitalOutputCommand(int pin, bool level)
		{
			return new DigitalOutputSetValueCommand(new DigitalOutputSpec(pin), level);
		}

        public IDigitalOutputValueSetCommand CreateSetDigitalOutputCommand(DigitalOutputSpec spec, bool level)
        {
            return new DigitalOutputSetValueCommand(spec, level);
        }

        public IDigitalOutputCloseCommand CreateCloseDigitalOutputCommand(int pin)
		{
			return new DigitalOutputCloseCommand(new DigitalOutputSpec(pin));
		}

		public IDigitalOutputCloseCommand CreateCloseDigitalOutputCommand(DigitalOutputSpec spec)
		{
			return new DigitalOutputCloseCommand(spec);
		}
	}
}
