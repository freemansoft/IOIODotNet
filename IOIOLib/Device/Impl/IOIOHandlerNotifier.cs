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
        public void HandleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            throw new NotImplementedException();
        }

        public void HandleConnectionLost()
        {
            throw new NotImplementedException();
        }

        public void HandleSoftReset()
        {
            throw new NotImplementedException();
        }

        public void HandleCheckInterfaceResponse(bool supported)
        {
            throw new NotImplementedException();
        }

        public void HandleSetChangeNotify(int pin, bool changeNotify)
        {
            throw new NotImplementedException();
        }

        public void HandleReportDigitalInStatus(int pin, bool level)
        {
            throw new NotImplementedException();
        }

        public void HandleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            throw new NotImplementedException();
        }

        public void HandleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            throw new NotImplementedException();
        }

        public void HandleAnalogPinStatus(int pin, bool open)
        {
            throw new NotImplementedException();
        }

        public void HandleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            throw new NotImplementedException();
        }

        public void HandleUartOpen(int uartNum)
        {
            throw new NotImplementedException();
        }

        public void HandleUartClose(int uartNum)
        {
            throw new NotImplementedException();
        }

        public void HandleUartData(int uartNum, int numBytes, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void HandleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            throw new NotImplementedException();
        }

        public void HandleSpiOpen(int spiNum)
        {
            throw new NotImplementedException();
        }

        public void HandleSpiClose(int spiNum)
        {
            throw new NotImplementedException();
        }

        public void HandleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            throw new NotImplementedException();
        }

        public void HandleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            throw new NotImplementedException();
        }

        public void HandleI2cOpen(int i2cNum)
        {
            throw new NotImplementedException();
        }

        public void HandleI2cClose(int i2cNum)
        {
            throw new NotImplementedException();
        }

        public void HandleI2cResult(int i2cNum, int size, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void HandleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
            throw new NotImplementedException();
        }

        public void HandleIcspOpen()
        {
            throw new NotImplementedException();
        }

        public void HandleIcspClose()
        {
            throw new NotImplementedException();
        }

        public void HandleIcspReportRxStatus(int bytesRemaining)
        {
            throw new NotImplementedException();
        }

        public void HandleIcspResult(int size, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void HandleIncapReport(int incapNum, int size, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void HandleIncapClose(int incapNum)
        {
            throw new NotImplementedException();
        }

        public void HandleIncapOpen(int incapNum)
        {
            throw new NotImplementedException();
        }

        public void HandleCapSenseReport(int pinNum, int value)
        {
            throw new NotImplementedException();
        }

        public void HandleSetCapSenseSampling(int pinNum, bool enable)
        {
            throw new NotImplementedException();
        }

        public void HandleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            throw new NotImplementedException();
        }

        public void HandleSync()
        {
            throw new NotImplementedException();
        }
    }
}
