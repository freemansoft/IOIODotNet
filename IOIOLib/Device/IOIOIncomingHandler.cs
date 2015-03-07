using IOIOLib.Device.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device
{
    /// <summary>
    /// The incoming protocol handler
    /// </summary>
    public interface IOIOIncomingHandler
    {
        void handleEstablishConnection(byte[] hardwareId, byte[] bootloaderId,
                byte[] firmwareId);

        void handleConnectionLost();

        void handleSoftReset();

        void handleCheckInterfaceResponse(bool supported);

        void handleSetChangeNotify(int pin, bool changeNotify);

        void handleReportDigitalInStatus(int pin, bool level);

        void handleRegisterPeriodicDigitalSampling(int pin, int freqScale);

        void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values);

        /// <summary>
        /// Added and removed pins
        /// </summary>
        /// <param name="Pin"></param>
        /// <param name="IsOpen"></param>
        void handleAnalogPinStatus(int pin, bool open);

        /// <summary>
        /// Pin analog Values
        /// </summary>
        /// <param name="pins"></param>
        /// <param name="Values"></param>
        void handleReportAnalogInStatus(List<int> pins, List<int> values);

        void handleUartOpen(int uartNum);

        void handleUartClose(int uartNum);

        void handleUartData(int uartNum, int numBytes, byte[] data);

        void handleUartReportTxStatus(int uartNum, int bytesRemaining);

        void handleSpiOpen(int spiNum);

        void handleSpiClose(int spiNum);

        void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes);

        void handleSpiReportTxStatus(int spiNum, int bytesRemaining);

        void handleI2cOpen(int i2cNum);

        void handleI2cClose(int i2cNum);

        void handleI2cResult(int i2cNum, int size, byte[] data);

        void handleI2cReportTxStatus(int spiNum, int bytesRemaining);

        void handleIcspOpen();

        void handleIcspClose();

        void handleIcspReportRxStatus(int bytesRemaining);

        void handleIcspResult(int size, byte[] data);

        void handleIncapReport(int incapNum, int size, byte[] data);

        void handleIncapClose(int incapNum);

        void handleIncapOpen(int incapNum);

        void handleCapSenseReport(int pinNum, int value);

        void handleSetCapSenseSampling(int pinNum, bool enable);

        void handleSequencerEvent(SequencerEvent seqEvent, int arg);

        void handleSync();
    }
}
