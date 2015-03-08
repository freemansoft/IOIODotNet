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
