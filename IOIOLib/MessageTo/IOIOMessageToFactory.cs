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
    public class IOIOMessageToFactory
    {


        public IConfigureAnalogInputTo CreateConfigureAnalogInput(int pin)
        {
            return new ConfigureAnalogInputTo(pin);
        }

        public IConfigureAnalogInputTo CreateConfigureAnalogInput(int pin, bool notifyChange)
        {
            return new ConfigureAnalogInputTo(pin, notifyChange);
        }

        public IConfigureDigitalInputTo CreateConfigureDigitalInput(DigitalInputSpec spec)
        {
            return new ConfigureDigitalInputTo(spec);
        }

        public IConfigureDigitalInputTo CreateConfigureDigitalInput(int pin)
        {
            return new ConfigureDigitalInputTo(new DigitalInputSpec(pin));
        }

        public IConfigureDigitalInputTo CreateConfigureDigitalInput(int pin, bool trackChanges)
        {
            return new ConfigureDigitalInputTo(new DigitalInputSpec(pin), trackChanges);
        }

        public IConfigureDigitalInputTo CreateConfigureDigitalInput(int pin, Component.Types.DigitalInputSpecMode mode)
        {
            return new ConfigureDigitalInputTo(new DigitalInputSpec(pin, mode));
        }

        public IConfigureDigitalOutputTo CreateConfigureDigitalOutput(Component.Types.DigitalOutputSpec spec, bool startValue)
        {
            return new ConfigureDigitalOutputTo(spec, startValue);
        }

        public IConfigureDigitalOutputTo CreateConfigureDigitalOutput(int pin, Component.Types.DigitalOutputSpecMode mode, bool startValue)
        {
            return new ConfigureDigitalOutputTo(new DigitalOutputSpec(pin, mode), startValue);
        }

        public IConfigureDigitalOutputTo CreateConfigureDigitalOutput(int pin, bool startValue)
        {
            return new ConfigureDigitalOutputTo(new DigitalOutputSpec(pin), startValue);
        }

        public IConfigureDigitalOutputTo CreateConfigureDigitalOutput(int pin)
        {
            return new ConfigureDigitalOutputTo(new DigitalOutputSpec(pin), false);
        }


        public ISetDigitalOutputValueTo CreateSetDigitalOutputTo(int pin, bool level)
        {
            return new SetDigitalOutputValueTo(pin, level);
        }

        /******************************************************************
         * 
         * 
         * 
         * *****************************************************************/

        public IConfigurePwmOutputTo CreateOpenPwmOutput(int pin, bool enable)
        {
            return new ConfigurePwmOutputTo(pin, enable);
        }

        public IConfigurePwmOutputTo CreateOpenPwmOutput(int pin, int freqHz)
        {
            return new ConfigurePwmOutputTo(pin, freqHz);
        }

        public IConfigurePwmOutputTo CreateOpenPwmOutput(int pin, int freqHz, float dutyCycle)
        {
            return new ConfigurePwmOutputTo(pin, freqHz, dutyCycle);
        }

        public IConfigurePulseInputTo CreateOpenPulseInput(Component.Types.DigitalInputSpec spec, Component.Types.PulseInputClockRate rate, Component.Types.PulseInputMode mode, bool doublePrecision)
        {
            return new ConfigurePulseInputTo();
        }

        public IConfigurePulseInputTo CreateOpenPulseInput(int pin, Component.Types.PulseInputMode mode)
        {
            return new ConfigurePulseInputTo();
        }

        public IConfigureUartTo CreateOpenUartTo(Component.Types.DigitalInputSpec rx, Component.Types.DigitalOutputSpec tx, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
        {
            return new ConfigureUartTo(rx, tx, baud, parity, stopbits);
        }

        public IConfigureUartTo CreateOpenUartTo(int rx, int tx, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
        {
            // are these the right pull / drain settings?
            return new ConfigureUartTo(new DigitalInputSpec(rx), new DigitalOutputSpec(tx), baud, parity, stopbits);
        }

        public ICloseUartTo CreateCloseUartTo(int uartNum)
        {
            return new CloseUartTo(uartNum);
        }

        public IConfigureSpiMasterTo createOpenSpiMaster(Component.Types.DigitalInputSpec miso, Component.Types.DigitalOutputSpec mosi, Component.Types.DigitalOutputSpec clk, Component.Types.DigitalOutputSpec[] slaveSelect, Component.Types.SpiMasterConfig config)
        {
            return new ConfigureSpiMasterTo(miso, mosi, clk, slaveSelect, config);
        }

        public IConfigureSpiMasterTo createOpenSpiMaster(int miso, int mosi, int clk, int[] slaveSelect, Component.Types.SpiMasterRate rate)
        {
            DigitalOutputSpec[] slaveSelectCalc = new DigitalOutputSpec[slaveSelect.Length];
            return new ConfigureSpiMasterTo(new DigitalInputSpec(miso), new DigitalOutputSpec(mosi), new DigitalOutputSpec(clk), slaveSelectCalc, new SpiMasterConfig(rate));
        }

        public IConfigureSpiMasterTo createOpenSpiMaster(int miso, int mosi, int clk, int slaveSelect, Component.Types.SpiMasterRate rate)
        {
            DigitalOutputSpec[] slaveSelectCalc = new DigitalOutputSpec[1];
            slaveSelectCalc[0] = new DigitalOutputSpec(slaveSelect);
            return new ConfigureSpiMasterTo(new DigitalInputSpec(miso), new DigitalOutputSpec(mosi), new DigitalOutputSpec(clk), slaveSelectCalc, new SpiMasterConfig(rate));
        }


        public IConfigureTwiMasterTo createOpenTwiMaster(int twiNum, Component.Types.TwiMasterRate rate, bool smbus)
        {
            return new ConfigureTwiMasterTo(twiNum, rate, smbus);
        }

        public IConfigureIcspMasterTo createOpenIcspMaster()
        {
            return new ConfigureIcspMasterTo();
        }

        public IConfigureCapSenseTo createOpenCapSense(int pin)
        {
            return new ConfigureCapSenseTo(pin, CapSenseCoefficients.DEFAULT_COEF);
        }

        public IConfigureCapSenseTo createOpenCapSense(int pin, float filterCoef)
        {
            return new ConfigureCapSenseTo(pin, filterCoef);
        }

        public IConfigureSequencerTo createOpenSequencer(Component.Types.ISequencerChannelConfig[] config)
        {
            return new ConfigureSequencerTo(config);
        }


    }
}
