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
    public class PwmOutputConfigureCommand : IPwmOutputConfigureCommand
    {

        public PwmOutputSpec PwmDef { get; private set; }

        public bool Enable { get; private set; }


        public float DutyCycle { get; private set; }

		public int RequestedFrequency { get; private set; }

        public PwmOutputConfigureCommand(DigitalOutputSpec spec, bool enable)
        {

			this.PwmDef = new PwmOutputSpec(spec) ;
            this.Enable = enable;
            this.DutyCycle = float.NaN;
			this.RequestedFrequency = 1000;			// default in IOIO is 1Khz for sampling so maybe here too
        }

        public PwmOutputConfigureCommand(DigitalOutputSpec spec, int freqHz)
        {
			this.PwmDef = new PwmOutputSpec(spec);
			this.Enable = true;
            this.DutyCycle = float.NaN;
			this.RequestedFrequency = freqHz;
        }

		/// <summary>
		/// TODO add PeriodUSec to this constructor
		/// </summary>
		/// <param name="spec"></param>
		/// <param name="freqHz"></param>
		/// <param name="dutyCycle"></param>
        public PwmOutputConfigureCommand(DigitalOutputSpec spec, int freqHz, float dutyCycle)
        {
			this.PwmDef = new PwmOutputSpec(spec);
			this.Enable = true;
            this.DutyCycle = dutyCycle;
			this.RequestedFrequency = freqHz;
        }

        public bool ExecuteMessage(IOIOProtocolOutgoing outBound)
        {
			outBound.setPinDigitalOut(this.PwmDef.PinSpec.Pin, false, this.PwmDef.PinSpec.Mode);
            outBound.setPinPwm(this.PwmDef.PinSpec.Pin, this.PwmDef.PwmNumber, this.Enable);

			IPwmOutputUpdateCommand updateCommand;
			// this should accepted periodUSec also
			if (!float.IsNaN(this.DutyCycle))
			{
				updateCommand = new PwmOutputUpdateDutyCycleCommand(this.PwmDef, this.RequestedFrequency, this.DutyCycle);
			}
			else
			{
				updateCommand = new PwmOutputUpdateCommand(this.PwmDef, this.RequestedFrequency);
			}
			updateCommand.ExecuteMessage(outBound);
			// retain any frequency change done by the update command
			this.PwmDef = updateCommand.PwmDef; 

            return true;
        }

		public bool Alloc(Device.IResourceManager rManager)
		{
			Resource outPin = new Resource(ResourceType.PIN, this.PwmDef.PinSpec.Pin);
			Resource pwm = new Resource(ResourceType.OUTCOMPARE);
			rManager.Alloc(outPin);
			rManager.Alloc(pwm);		// acquires the pwm number
			// retain the PWM number
			this.PwmDef = new PwmOutputSpec(this.PwmDef.PinSpec, pwm.Id_);

			return true;
		}
	}
}
