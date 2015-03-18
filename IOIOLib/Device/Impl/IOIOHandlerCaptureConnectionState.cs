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
    /// Used to capture connection state but nothing else.  No memory growth issue with this
    /// </summary>
    public class IOIOHandlerCaptureConnectionState : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerCaptureConnectionState));
        /// <summary>
        /// response from the checkInterfaceResponse call 
        /// </summary>
        internal ISupportedInterfaceFrom Supported_;

        internal IConnectedDeviceResponse EstablishConnectionFrom_;

        public void HandleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            EstablishConnectionFrom_ = new ConnectedDeviceResponse(
                System.Text.Encoding.ASCII.GetString(hardwareId),
                System.Text.Encoding.ASCII.GetString(bootloaderId),
                System.Text.Encoding.ASCII.GetString(firmwareId),
                Board.AllBoards[System.Text.Encoding.ASCII.GetString(hardwareId)]
                );
        }

        /// <summary>
        /// TODO implement
        /// </summary>
        public void HandleConnectionLost()
        {
        }

        /// <summary>
        ///  TODO implement
        /// </summary>
        public void HandleSoftReset()
        {
        }

        public void HandleCheckInterfaceResponse(bool supported)
        {
            this.Supported_ = new SupportedInterfaceFrom(supported);
        }

        public void HandleSetChangeNotify(int pin, bool changeNotify)
        {
        }

        public void HandleReportDigitalInStatus(int pin, bool level)
        {
        }

        public void HandleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
        }

        public void HandleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
        }

        public void HandleAnalogPinStatus(int pin, bool open)
        {
        }

        public void HandleReportAnalogInStatus(List<int> pins, List<int> values)
        {
        }

        public void HandleUartOpen(int uartNum)
        {
        }

        public void HandleUartClose(int uartNum)
        {
        }

        public void HandleUartData(int uartNum, int numBytes, byte[] data)
        {
        }


        public void HandleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
        }

        public void HandleSpiOpen(int spiNum)
        {
        }

        public void HandleSpiClose(int spiNum)
        {
        }

        public void HandleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
        }

        internal IDictionary<int, IHandleSpiReportTxStatusFrom> StatehandleSpiReportTxStatus_ = new Dictionary<int, IHandleSpiReportTxStatusFrom>();
        public void HandleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        internal IDictionary<int, II2cFrom> StateI2c_ = new Dictionary<int, II2cFrom>();
        public void HandleI2cOpen(int i2cNum)
        {
        }

        public void HandleI2cClose(int i2cNum)
        {
        }

        public void HandleI2cResult(int i2cNum, int size, byte[] data)
        {
        }

        internal IDictionary<int, IHandleI2cReportTxStatusFrom> StateHandleI2cReportTxStatus_ = new Dictionary<int, IHandleI2cReportTxStatusFrom>();
        public void HandleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        public void HandleIcspOpen()
        {
        }

        public void HandleIcspClose()
        {
        }

        public void HandleIcspReportRxStatus(int bytesRemaining)
        {
        }

        public void HandleIcspResult(int size, byte[] data)
        {
        }

        public void HandleIncapReport(int incapNum, int size, byte[] data)
        {
        }

        public void HandleIncapClose(int incapNum)
        {
        }

        public void HandleIncapOpen(int incapNum)
        {
        }

        public void HandleCapSenseReport(int pinNum, int value)
        {
        }

        public void HandleSetCapSenseSampling(int pinNum, bool enable)
        {
        }

        public void HandleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
        }

        /// <summary>
        /// TODO Implement... possibly
        /// </summary>
        public void HandleSync()
        {
        }
    }
}
