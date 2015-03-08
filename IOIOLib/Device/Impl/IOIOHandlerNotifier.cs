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
