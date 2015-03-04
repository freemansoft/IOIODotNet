using IOIOLib.Device.Types;
using IOIOLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// TODO: This class should be a dispatcher for event listeners.
    /// </summary>
    public class IOIOIncomingHandlerCaptureState : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOIncomingHandlerCaptureState));
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        private byte[] HardwareId;
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        private byte[] BootloaderId;
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        private byte[] FirmwareId;
        /// <summary>
        /// response from the checkInterfaceResponse call 
        /// </summary>
        internal bool Supported;
        /// <summary>
        /// should we have a variable here or just look it up each time?
        /// this should probably not exist.  It should be posted as part of event to listeners
        /// </summary>
        internal Hardware OurHardware = null;

        public void handleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            this.HardwareId = hardwareId;
            this.BootloaderId = bootloaderId;
            this.FirmwareId = firmwareId;
            OurHardware = Board.AllBoards[System.Text.Encoding.ASCII.GetString(hardwareId)];
        }

        public void handleConnectionLost()
        {
        }

        public void handleSoftReset()
        {
        }

        public void handleCheckInterfaceResponse(bool supported)
        {
            this.Supported = supported;
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

        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        public void handleI2cOpen(int i2cNum)
        {
        }

        public void handleI2cClose(int i2cNum)
        {
        }

        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
        }

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

        public void handleSync()
        {
        }
    }
}
