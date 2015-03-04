using IOIOLib.Connection;
using IOIOLib.Device.Types;
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
            throw new NotImplementedException();
        }

        public Component.IDigitalInput openDigitalInput(Component.Types.DigitalInputSpec spec)
        {
            throw new NotImplementedException();
        }

        public Component.IDigitalInput openDigitalInput(int pin)
        {
            throw new NotImplementedException();
        }

        public Component.IDigitalInput openDigitalInput(int pin, Component.Types.DigitalInputSpecMode mode)
        {
            throw new NotImplementedException();
        }

        public Component.IDigitalOutput openDigitalOutput(Component.Types.DigitalOutputSpec spec, bool startValue)
        {
            throw new NotImplementedException();
        }

        public Component.IDigitalOutput openDigitalOutput(int pin, Component.Types.DigitalOutputSpecMode mode, bool startValue)
        {
            throw new NotImplementedException();
        }

        public Component.IDigitalOutput openDigitalOutput(int pin, bool startValue)
        {
            throw new NotImplementedException();
        }

        public Component.IDigitalOutput openDigitalOutput(int pin)
        {
            throw new NotImplementedException();
        }

        public Component.IAnalogInput openAnalogInput(int pin)
        {
            throw new NotImplementedException();
        }

        public Component.IPwmOutput openPwmOutput(Component.Types.DigitalOutputSpec spec, int freqHz)
        {
            throw new NotImplementedException();
        }

        public Component.IPwmOutput openPwmOutput(int pin, int freqHz)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Component.IUart openUart(int rx, int tx, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
        {
            throw new NotImplementedException();
        }

        public Component.ISpiMaster openSpiMaster(Component.Types.DigitalInputSpec miso, Component.Types.DigitalOutputSpec mosi, Component.Types.DigitalOutputSpec clk, Component.Types.DigitalOutputSpec[] slaveSelect, Component.Types.SpiMasterConfig config)
        {
            throw new NotImplementedException();
        }

        public Component.ISpiMaster openSpiMaster(int miso, int mosi, int clk, int[] slaveSelect, Component.Types.SpiMasterRate rate)
        {
            throw new NotImplementedException();
        }

        public Component.ISpiMaster openSpiMaster(int miso, int mosi, int clk, int slaveSelect, Component.Types.SpiMasterRate rate)
        {
            throw new NotImplementedException();
        }

        public Component.ITwiMaster openTwiMaster(int twiNum, Component.Types.TwiMasterRate rate, bool smbus)
        {
            throw new NotImplementedException();
        }

        public Component.IIcspMaster openIcspMaster()
        {
            throw new NotImplementedException();
        }

        public Component.ICapSense openCapSense(int pin)
        {
            throw new NotImplementedException();
        }

        public Component.ICapSense openCapSense(int pin, float filterCoef)
        {
            throw new NotImplementedException();
        }

        public Component.ISequencer openSequencer(Component.Types.ISequencerChannelConfig[] config)
        {
            throw new NotImplementedException();
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
