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
    /// TODO: Captures a string message for every incoming.  Primarily used for testing
    /// </summary>
    public class IOIOIncomingHandlerCaptureLog : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOIncomingHandlerCaptureLog));

        /// <summary>
        /// TODO:  Use more efficient queue that retains last N
        /// </summary>
        internal List<string> capturedLogs = new List<string>();

        internal int MaxCount = 5;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxCaptureDepth">number to retain in buffer.  
        /// value less 0 means all which cna be a lot
        /// </param>
        public IOIOIncomingHandlerCaptureLog(int maxCaptureDepth)
        {
            MaxCount = maxCaptureDepth;
        }

        private void LogAndCapture(string logString)
        {
            LOG.Debug(logString);
            capturedLogs.Add(logString);
            if (MaxCount > 0 && capturedLogs.Count > MaxCount)
            {
                capturedLogs.RemoveAt(0);
            }
        }


        public void handleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {

            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name
             + " hwId:" + System.Text.Encoding.ASCII.GetString(hardwareId)
             + " bootId:" + System.Text.Encoding.ASCII.GetString(bootloaderId)
             + " firmId:" + System.Text.Encoding.ASCII.GetString(firmwareId);
            LogAndCapture(LogString);
        }

        public void handleConnectionLost()
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogAndCapture(LogString);
        }

        public void handleSoftReset()
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogAndCapture(LogString);
        }

        public void handleCheckInterfaceResponse(bool supported)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " supported: " + supported;
            LogAndCapture(LogString);
        }

        public void handleSetChangeNotify(int pin, bool changeNotify)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pin:" + pin + " " + changeNotify;
            LogAndCapture(LogString);
        }

        public void handleReportDigitalInStatus(int pin, bool level)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pin:" + pin + " " + level;
            LogAndCapture(LogString);
        }

        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pin:" + pin;
            LogAndCapture(LogString);
        }

        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " frameNum:" + frameNum;
            LogAndCapture(LogString);
        }

        public void handleAnalogPinStatus(int pin, bool open)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pin:" + pin;
            LogAndCapture(LogString);
        }

        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pins:" + pins;
            LogAndCapture(LogString);
        }

        public void handleUartOpen(int uartNum)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " uartNum:" + uartNum;
            LogAndCapture(LogString);
        }

        public void handleUartClose(int uartNum)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " uartNum:" + uartNum;
            LogAndCapture(LogString);
        }

        public void handleUartData(int uartNum, int numBytes, byte[] data)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " uartNum:" + uartNum;
            LogAndCapture(LogString);
        }

        public void handleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " uartNum:" + uartNum;
            LogAndCapture(LogString);
        }

        public void handleSpiOpen(int spiNum)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " spiNum:" + spiNum;
            LogAndCapture(LogString);
        }

        public void handleSpiClose(int spiNum)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " spiNum:" + spiNum;
            LogAndCapture(LogString);
        }

        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " spiNum:" + spiNum;
            LogAndCapture(LogString);
        }

        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " spiNum:" + spiNum;
            LogAndCapture(LogString);
        }

        public void handleI2cOpen(int i2cNum)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " i2cNum:" + i2cNum;
            LogAndCapture(LogString);
        }

        public void handleI2cClose(int i2cNum)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " i2cNum:" + i2cNum;
            LogAndCapture(LogString);
        }

        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " i2cNum:" + i2cNum;
            LogAndCapture(LogString);
        }

        public void handleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " spiNum:" + spiNum;
            LogAndCapture(LogString);
        }

        public void handleIcspOpen()
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogAndCapture(LogString);
        }

        public void handleIcspClose()
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogAndCapture(LogString);
        }

        public void handleIcspReportRxStatus(int bytesRemaining)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " bytesRemaing:" + bytesRemaining;
            LogAndCapture(LogString);
        }

        public void handleIcspResult(int size, byte[] data)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " size:" + size;
            LogAndCapture(LogString);
        }
        public void handleIncapReport(int incapNum, int size, byte[] data)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " incapNum:" + incapNum;
            LogAndCapture(LogString);
        }

        public void handleIncapClose(int incapNum)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pin:" + incapNum;
            LogAndCapture(LogString);
        }

        public void handleIncapOpen(int incapNum)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pin:" + incapNum;
            LogAndCapture(LogString);
        }

        public void handleCapSenseReport(int pinNum, int value)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pin:" + pinNum;
            LogAndCapture(LogString);
        }

        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pin:" + pinNum;
            LogAndCapture(LogString);
        }

        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogAndCapture(LogString);
        }

        public void handleSync()
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogAndCapture(LogString);
        }
    }
}
