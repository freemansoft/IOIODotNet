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

		public IPwmOutputConfigureCommand CreateOpenPwmOutput( DigitalOutputSpec spec, bool enable)
		{
			return new PwmOutputConfigureCommand(spec, enable);
		}

		public IPwmOutputConfigureCommand CreateOpenPwmOutput(DigitalOutputSpec spec, int freqHz)
		{
			return new PwmOutputConfigureCommand(spec, freqHz);
		}

		public IPwmOutputConfigureCommand CreateOpenPwmOutput(DigitalOutputSpec spec, int freqHz, float dutyCycle)
		{
			return new PwmOutputConfigureCommand(spec, freqHz, dutyCycle);
		}

		public IPwmOutputCloseCommand CreateClosePwmOutput(PwmOutputSpec spec)
		{
			return new PwmOutputCloseCommand(spec);
		}

	}
}
