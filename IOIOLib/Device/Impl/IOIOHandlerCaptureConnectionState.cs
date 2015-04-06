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

        public virtual void HandleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
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
        public virtual void HandleConnectionLost()
        {
        }

        /// <summary>
        ///  TODO implement
        /// </summary>
        public virtual void HandleSoftReset()
        {
        }

        public virtual void HandleCheckInterfaceResponse(bool supported)
        {
            this.Supported_ = new SupportedInterfaceFrom(supported);
        }

        public virtual void HandleSetChangeNotify(int pin, bool changeNotify)
        {
        }

        public virtual void HandleReportDigitalInStatus(int pin, bool level)
        {
        }

        public virtual void HandleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
        }

        public virtual void HandleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
        }

        public virtual void HandleAnalogPinStatus(int pin, bool open)
        {
        }

        public virtual void HandleReportAnalogInStatus(List<int> pins, List<int> values)
        {
        }

        public virtual void HandleUartOpen(int uartNum)
        {
        }

        public virtual void HandleUartClose(int uartNum)
        {
        }

        public virtual void HandleUartData(int uartNum, int numBytes, byte[] data)
        {
        }


        public virtual void HandleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
        }

        public virtual void HandleSpiOpen(int spiNum)
        {
        }

        public virtual void HandleSpiClose(int spiNum)
        {
        }

        public virtual void HandleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
        }

        internal IDictionary<int, ISpiReportTxStatusFrom> StatehandleSpiReportTxStatus_ = new Dictionary<int, ISpiReportTxStatusFrom>();
        public virtual void HandleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        internal IDictionary<int, II2cFrom> StateI2c_ = new Dictionary<int, II2cFrom>();
        public virtual void HandleI2cOpen(int i2cNum)
        {
        }

        public virtual void HandleI2cClose(int i2cNum)
        {
        }

        public virtual void HandleI2cResult(int i2cNum, int size, byte[] data)
        {
        }

        public virtual void HandleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        public virtual void HandleIcspOpen()
        {
        }

        public virtual void HandleIcspClose()
        {
        }

        public virtual void HandleIcspReportRxStatus(int bytesRemaining)
        {
        }

        public virtual void HandleIcspResult(int size, byte[] data)
        {
        }

        public virtual void HandleIncapReport(int incapNum, int size, byte[] data)
        {
        }

        public virtual void HandleIncapClose(int incapNum)
        {
        }

        public virtual void HandleIncapOpen(int incapNum)
        {
        }

        public virtual void HandleCapSenseReport(int pinNum, int value)
        {
        }

        public virtual void HandleSetCapSenseSampling(int pinNum, bool enable)
        {
        }

        public virtual void HandleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
        }

        /// <summary>
        /// TODO Implement... possibly
        /// </summary>
        public virtual void HandleSync()
        {
        }
    }
}
