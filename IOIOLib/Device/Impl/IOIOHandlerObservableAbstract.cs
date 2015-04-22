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
using System.Collections;
using IOIOLib.Component.Types;
using IOIOLib.Message;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// This class leaks if you don't read the messages it captures
    /// </summary>
    public abstract class IOIOHandlerObservableAbstract : IIncomingHandlerIOIO, IObservableIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerObservableAbstract));

        internal ConcurrentBag<IObserverIOIO> Interested_ = new ConcurrentBag<IObserverIOIO>();

        /// <summary>
        /// Does not yet return an actual unsubscriber
        /// </summary>
        /// <param name="t">the specific subtype of IMessageFromIOIO you are interested in</param>
        /// <param name="observer">object that wishes to be notified</param>
        /// <returns></returns>
        public virtual IDisposable Subscribe(IObserverIOIO observer)
        {
            Interested_.Add(observer);
            return null;
        }

        /// <summary>
        /// the one method subclass ned to implement
        /// </summary>
        /// <param name="message"></param>
		public abstract void HandleMessage(IMessageIOIO message);


		public virtual void HandleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            IConnectedDeviceResponse EstablishConnectionFrom_ = new ConnectedDeviceResponse(
                System.Text.Encoding.ASCII.GetString(hardwareId),
                System.Text.Encoding.ASCII.GetString(bootloaderId),
                System.Text.Encoding.ASCII.GetString(firmwareId),
                Board.AllBoards[System.Text.Encoding.ASCII.GetString(hardwareId)]
                );
            this.HandleMessage(EstablishConnectionFrom_);
        }

        public virtual void HandleConnectionLost()
        {
            // need to implement ConnectionLostFrom
        }

        public virtual void HandleSoftReset()
        {
            // need to implement SoftResetFrom
        }

        public virtual void HandleCheckInterfaceResponse(bool supported)
        {
            this.HandleMessage(new SupportedInterfaceFrom(supported));
        }

        public virtual void HandleSetChangeNotify(int pin, bool changeNotify)
        {
            this.HandleMessage(new SetChangeNotifyMessageFrom(pin, changeNotify));
        }

        public virtual void HandleReportDigitalInStatus(int pin, bool level)
        {
            this.HandleMessage(new ReportDigitalInStatusFrom(pin, level));
        }

        public virtual void HandleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            this.HandleMessage(new RegisterPeriodicDigitalSamplingFrom(pin, freqScale));
        }

        public virtual void HandleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            this.HandleMessage(new ReportPeriodicDigitalInStatusFrom(frameNum, values));
        }

        public virtual void HandleAnalogPinStatus(int pin, bool open)
        {
            this.HandleMessage(new AnalogPinStatusFrom(pin, open));
        }

        public virtual void HandleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            if (pins.Count != values.Count)
            {
                LOG.Warn("HandleReportAnalogInStatus has pins:" + pins.Count + " Values:" + values.Count);
            }
            for (int i = 0; i < pins.Count; i++)
            {
                this.HandleMessage(new ReportAnalogPinValuesFrom(pins[i], values[i]));
            }
        }

        /// <summary>
        ///  empty or close means closed
        ///  IsOpen means IsOpen
        /// </summary>
        public virtual void HandleUartOpen(int uartNum)
        {
            this.HandleMessage(new UartOpenFrom(uartNum));
        }

        public virtual void HandleUartClose(int uartNum)
        {
            this.HandleMessage(new UartCloseFrom(uartNum));
        }

        public virtual void HandleUartData(int uartNum, int numBytes, byte[] data)
        {
            this.HandleMessage(new UartDataFrom(uartNum, numBytes, data));
        }


        public virtual void HandleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            this.HandleMessage(new UartReportTxStatusFrom(uartNum, bytesRemaining));
        }

        public virtual void HandleSpiOpen(int spiNum)
        {
            this.HandleMessage(new SpiOpenFrom(spiNum));
        }

        public virtual void HandleSpiClose(int spiNum)
        {
            this.HandleMessage(new SpiCloseFrom(spiNum));
        }

        public virtual void HandleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            this.HandleMessage(new SpiDataFrom(spiNum, ssPin, data, dataBytes));
        }

        public virtual void HandleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            this.HandleMessage(new SpiReportTxStatusFrom(spiNum, bytesRemaining));
        }

        public virtual void HandleI2cOpen(int i2cNum)
        {
            this.HandleMessage(new I2cOpenFrom(i2cNum));
        }

        public virtual void HandleI2cClose(int i2cNum)
        {
            this.HandleMessage(new I2cCloseFrom(i2cNum));
        }

        public virtual void HandleI2cResult(int i2cNum, int size, byte[] data)
        {
            this.HandleMessage(new I2cResultFrom(i2cNum, size, data));
        }

        public virtual void HandleI2cReportTxStatus(int i2cNum, int bytesRemaining)
        {
            this.HandleMessage(new I2cReportTxStatusFrom(i2cNum, bytesRemaining));
        }

        // default to close
        public virtual void HandleIcspOpen()
        {
            this.HandleMessage(new IcspOpenFrom());
        }

        public virtual void HandleIcspClose()
        {
            this.HandleMessage(new IcspCloseFrom());
        }

        public virtual void HandleIcspReportRxStatus(int bytesRemaining)
        {
            this.HandleMessage(new IcspReportRxStatusFrom(bytesRemaining));
        }

        public virtual void HandleIcspResult(int size, byte[] data)
        {
            this.HandleMessage(new IcspResultFrom(size, data));
        }

        public virtual void HandleIncapReport(int incapNum, int size, byte[] data)
        {
            this.HandleMessage(new IncapReportFrom(incapNum, size, data));
        }

        public virtual void HandleIncapClose(int incapNum)
        {
            this.HandleMessage(new IncapOpenFrom(incapNum));
        }

        public virtual void HandleIncapOpen(int incapNum)
        {
            this.HandleMessage(new IncapCloseFrom(incapNum));
        }

        public virtual void HandleCapSenseReport(int pinNum, int value)
        {
            this.HandleMessage(new CapSenseReportFrom(pinNum, value));
        }

        public virtual void HandleSetCapSenseSampling(int pinNum, bool enable)
        {
            this.HandleMessage(new CapSenseSamplingFrom(pinNum, enable));
        }

        public virtual void HandleSequencerEvent(SequencerEventState seqEvent, int arg)
        {
            this.HandleMessage(new SequencerEventFrom(seqEvent, arg));
        }

        public virtual void HandleSync()
        {
        }

	}
}
