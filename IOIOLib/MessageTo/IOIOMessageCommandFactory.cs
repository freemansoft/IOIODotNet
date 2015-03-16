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
using IOIOLib.Device.Types;
using IOIOLib.MessageTo.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    /// <summary>
    /// This is THE PlACE to create outbound messages
    /// This SHOULD manage the resource pools
    /// </summary>
    public class IOIOMessageCommandFactory
    {


        public IAnalogInputConfigureCommand CreateConfigureAnalogInput(int pin)
        {
            return new AnalogInputConfigureCommand(pin);
        }

        public IAnalogInputConfigureCommand CreateConfigureAnalogInput(int pin, bool notifyChange)
        {
            return new AnalogInputConfigureCommand(pin, notifyChange);
        }

        public IDigitalInputConfigureCommand CreateConfigureDigitalInput(DigitalInputSpec spec)
        {
            return new DigitalInputConfigureCommand(spec);
        }

        public IDigitalInputConfigureCommand CreateConfigureDigitalInput(int pin)
        {
            return new DigitalInputConfigureCommand(new DigitalInputSpec(pin));
        }

        public IDigitalInputConfigureCommand CreateConfigureDigitalInput(int pin, bool trackChanges)
        {
            return new DigitalInputConfigureCommand(new DigitalInputSpec(pin), trackChanges);
        }

        public IDigitalInputConfigureCommand CreateConfigureDigitalInput(int pin, Component.Types.DigitalInputSpecMode mode)
        {
            return new DigitalInputConfigureCommand(new DigitalInputSpec(pin, mode));
        }

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


        public IDigitalOutputValueSetCommand CreateSetDigitalOutputCommand(int pin, bool level)
        {
            return new DigitalOutputSetValueCommand(pin, level);
        }

        /******************************************************************
         * 
         * 
         * 
         * *****************************************************************/

        public IPwmOutputConfigureCommand CreateOpenPwmOutput(int pin, bool enable)
        {
            return new PwmOutputConfigureCommand(pin, enable);
        }

        public IPwmOutputConfigureCommand CreateOpenPwmOutput(int pin, int freqHz)
        {
            return new PwmOutputConfigureCommand(pin, freqHz);
        }

        public IPwmOutputConfigureCommand CreateOpenPwmOutput(int pin, int freqHz, float dutyCycle)
        {
            return new PwmOutputConfigureCommand(pin, freqHz, dutyCycle);
        }

        public IPulseInputConfigureCommand CreateOpenPulseInput(Component.Types.DigitalInputSpec spec, Component.Types.PulseInputClockRate rate, Component.Types.PulseInputMode mode, bool doublePrecision)
        {
            return new PulseInputConfigureCommand();
        }

        public IPulseInputConfigureCommand CreateOpenPulseInput(int pin, Component.Types.PulseInputMode mode)
        {
            return new PulseInputConfigureCommand();
        }

        public IUartConfigureCommand CreateOpenUart(Component.Types.DigitalInputSpec rx, Component.Types.DigitalOutputSpec tx, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
        {
            return new UartConfigureCommand(rx, tx, baud, parity, stopbits);
        }

        public IUartConfigureCommand CreateOpenUart(int rx, int tx, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
        {
            // are these the right pull / drain settings?
            return new UartConfigureCommand(new DigitalInputSpec(rx), new DigitalOutputSpec(tx), baud, parity, stopbits);
        }

        public IUartCloseCommand CreateCloseUart(int uartNum)
        {
            return new UartCloseCommand(uartNum);
        }

        public ISpiMasterConfigureCommand createOpenSpiMaster(Component.Types.DigitalInputSpec miso, Component.Types.DigitalOutputSpec mosi, Component.Types.DigitalOutputSpec clk, Component.Types.DigitalOutputSpec[] slaveSelect, Component.Types.SpiMasterConfig config)
        {
            return new SpiMasterConfigureCommand(miso, mosi, clk, slaveSelect, config);
        }

        public ISpiMasterConfigureCommand createOpenSpiMaster(int miso, int mosi, int clk, int[] slaveSelect, Component.Types.SpiMasterRate rate)
        {
            DigitalOutputSpec[] slaveSelectCalc = new DigitalOutputSpec[slaveSelect.Length];
            return new SpiMasterConfigureCommand(new DigitalInputSpec(miso), new DigitalOutputSpec(mosi), new DigitalOutputSpec(clk), slaveSelectCalc, new SpiMasterConfig(rate));
        }

        public ISpiMasterConfigureCommand createOpenSpiMaster(int miso, int mosi, int clk, int slaveSelect, Component.Types.SpiMasterRate rate)
        {
            DigitalOutputSpec[] slaveSelectCalc = new DigitalOutputSpec[1];
            slaveSelectCalc[0] = new DigitalOutputSpec(slaveSelect);
            return new SpiMasterConfigureCommand(new DigitalInputSpec(miso), new DigitalOutputSpec(mosi), new DigitalOutputSpec(clk), slaveSelectCalc, new SpiMasterConfig(rate));
        }


        public ITwiMasterConfigureCommand createOpenTwiMaster(int twiNum, Component.Types.TwiMasterRate rate, bool smbus)
        {
            return new TwiMasterConfigureCommand(twiNum, rate, smbus);
        }

        public IIcspMasterConfigureCommand createOpenIcspMaster()
        {
            return new IcspMasterConfigureCommand();
        }

        public ICapSenseConfigureCommand createOpenCapSense(int pin)
        {
            return new CapSenseConfigureCommand(pin, CapSenseCoefficients.DEFAULT_COEF);
        }

        public ICapSenseConfigureCommand createOpenCapSense(int pin, float filterCoef)
        {
            return new CapSenseConfigureCommand(pin, filterCoef);
        }

        public ISequencerConfigureCommand createOpenSequencer(Component.Types.ISequencerChannelConfig[] config)
        {
            return new SequencerConfigureCommand(config);
        }


    }
}
