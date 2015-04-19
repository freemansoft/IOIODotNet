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

using IOIOLib.Component.Types;
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
    public interface IIncomingHandlerIOIO
    {
        void HandleEstablishConnection(byte[] hardwareId, byte[] bootloaderId,
                byte[] firmwareId);

        void HandleConnectionLost();

        void HandleSoftReset();

        void HandleCheckInterfaceResponse(bool supported);

        void HandleSetChangeNotify(int pin, bool changeNotify);

        void HandleReportDigitalInStatus(int pin, bool level);

        void HandleRegisterPeriodicDigitalSampling(int pin, int freqScale);

        void HandleReportPeriodicDigitalInStatus(int frameNum, bool[] values);

        /// <summary>
        /// Added and removed pins
        /// </summary>
        /// <param name="Pin"></param>
        /// <param name="IsOpen"></param>
        void HandleAnalogPinStatus(int pin, bool open);

        /// <summary>
        /// Pin analog Values
        /// </summary>
        /// <param name="pins"></param>
        /// <param name="Values"></param>
        void HandleReportAnalogInStatus(List<int> pins, List<int> values);

        void HandleUartOpen(int uartNum);

        void HandleUartClose(int uartNum);

        void HandleUartData(int uartNum, int numBytes, byte[] data);

        void HandleUartReportTxStatus(int uartNum, int bytesRemaining);

        void HandleSpiOpen(int spiNum);

        void HandleSpiClose(int spiNum);

        void HandleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes);

        void HandleSpiReportTxStatus(int spiNum, int bytesRemaining);

        void HandleI2cOpen(int i2cNum);

        void HandleI2cClose(int i2cNum);

        void HandleI2cResult(int i2cNum, int size, byte[] data);

        void HandleI2cReportTxStatus(int i2cNum, int bytesRemaining);

        void HandleIcspOpen();

        void HandleIcspClose();

        void HandleIcspReportRxStatus(int bytesRemaining);

        void HandleIcspResult(int size, byte[] data);

        void HandleIncapReport(int incapNum, int size, byte[] data);

        void HandleIncapClose(int incapNum);

        void HandleIncapOpen(int incapNum);

        void HandleCapSenseReport(int pinNum, int value);

        void HandleSetCapSenseSampling(int pinNum, bool enable);

        void HandleSequencerEvent(SequencerEventState seqEvent, int arg);

        void HandleSync();
    }
}
