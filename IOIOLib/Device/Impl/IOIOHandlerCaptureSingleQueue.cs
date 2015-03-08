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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// TODO: This class should be a dispatcher for event listeners.
    /// </summary>
    public class IOIOHandlerCaptureSingleQueue : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerCaptureSingleQueue));

        /// <summary>
        /// Need to come up with an API for this
        /// </summary>
        internal ConcurrentQueue<IMessageFromIOIO> CapturedMessages_ =
            new ConcurrentQueue<IMessageFromIOIO>();

        public void Enqueue(IMessageFromIOIO message)
        {
            CapturedMessages_.Enqueue(message);
        }

        public void HandleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            IEstablishConnectionFrom EstablishConnectionFrom_ = new EstablishConnectionFrom(
                System.Text.Encoding.ASCII.GetString(hardwareId),
                System.Text.Encoding.ASCII.GetString(bootloaderId),
                System.Text.Encoding.ASCII.GetString(firmwareId),
                Board.AllBoards[System.Text.Encoding.ASCII.GetString(hardwareId)]
                );
            this.Enqueue(EstablishConnectionFrom_);
        }

        public void HandleConnectionLost()
        {
        }

        public void HandleSoftReset()
        {
        }

        public void HandleCheckInterfaceResponse(bool supported)
        {
            this.Enqueue(new SupportedInterfaceFrom(supported));
        }

        public void HandleSetChangeNotify(int pin, bool changeNotify)
        {
            this.Enqueue(new SetChangeNotifyMessageFrom(pin, changeNotify));
        }

        public void HandleReportDigitalInStatus(int pin, bool level)
        {
            this.Enqueue(new ReportDigitalInStatusFrom(pin, level));
        }

        public void HandleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            this.Enqueue(new RegisterPeriodicDigitalSamplingFrom(pin, freqScale));
        }

        public void HandleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            this.Enqueue(new ReportPeriodicDigitalInStatusFrom(frameNum, values));
        }

        public void HandleAnalogPinStatus(int pin, bool open)
        {
            this.Enqueue(new AnalogPinStatusFrom(pin, open));
        }

        public void HandleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            if (pins.Count != values.Count)
            {
                LOG.Warn("HandleReportAnalogInStatus has pins:" + pins.Count + " Values:" + values.Count);
            }
            for (int i = 0; i < pins.Count; i++)
            {
                this.Enqueue(new ReportAnalogPinValuesFrom(pins[i], values[i]));
            }
        }

        /// <summary>
        ///  empty or close means closed
        ///  IsOpen means IsOpen
        /// </summary>
        public void HandleUartOpen(int uartNum)
        {
            this.Enqueue(new UartOpenFrom(uartNum));
        }

        public void HandleUartClose(int uartNum)
        {
            this.Enqueue(new UartCloseFrom(uartNum));
        }

        public void HandleUartData(int uartNum, int numBytes, byte[] data)
        {
            this.Enqueue( new UartDataFrom(uartNum, numBytes, data));
        }


        public void HandleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            this.Enqueue(new HandleUartReportTxStatusFrom(uartNum, bytesRemaining));
        }

        public void HandleSpiOpen(int spiNum)
        {
            this.Enqueue(new SpiOpenFrom(spiNum));
        }

        public void HandleSpiClose(int spiNum)
        {
            this.Enqueue(new SpiCloseFrom(spiNum));
        }

        public void HandleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            this.Enqueue(new SpiDataFrom(spiNum, ssPin, data, dataBytes));
        }

        public void HandleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            this.Enqueue(new HandleSpiReportTxStatusFrom(spiNum, bytesRemaining));
        }

        public void HandleI2cOpen(int i2cNum)
        {
            this.Enqueue(new I2cOpenFrom(i2cNum));
        }

        public void HandleI2cClose(int i2cNum)
        {
            this.Enqueue(new I2cCloseFrom(i2cNum));
        }

        public void HandleI2cResult(int i2cNum, int size, byte[] data)
        {
            this.Enqueue(new I2cResultFrom(i2cNum, size, data));
        }

        public void HandleI2cReportTxStatus(int i2cNum, int bytesRemaining)
        {
            this.Enqueue(new HandleI2cReportTxStatusFrom(i2cNum));
        }

        // default to close
        public void HandleIcspOpen()
        {
            this.Enqueue(new IcspOpenFrom());
        }

        public void HandleIcspClose()
        {
            this.Enqueue(new IcspCloseFrom());
        }

        public void HandleIcspReportRxStatus(int bytesRemaining)
        {
            this.Enqueue(new IcspReportRxStatusFrom(bytesRemaining));
        }

        public void HandleIcspResult(int size, byte[] data)
        {
            this.Enqueue(new IcspResultFrom(size, data));
        }

        public void HandleIncapReport(int incapNum, int size, byte[] data)
        {
            this.Enqueue(new IncapReportFrom(incapNum, size, data));
        }

        public void HandleIncapClose(int incapNum)
        {
            this.Enqueue(new IncapOpenFrom(incapNum));
        }

        public void HandleIncapOpen(int incapNum)
        {
            this.Enqueue(new IncapCloseFrom(incapNum));
        }

        public void HandleCapSenseReport(int pinNum, int value)
        {
            this.Enqueue(new CapSenseReportFrom(pinNum, value));
        }

        public void HandleSetCapSenseSampling(int pinNum, bool enable)
        {
            this.Enqueue(new CapSenseSamplingFrom(pinNum, enable));
        }

        public void HandleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            this.Enqueue(new SequencerEventFrom(seqEvent, arg));
        }

        public void HandleSync()
        {
        }
    }
}
