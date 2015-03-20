using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
	public class PwmOutputSpec
	{
		public DigitalOutputSpec PinSpec { get; private set; }

		public int PwmNumber { get; private set; }

		public int Frequency { get; private set; }


		/// <summary>
		/// defines a pwm pin.  immutable so have to create new ones as you configure the pin
		/// </summary>
		/// <param name="spec"></param>
		/// <param name="pwmNumber">defaults to -1</param>
		/// <param name="frequency">defaults to -1</param>
		public PwmOutputSpec(DigitalOutputSpec spec, int pwmNumber = -1, int frequency = -1)
		{
			this.PinSpec = spec;
			this.PwmNumber = pwmNumber;
			this.Frequency = frequency;
		}
	}
}
