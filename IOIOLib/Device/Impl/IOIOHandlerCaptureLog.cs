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
    public class IOIOHandlerCaptureLog : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerCaptureLog));

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
        public IOIOHandlerCaptureLog(int maxCaptureDepth)
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
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " Pin:" + pin + " " + changeNotify;
            LogAndCapture(LogString);
        }

        public void handleReportDigitalInStatus(int pin, bool level)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " Pin:" + pin + " " + level;
            LogAndCapture(LogString);
        }

        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " Pin:" + pin;
            LogAndCapture(LogString);
        }

        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " FrameNum:" + frameNum;
            LogAndCapture(LogString);
        }

        public void handleAnalogPinStatus(int pin, bool open)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " Pin:" + pin;
            LogAndCapture(LogString);
        }

        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            string LogString;
            if (pins != null && values != null)
            {
                LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pins:" + string.Join(", ", pins) + " Values: " + string.Join(", ", values);
            }
            else
            {
                LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " pins:" + pins + " Values: " + values;
            }
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
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " i2cNum:" + spiNum;
            LogAndCapture(LogString);
        }

        public void handleSpiClose(int spiNum)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " i2cNum:" + spiNum;
            LogAndCapture(LogString);
        }

        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " i2cNum:" + spiNum;
            LogAndCapture(LogString);
        }

        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " i2cNum:" + spiNum;
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
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " i2cNum:" + spiNum;
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
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " Pin:" + incapNum;
            LogAndCapture(LogString);
        }

        public void handleIncapOpen(int incapNum)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " Pin:" + incapNum;
            LogAndCapture(LogString);
        }

        public void handleCapSenseReport(int pinNum, int value)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " Pin:" + pinNum;
            LogAndCapture(LogString);
        }

        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " Pin:" + pinNum;
            LogAndCapture(LogString);
        }

        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name + " Event:" + seqEvent;
            LogAndCapture(LogString);
        }

        public void handleSync()
        {
            string LogString = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LogAndCapture(LogString);
        }
    }
}
