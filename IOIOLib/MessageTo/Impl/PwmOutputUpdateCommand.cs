/*
 * Copyright 2011 Ytai Ben-Tsvi. All rights reserved.
 * Copyright 2015 Joe Freeman. All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 * 
 *    1. Redistributions of source code must retain the above copyright notice, this list of
 *       conditions and the following disclaimer.
 * 
 *    2. Redistributions in binary form must reproduce the above copyright notice, this list
 *       of conditions and the following disclaimer in the documentation and/or other materials
 *       provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED 'AS IS AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL ARSHAN POURSOHI OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied.
 */

using IOIOLib.Component.Types;
using IOIOLib.Device.Impl;
using IOIOLib.Device.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class PwmOutputUpdateCommand : IPwmOutputUpdateCommand
    {

        public PwmOutputSpec PwmSpec { get; private set; }

        public float DutyCycle { get; private set; }

		public int RequestedFrequency { get; private set; }

		/// <summary>
		/// calculated
		/// </summary>
		internal int CalculatedPeriod_ { get; private set; }
		/// <summary>
		/// calculated
		/// </summary>
        internal PwmScale CalculatedScale_ { get; private set; }


		/// <summary>
		/// changes the frequency but not the duty cycle
		/// changes frequency as long as passed in frequency is differnt than spec frequency
		/// </summary>
		/// <param name="spec"></param>
		/// <param name="freqHz"></param>
        public PwmOutputUpdateCommand(PwmOutputSpec spec, int freqHz)
        {
			this.PwmSpec = spec;
            this.DutyCycle = float.NaN;
			this.RequestedFrequency = freqHz;
        }

		/// <summary>
		/// changes the duty cycle but not the frequency
		/// </summary>
		/// <param name="spec"></param>
		/// <param name="dutyCycle"></param>
		public PwmOutputUpdateCommand(PwmOutputSpec spec, float dutyCycle)
		{
			if (spec.Frequency <= 0)
			{
				throw new ArgumentException("Spec Frequency must be > 0 when frequency not specified" + spec.Frequency);
			}
			this.PwmSpec = spec;
			this.DutyCycle = dutyCycle;
			this.RequestedFrequency = spec.Frequency;
		}

		/// <summary>
		/// change frequency ans duty cycle
		/// </summary>
		/// <param name="spec"></param>
		/// <param name="freqHz"></param>
		/// <param name="dutyCycle"></param>
			public PwmOutputUpdateCommand(PwmOutputSpec spec, int freqHz, float dutyCycle)
        {
			this.PwmSpec = spec;
            this.DutyCycle = dutyCycle;
			this.RequestedFrequency = freqHz;
        }

        public bool ExecuteMessage(IOIOProtocolOutgoing outBound, Device.IResourceManager rManager)
        {
			// calculate the period and scale even if not setting frequency because needed by duty cycle
			CalculatePeriodAndScale(this.RequestedFrequency);
			if (this.RequestedFrequency != PwmSpec.Frequency)
			{
				outBound.setPwmPeriod(this.PwmSpec.PwmNumber, this.CalculatedPeriod_, this.CalculatedScale_);
				// update the Pwm spec to show the current settings
				this.PwmSpec = new PwmOutputSpec(this.PwmSpec.PinSpec, this.PwmSpec.PwmNumber, this.RequestedFrequency);
			}
            if (this.DutyCycle != float.NaN)
            {
                setPulseWidthInClocks(outBound, this.CalculatedPeriod_, this.DutyCycle);
            }
            return true;
        }


		private void CalculatePeriodAndScale(int freqHz)
		{
			CalculatedScale_ = null;
			float baseUs;
			foreach (PwmScale OneScale in PwmScale.AllScales)
			{
				int clk = 16000000 / OneScale.scale;
				CalculatedPeriod_ = clk / freqHz;
				if (CalculatedPeriod_ <= 65536)
				{
					baseUs = 1000000.0f / clk;
					CalculatedScale_ = OneScale;
					break;
				}
			}
			if (CalculatedScale_ == null)
			{
				throw new ArgumentException("Frequency too low: " + freqHz);
			}
		}

		private void setPulseWidthInClocks(IOIOProtocolOutgoing outBound, int period, float dutyCycle)
        {
            float pulseWidthInClocks = period * dutyCycle;
            if (pulseWidthInClocks > this.CalculatedPeriod_)
            {
                pulseWidthInClocks = this.CalculatedPeriod_;
            }
            int pulseWidth;
            int fraction;
            pulseWidthInClocks -= 1; // period parameter is one less than the actual period length
            // yes, there is 0 and then 2 (no 1) - this is not a bug, that
            // is how the hardware PWM module works.
            if (pulseWidthInClocks < 1)
            {
                pulseWidth = 0;
                fraction = 0;
            }
            else
            {
                pulseWidth = (int)pulseWidthInClocks;
                fraction = ((int)pulseWidthInClocks * 4) & 0x03;
            }
            outBound.setPwmDutyCycle(this.PwmSpec.PwmNumber, pulseWidth, fraction);
        }
    }
}
