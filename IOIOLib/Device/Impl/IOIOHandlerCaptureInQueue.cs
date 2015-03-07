using IOIOLib.Device.Types;
using IOIOLib.MessageFrom;
using IOIOLib.MessageFrom.Impl;
using IOIOLib.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// TODO: This class should be a dispatcher for event listeners.
    /// </summary>
    public class IOIOHandlerCaptureInQueue : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerCaptureInQueue));

        /// <summary>
        /// Need to come up with an API for this
        /// </summary>
        internal ConcurrentQueue<IMessageFromIOIO> CapturedMessages =
            new ConcurrentQueue<IMessageFromIOIO>();

        public void Enqueue(IMessageFromIOIO message)
        {
            CapturedMessages.Enqueue(message);
        }

        public void handleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            IEstablishConnectionFrom EstablishConnectionFrom_ = new EstablishConnectionFrom(
                System.Text.Encoding.ASCII.GetString(hardwareId),
                System.Text.Encoding.ASCII.GetString(bootloaderId),
                System.Text.Encoding.ASCII.GetString(firmwareId),
                Board.AllBoards[System.Text.Encoding.ASCII.GetString(hardwareId)]
                );
            this.Enqueue(EstablishConnectionFrom_);
        }

        public void handleConnectionLost()
        {
        }

        public void handleSoftReset()
        {
        }

        public void handleCheckInterfaceResponse(bool supported)
        {
            this.Enqueue(new SupportedInterfaceFrom(supported));
        }

        public void handleSetChangeNotify(int pin, bool changeNotify)
        {
            this.Enqueue(new SetChangeNotifyMessageFrom(pin, changeNotify));
        }

        public void handleReportDigitalInStatus(int pin, bool level)
        {
            this.Enqueue(new ReportDigitalInStatusFrom(pin, level));
        }

        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            this.Enqueue(new RegisterPeriodicDigitalSamplingFrom(pin, freqScale));
        }

        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            this.Enqueue(new ReportPeriodicDigitalInStatusFrom(frameNum, values));
        }

        public void handleAnalogPinStatus(int pin, bool open)
        {
            this.Enqueue(new AnalogPinStatusFrom(pin, open));
        }

        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            if (pins.Count != values.Count)
            {
                LOG.Warn("handleReportAnalogInStatus has pins:" + pins.Count + " Values:" + values.Count);
            }
            for (int i = 0; i < pins.Count; i++)
            {
                this.Enqueue(new ReportAnalogPinValuesFrom(pins[i], values[i]));
            }
        }

        /// <summary>
        ///  empty or close means closed
        ///  IsOpen means IsOpen
        /// </summary>
        public void handleUartOpen(int uartNum)
        {
            this.Enqueue(new UartOpenFrom(uartNum));
        }

        public void handleUartClose(int uartNum)
        {
            this.Enqueue(new UartCloseFrom(uartNum));
        }

        public void handleUartData(int uartNum, int numBytes, byte[] data)
        {
            this.Enqueue( new UartDataFrom(uartNum, numBytes, data));
        }


        public void handleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            this.Enqueue(new HandleUartReportTxStatusFrom(uartNum, bytesRemaining));
        }

        public void handleSpiOpen(int spiNum)
        {
            this.Enqueue(new SpiOpenFrom(spiNum));
        }

        public void handleSpiClose(int spiNum)
        {
            this.Enqueue(new SpiCloseFrom(spiNum));
        }

        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            this.Enqueue(new SpiDataFrom(spiNum, ssPin, data, dataBytes));
        }

        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            this.Enqueue(new HandleSpiReportTxStatusFrom(spiNum, bytesRemaining));
        }

        public void handleI2cOpen(int i2cNum)
        {
            this.Enqueue(new I2cOpenFrom(i2cNum));
        }

        public void handleI2cClose(int i2cNum)
        {
            this.Enqueue(new I2cCloseFrom(i2cNum));
        }

        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
            this.Enqueue(new I2cResultFrom(i2cNum, size, data));
        }

        public void handleI2cReportTxStatus(int i2cNum, int bytesRemaining)
        {
            this.Enqueue(new HandleI2cReportTxStatusFrom(i2cNum));
        }

        // default to close
        public void handleIcspOpen()
        {
            this.Enqueue(new IcspOpenFrom());
        }

        public void handleIcspClose()
        {
            this.Enqueue(new IcspCloseFrom());
        }

        public void handleIcspReportRxStatus(int bytesRemaining)
        {
            this.Enqueue(new IcspReportRxStatusFrom(bytesRemaining));
        }

        public void handleIcspResult(int size, byte[] data)
        {
            this.Enqueue(new IcspResultFrom(size, data));
        }

        public void handleIncapReport(int incapNum, int size, byte[] data)
        {
            this.Enqueue(new IncapReportFrom(incapNum, size, data));
        }

        public void handleIncapClose(int incapNum)
        {
            this.Enqueue(new IncapOpenFrom(incapNum));
        }

        public void handleIncapOpen(int incapNum)
        {
            this.Enqueue(new IncapCloseFrom(incapNum));
        }

        public void handleCapSenseReport(int pinNum, int value)
        {
            this.Enqueue(new CapSenseReportFrom(pinNum, value));
        }

        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
            this.Enqueue(new CapSenseSamplingFrom(pinNum, enable));
        }

        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            this.Enqueue(new SequencerEventFrom(seqEvent, arg));
        }

        public void handleSync()
        {
        }
    }
}
