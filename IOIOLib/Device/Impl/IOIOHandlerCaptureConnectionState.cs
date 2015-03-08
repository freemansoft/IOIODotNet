using IOIOLib.Device.Types;
using IOIOLib.MessageFrom;
using IOIOLib.MessageFrom.Impl;
using IOIOLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// Small subset of IOIOHandlerCaptureSeparateQueue
    /// </summary>
    public class IOIOHandlerCaptureConnectionState : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerCaptureConnectionState));
        /// <summary>
        /// response from the checkInterfaceResponse call 
        /// </summary>
        internal ISupportedInterfaceFrom Supported_;

        internal IEstablishConnectionFrom EstablishConnectionFrom_;

        public void handleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            EstablishConnectionFrom_ = new EstablishConnectionFrom(
                System.Text.Encoding.ASCII.GetString(hardwareId),
                System.Text.Encoding.ASCII.GetString(bootloaderId),
                System.Text.Encoding.ASCII.GetString(firmwareId),
                Board.AllBoards[System.Text.Encoding.ASCII.GetString(hardwareId)]
                );
        }

        /// <summary>
        /// TODO implement
        /// </summary>
        public void handleConnectionLost()
        {
        }

        /// <summary>
        ///  TODO implement
        /// </summary>
        public void handleSoftReset()
        {
        }

        public void handleCheckInterfaceResponse(bool supported)
        {
            this.Supported_ = new SupportedInterfaceFrom(supported);
        }

        public void handleSetChangeNotify(int pin, bool changeNotify)
        {
        }

        public void handleReportDigitalInStatus(int pin, bool level)
        {
        }

        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
        }

        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
        }

        public void handleAnalogPinStatus(int pin, bool open)
        {
        }

        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
        }

        public void handleUartOpen(int uartNum)
        {
        }

        public void handleUartClose(int uartNum)
        {
        }

        public void handleUartData(int uartNum, int numBytes, byte[] data)
        {
        }


        public void handleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
        }

        public void handleSpiOpen(int spiNum)
        {
        }

        public void handleSpiClose(int spiNum)
        {
        }

        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
        }

        internal IDictionary<int, IHandleSpiReportTxStatusFrom> StatehandleSpiReportTxStatus_ = new Dictionary<int, IHandleSpiReportTxStatusFrom>();
        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        internal IDictionary<int, II2cFrom> StateI2c_ = new Dictionary<int, II2cFrom>();
        public void handleI2cOpen(int i2cNum)
        {
        }

        public void handleI2cClose(int i2cNum)
        {
        }

        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
        }

        internal IDictionary<int, IHandleI2cReportTxStatusFrom> StateHandleI2cReportTxStatus_ = new Dictionary<int, IHandleI2cReportTxStatusFrom>();
        public void handleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        public void handleIcspOpen()
        {
        }

        public void handleIcspClose()
        {
        }

        public void handleIcspReportRxStatus(int bytesRemaining)
        {
        }

        public void handleIcspResult(int size, byte[] data)
        {
        }

        public void handleIncapReport(int incapNum, int size, byte[] data)
        {
        }

        public void handleIncapClose(int incapNum)
        {
        }

        public void handleIncapOpen(int incapNum)
        {
        }

        public void handleCapSenseReport(int pinNum, int value)
        {
        }

        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
        }

        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
        }

        /// <summary>
        /// TODO Implement... possibly
        /// </summary>
        public void handleSync()
        {
        }
    }
}
