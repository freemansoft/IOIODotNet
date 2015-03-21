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

		public IPwmOutputConfigureCommand CreateConfigurePwmOutput( DigitalOutputSpec spec, bool enable)
		{
			return new PwmOutputConfigureCommand(spec, enable);
		}

		public IPwmOutputConfigureCommand CreateConfigurePwmOutput(DigitalOutputSpec spec, int freqHz)
		{
			return new PwmOutputConfigureCommand(spec, freqHz);
		}

		public IPwmOutputConfigureCommand CreateConfigurePwmOutput(DigitalOutputSpec spec, int freqHz, float dutyCycle)
		{
			return new PwmOutputConfigureCommand(spec, freqHz, dutyCycle);
		}

		public IPwmOutputUpdateCommand CreateUpdatePwmOutput(PwmOutputSpec spec, int freqHz)
		{
			return new PwmOutputUpdateCommand(spec, freqHz);
		}
		public IPwmOutputUpdateCommand CreateUpdatePwmOutput(PwmOutputSpec spec, int freqHz, float dutyCycle)
		{
			return new PwmOutputUpdateCommand(spec, freqHz, dutyCycle);
		}

		public IPwmOutputUpdateCommand CreateUpdatePwmOutput(PwmOutputSpec spec, float dutyCycle)
		{
			return new PwmOutputUpdateCommand(spec, dutyCycle);
		}

		public IPwmOutputCloseCommand CreateClosePwmOutput(PwmOutputSpec spec)
		{
			return new PwmOutputCloseCommand(spec);
		}

	}
}
