using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// This class does nothing. It is a placeholder for future notification code.
    /// It partially exists so that the README.md has something to mention.
    /// </summary>
    class IOIOHandlerNotifier : IOIOIncomingHandler
    {
        public void handleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            throw new NotImplementedException();
        }

        public void handleConnectionLost()
        {
            throw new NotImplementedException();
        }

        public void handleSoftReset()
        {
            throw new NotImplementedException();
        }

        public void handleCheckInterfaceResponse(bool supported)
        {
            throw new NotImplementedException();
        }

        public void handleSetChangeNotify(int pin, bool changeNotify)
        {
            throw new NotImplementedException();
        }

        public void handleReportDigitalInStatus(int pin, bool level)
        {
            throw new NotImplementedException();
        }

        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            throw new NotImplementedException();
        }

        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            throw new NotImplementedException();
        }

        public void handleAnalogPinStatus(int pin, bool open)
        {
            throw new NotImplementedException();
        }

        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            throw new NotImplementedException();
        }

        public void handleUartOpen(int uartNum)
        {
            throw new NotImplementedException();
        }

        public void handleUartClose(int uartNum)
        {
            throw new NotImplementedException();
        }

        public void handleUartData(int uartNum, int numBytes, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void handleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            throw new NotImplementedException();
        }

        public void handleSpiOpen(int spiNum)
        {
            throw new NotImplementedException();
        }

        public void handleSpiClose(int spiNum)
        {
            throw new NotImplementedException();
        }

        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            throw new NotImplementedException();
        }

        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            throw new NotImplementedException();
        }

        public void handleI2cOpen(int i2cNum)
        {
            throw new NotImplementedException();
        }

        public void handleI2cClose(int i2cNum)
        {
            throw new NotImplementedException();
        }

        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void handleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
            throw new NotImplementedException();
        }

        public void handleIcspOpen()
        {
            throw new NotImplementedException();
        }

        public void handleIcspClose()
        {
            throw new NotImplementedException();
        }

        public void handleIcspReportRxStatus(int bytesRemaining)
        {
            throw new NotImplementedException();
        }

        public void handleIcspResult(int size, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void handleIncapReport(int incapNum, int size, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void handleIncapClose(int incapNum)
        {
            throw new NotImplementedException();
        }

        public void handleIncapOpen(int incapNum)
        {
            throw new NotImplementedException();
        }

        public void handleCapSenseReport(int pinNum, int value)
        {
            throw new NotImplementedException();
        }

        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
            throw new NotImplementedException();
        }

        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            throw new NotImplementedException();
        }

        public void handleSync()
        {
            throw new NotImplementedException();
        }
    }
}
