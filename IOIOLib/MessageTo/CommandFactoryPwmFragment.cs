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

		public IPwmOutputConfigureCommand CreatePwmOutputConfigure( DigitalOutputSpec spec, bool enable)
		{
			return new PwmOutputConfigureCommand(spec, enable);
		}

		public IPwmOutputConfigureCommand CreatePwmOutputConfigure(DigitalOutputSpec spec, int freqHz)
		{
			return new PwmOutputConfigureCommand(spec, freqHz);
		}

		public IPwmOutputConfigureCommand CreatePwmOutputConfigure(DigitalOutputSpec spec, int freqHz, float dutyCycle)
		{
			return new PwmOutputConfigureCommand(spec, freqHz, dutyCycle);
		}

		public IPwmOutputUpdateCommand CreatePwmOutputUpdate(PwmOutputSpec spec, int freqHz)
		{
			return new PwmOutputUpdateCommand(spec, freqHz);
		}
		public IPwmOutputUpdateDutyCycleCommand CreatePwmOutputUpdate(PwmOutputSpec spec, int freqHz, float dutyCycle)
		{
			return new PwmOutputUpdateDutyCycleCommand(spec, freqHz, dutyCycle);
		}

		public IPwmOutputUpdateDutyCycleCommand CreatePwmDutyCycleOutputUpdate(PwmOutputSpec spec, float dutyCycle)
		{
			return new PwmOutputUpdateDutyCycleCommand(spec, dutyCycle);
		}

		public IPwmOutputUpdatePulseWidthCommand CreatePwmPulseWithOutputUpdate(PwmOutputSpec spec, float pulseWithUSec)
		{
			return new PwmOutputUpdatePulseWidthCommand(spec, pulseWithUSec);
		}

		public IPwmOutputCloseCommand CreatePwmOutputClose(PwmOutputSpec spec)
		{
			return new PwmOutputCloseCommand(spec);
		}

	}
}
