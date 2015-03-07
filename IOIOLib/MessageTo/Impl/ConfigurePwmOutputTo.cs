using IOIOLib.Device.Impl;
using IOIOLib.Device.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class ConfigurePwmOutputTo : IConfigurePwmOutputTo
    {
        public bool ShouldSetDutyCycle { get; private set; }

        public  int Pin { get; private set; }

        public bool Enable { get; private set; }

        public int PwmNumber { get; private set; }

        public float DutyCycle { get; private set; }

        public int Period { get; private set; }
        public PwmScale Scale { get; private set; }

        public ConfigurePwmOutputTo(int pin, bool enable)
        {
            this.Pin = pin;
            this.Enable = enable;
            this.DutyCycle = 0.0f;
            this.ShouldSetDutyCycle = false;
            CalculatePeriodAndScale(1000);//// default in IOIO is 1Khz for sampling so maybe here too
            this.PwmNumber = 0; // should use a resource manager
        }

        public ConfigurePwmOutputTo(int pin, int freqHz)
        {
            this.Pin = pin;
            this.Enable = true;
            this.DutyCycle = 0.0f;
            this.ShouldSetDutyCycle = false;
            CalculatePeriodAndScale(freqHz);
            this.PwmNumber = 0; // should use a resource manager
        }

        public ConfigurePwmOutputTo(int pin, int freqHz, float dutyCycle)
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
