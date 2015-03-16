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
        public bool ShouldSetDutyCycle { get; private set; }

        public  int Pin { get; private set; }

        public bool Enable { get; private set; }

        public int PwmNumber { get; private set; }

        public float DutyCycle { get; private set; }

        public int Period { get; private set; }
        public PwmScale Scale { get; private set; }

        public PwmOutputConfigureCommand(int pin, bool enable)
        {
            this.Pin = pin;
            this.Enable = enable;
            this.DutyCycle = 0.0f;
            this.ShouldSetDutyCycle = false;
            CalculatePeriodAndScale(1000);//// default in IOIO is 1Khz for sampling so maybe here too
            this.PwmNumber = 0; // should use a resource manager
        }

        public PwmOutputConfigureCommand(int pin, int freqHz)
        {
            this.Pin = pin;
            this.Enable = true;
            this.DutyCycle = 0.0f;
            this.ShouldSetDutyCycle = false;
            CalculatePeriodAndScale(freqHz);
            this.PwmNumber = 0; // should use a resource manager
        }

        public PwmOutputConfigureCommand(int pin, int freqHz, float dutyCycle)
        {
            this.Pin = pin;
            this.Enable = true;
            this.DutyCycle = dutyCycle;
            this.ShouldSetDutyCycle = true;
            CalculatePeriodAndScale(1000); //// default in IOIO is 1Khz for sampling so maybe here too
            this.PwmNumber = 0; // should use a resource manager
        }

        private void CalculatePeriodAndScale(int freqHz)
        {
            Scale = null;
            float baseUs;
            foreach (PwmScale OneScale in PwmScale.AllScales)
            {
                int clk = 16000000 / OneScale.scale;
                Period = clk / freqHz;
                if (Period <= 65536)
                {
                    baseUs = 1000000.0f / clk;
                    Scale = OneScale;
                    break;
                }
            }
            if (Scale == null)
            {
                throw new ArgumentException("Frequency too low: " + freqHz);
            }
        }

        public bool ExecuteMessage(IOIOProtocolOutgoing outBound)
        {
            outBound.setPinPwm(this.Pin, this.PwmNumber, this.Enable);
            outBound.setPwmPeriod(this.PwmNumber, this.Period, this.Scale);
            if (this.ShouldSetDutyCycle)
            {
                setPulseWidthInClocks(outBound, this.Period, this.DutyCycle);
            }
            return true;
        }

        private void setPulseWidthInClocks(IOIOProtocolOutgoing outBound, int period, float dutyCycle)
        {
            float pulseWidthInClocks = period * dutyCycle;
            if (pulseWidthInClocks > this.Period)
            {
                pulseWidthInClocks = this.Period;
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
            outBound.setPwmDutyCycle(this.PwmNumber, pulseWidth, fraction);
        }
    }
}
