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
	/// <summary>
	///  exists so that we can have same constructors for DutyCycle and PulseWidth constructors
	/// </summary>
    public class PwmOutputUpdateDutyCycleCommand : PwmOutputUpdateCommand, IPwmOutputUpdateDutyCycleCommand
    {



		/// <summary>
		/// changes the duty cycle but not the frequency
		/// </summary>
		/// <param name="spec"></param>
		/// <param name="dutyCycle"></param>
		public PwmOutputUpdateDutyCycleCommand(PwmOutputSpec spec, float dutyCycle)
		{
			if (spec.Frequency <= 0)
			{
				throw new ArgumentException("Spec Frequency must be > 0 when frequency not specified" + spec.Frequency);
			}
			this.PwmDef = spec;
			this.DutyCycle = dutyCycle;
			this.PulseWidthUSec = float.NaN;
			this.RequestedFrequency = spec.Frequency; 
		}

		/// <summary>
		/// change frequency ans duty cycle
		/// </summary>
		/// <param name="spec"></param>
		/// <param name="freqHz"></param>
		/// <param name="dutyCycle"></param>
		public PwmOutputUpdateDutyCycleCommand(PwmOutputSpec spec, int freqHz, float dutyCycle)
		{
			if (freqHz <= 0)
			{
				throw new ArgumentException("Frequency must be > 0 when frequency specified" + freqHz);
			}
			this.PwmDef = spec;
			this.DutyCycle = dutyCycle;
			this.RequestedFrequency = freqHz;
			this.PulseWidthUSec = float.NaN;
		}
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
