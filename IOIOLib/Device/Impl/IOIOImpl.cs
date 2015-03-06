using IOIOLib.Component;
using IOIOLib.Component.Impl;
using IOIOLib.Component.Types;
using IOIOLib.Connection;
using IOIOLib.Device.Types;
using IOIOLib.IOIOException;
using IOIOLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// This class will SOMEDAY act as an object oriented wrapper around the IOIO boards.
    /// </summary>
    class IOIOImpl : IOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOImpl));

        private IOIOConnection Conn;
        /// <summary>
        /// TODO Need to get on this and make state be correct!
        /// </summary>
        private IOIOState State = IOIOState.INIT;
        private IOIOProtocolOutgoing OutProt;
        private IOIOProtocolIncoming InProt;
        private IOIOIncomingHandler InboundHandler;
        private IOIOIncomingHandlerCaptureState InbountStateCapture;
        private IOIOIncomingHandlerCaptureLog InboundCaptureAndLog;


        public IOIOImpl(IOIOConnection conn)
        {
            this.Conn = conn;
        }


        /// <summary>
        /// This method is not yet finished.
        /// </summary>
        public void waitForConnect()
        {
            Conn.waitForConnect();
            InbountStateCapture = new IOIOIncomingHandlerCaptureState();
            InboundCaptureAndLog = new IOIOIncomingHandlerCaptureLog(10);
            InboundHandler = new IOIOIncomingHandlerDistributor(
                new List<IOIOIncomingHandler> { InbountStateCapture, InboundCaptureAndLog });

            OutProt = new IOIOProtocolOutgoing(this.Conn.getOutputStream());
            InProt = new IOIOProtocolIncoming(this.Conn.getInputStream(), this.InboundHandler);
            //Joe's COM4 @ 115200 spits out HW,BootLoader,InterfaceVersion: IOIOSPRK0016IOIO0311IOIO0500
            initBoardVersion();
            //checkInterfaceVersion();
        }

        /// <summary>
        /// Verify we are connected and set our state accordingly
        /// </summary>
        private void initBoardVersion()
        {
            // hack until we figure out where state should be and how we accesses
            // Should this build the hardware object and retain it instead of doing it in the handler?
            // the inbound handler actually has already processed the board version.  
            if (InbountStateCapture.OurHardware == null)
            {
                State = IOIOState.DEAD;
            }
            else
            {
                State = IOIOState.CONNECTED;
            }
            LOG.Info("Hardware is " + InbountStateCapture.OurHardware);
        }

        /// <summary>
        /// This method is not yet finished.
        /// This is really an outbound call to the IOIO.  
        /// It will tell us someitme later if it is the right version
        /// </summary>
        private void checkInterfaceVersion()
        {
            OutProt.checkInterfaceVersion();
            LOG.Warn("checkInterfaceVersion should wait for and check the response");
            //State = IOIOState.INCOMPATIBLE;
        }

        public void disconnect()
        {
            throw new NotImplementedException();
        }

        public void waitForDisconnect()
        {
            throw new NotImplementedException();
        }

        public IOIOState getState()
        {
            return State;
        }

        public void softReset()
        {
            throw new NotImplementedException();
        }

        public void hardReset()
        {
            throw new NotImplementedException();
        }

        public string getImplVersion(IOIOVersionType v)
        {
            if (this.State == IOIOState.INIT)
            {
                throw new IllegalStateException(
                        "Connection has not yet been established");
            }
            switch (v)
            {
                case IOIOVersionType.HARDWARE_VER:
                    return this.InbountStateCapture.HardwareId;
                case IOIOVersionType.BOOTLOADER_VER:
                    return this.InbountStateCapture.BootloaderId;
                case IOIOVersionType.APP_FIRMWARE_VER:
                    return this.InbountStateCapture.FirmwareId;
                case IOIOVersionType.IOIOLIB_VER:
                    return "IOIO0504";
            }
            return null;
        }

        public Component.IDigitalInput openDigitalInput(DigitalInputSpec spec)
        {
            return new DigitalInput(spec);
        }

        public Component.IDigitalInput openDigitalInput(int pin)
        {
            return new Component.Impl.DigitalInput(new DigitalInputSpec(pin));
        }

        public Component.IDigitalInput openDigitalInput(int pin, Component.Types.DigitalInputSpecMode mode)
        {
            return new Component.Impl.DigitalInput(new DigitalInputSpec(pin, mode));
        }

        public Component.IDigitalOutput openDigitalOutput(Component.Types.DigitalOutputSpec spec, bool startValue)
        {
            return new Component.Impl.DigitalOutput(spec, startValue);
            throw new NotImplementedException();
        }

        public Component.IDigitalOutput openDigitalOutput(int pin, Component.Types.DigitalOutputSpecMode mode, bool startValue)
        {
            return new Component.Impl.DigitalOutput(new DigitalOutputSpec(pin, mode), startValue);
        }

        public Component.IDigitalOutput openDigitalOutput(int pin, bool startValue)
        {
            return new Component.Impl.DigitalOutput(new DigitalOutputSpec(pin), startValue);
        }

        public Component.IDigitalOutput openDigitalOutput(int pin)
        {
            return new Component.Impl.DigitalOutput(new DigitalOutputSpec(pin), false);
        }

        public Component.IAnalogInput openAnalogInput(int pin)
        {
            return new Component.Impl.AnalogInput(pin);
        }

        public Component.IPwmOutput openPwmOutput(Component.Types.DigitalOutputSpec spec, int freqHz)
        {
            return new Component.Impl.PwmOutput(spec, freqHz);
        }

        public Component.IPwmOutput openPwmOutput(int pin, int freqHz)
        {
            return new Component.Impl.PwmOutput(new DigitalOutputSpec(pin), freqHz);
        }

        public Component.IPulseInput openPulseInput(Component.Types.DigitalInputSpec spec, Component.Types.PulseInputClockRate rate, Component.Types.PulseInputMode mode, bool doublePrecision)
        {
            throw new NotImplementedException();
        }

        public Component.IPulseInput openPulseInput(int pin, Component.Types.PulseInputMode mode)
        {
            throw new NotImplementedException();
        }

        public Component.IUart openUart(Component.Types.DigitalInputSpec rx, Component.Types.DigitalOutputSpec tx, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
        {
            return new Component.Impl.Uart(rx, tx, baud, parity, stopbits);
            throw new NotImplementedException();
        }

        public Component.IUart openUart(int rx, int tx, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
        {
            return new Component.Impl.Uart(new DigitalInputSpec(rx), new DigitalOutputSpec(tx), baud, parity, stopbits);
        }

        public Component.ISpiMaster openSpiMaster(Component.Types.DigitalInputSpec miso, Component.Types.DigitalOutputSpec mosi, Component.Types.DigitalOutputSpec clk, Component.Types.DigitalOutputSpec[] slaveSelect, Component.Types.SpiMasterConfig config)
        {
            return new Component.Impl.SpiMaster(miso, mosi, clk, slaveSelect, config);
        }

        public Component.ISpiMaster openSpiMaster(int miso, int mosi, int clk, int[] slaveSelect, Component.Types.SpiMasterRate rate)
        {
            DigitalOutputSpec[] slaveSelectCalc = new DigitalOutputSpec[slaveSelect.Length];
            return new Component.Impl.SpiMaster(new DigitalInputSpec(miso), new DigitalOutputSpec(mosi), new DigitalOutputSpec(clk), slaveSelectCalc, new SpiMasterConfig(rate));
        }

        public Component.ISpiMaster openSpiMaster(int miso, int mosi, int clk, int slaveSelect, Component.Types.SpiMasterRate rate)
        {
            DigitalOutputSpec[] slaveSelectCalc = new DigitalOutputSpec[1];
            slaveSelectCalc[0] = new DigitalOutputSpec(slaveSelect);
            return new Component.Impl.SpiMaster(new DigitalInputSpec(miso), new DigitalOutputSpec(mosi), new DigitalOutputSpec(clk), slaveSelectCalc, new SpiMasterConfig(rate));
        }

        public Component.ITwiMaster openTwiMaster(int twiNum, Component.Types.TwiMasterRate rate, bool smbus)
        {
            return new Component.TwiMaster(twiNum, rate, smbus);
        }

        public Component.IIcspMaster openIcspMaster()
        {
            return new Component.Impl.IcspMaster();
        }

        public Component.ICapSense openCapSense(int pin)
        {
            return new Component.Impl.CapSense(pin, CapSense.DEFAULT_COEF);
        }

        public Component.ICapSense openCapSense(int pin, float filterCoef)
        {
            return new Component.Impl.CapSense(pin, filterCoef);
        }

        public Component.ISequencer openSequencer(Component.Types.ISequencerChannelConfig[] config)
        {
            return new Component.Impl.Sequencer(config);
        }

        public void beginBatch()
        {
            throw new NotImplementedException();
        }

        public void endBatch()
        {
            throw new NotImplementedException();
        }

        public void sync()
        {
            throw new NotImplementedException();
        }

    }
}
