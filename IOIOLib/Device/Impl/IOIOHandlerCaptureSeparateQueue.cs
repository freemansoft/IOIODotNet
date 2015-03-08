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
using System.Collections.Concurrent;
using IOIOLib.IOIOException;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// TODO: This class should be a dispatcher for event listeners.
    /// </summary>
    public class IOIOHandlerCaptureSeparateQueue : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerCaptureSeparateQueue));
        /// <summary>
        /// response from the checkInterfaceResponse call 
        /// </summary>
        internal ISupportedInterfaceFrom Supported_;

        internal IEstablishConnectionFrom EstablishConnectionFrom_;

        public void HandleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            EstablishConnectionFrom_ = new EstablishConnectionFrom(
                System.Text.Encoding.ASCII.GetString(hardwareId),
                System.Text.Encoding.ASCII.GetString(bootloaderId),
                System.Text.Encoding.ASCII.GetString(firmwareId),
                Board.AllBoards[System.Text.Encoding.ASCII.GetString(hardwareId)]
                );
        }

        public void HandleConnectionLost()
        {
        }

        public void HandleSoftReset()
        {
        }

        public void HandleCheckInterfaceResponse(bool supported)
        {
            this.Supported_ = new SupportedInterfaceFrom(supported);
        }

        internal ConcurrentDictionary<Type, ConcurrentQueue<IMessageFromIOIO>> ClassifiedStorage_ = new ConcurrentDictionary<Type, ConcurrentQueue<IMessageFromIOIO>>();

        private void AddToClassifiedStorage(Type t, IMessageFromIOIO message)
        {
            // FIXME should verify message is of Type_ t
            if (!ClassifiedStorage_.ContainsKey(t))
            {
                ClassifiedStorage_.TryAdd(t, new ConcurrentQueue<IMessageFromIOIO>());
            }
            ConcurrentQueue<IMessageFromIOIO> ourQueue;
            bool gotIt = ClassifiedStorage_.TryGetValue(t, out ourQueue);
            if (gotIt)
            {
                ourQueue.Enqueue(message);
            }
        }

        internal ConcurrentQueue<IMessageFromIOIO> GetClassified(Type t)
        {
            if (!ClassifiedStorage_.ContainsKey(t))
            {
                ClassifiedStorage_.TryAdd(t, new ConcurrentQueue<IMessageFromIOIO>());
            }
            ConcurrentQueue<IMessageFromIOIO> ourQueue;
            bool gotIt = ClassifiedStorage_.TryGetValue(t, out ourQueue);
            if (gotIt)
            {
                return ourQueue;
            }
            else
            {
                throw new IllegalStateException("couldn't make Type_ work with classified storage " + t.Name);
            }
        }

        /// <summary>
        /// should this be classified with IDigitalInFrom
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="changeNotify"></param>
        public void HandleSetChangeNotify(int pin, bool changeNotify)
        {
            AddToClassifiedStorage(typeof(ISetChangeNotifyMessageFrom), new SetChangeNotifyMessageFrom(pin, changeNotify));
        }

        public void HandleReportDigitalInStatus(int pin, bool level)
        {
            AddToClassifiedStorage(typeof(IDigitalInFrom),
                new ReportDigitalInStatusFrom(pin, level));
        }

        public void HandleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            AddToClassifiedStorage(typeof(IDigitalInFrom),
                new RegisterPeriodicDigitalSamplingFrom(pin, freqScale));
        }

        public void HandleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            AddToClassifiedStorage(typeof(IDigitalInFrom),
                new ReportPeriodicDigitalInStatusFrom(frameNum, values));
        }

        public void HandleAnalogPinStatus(int pin, bool open)
        {
            AddToClassifiedStorage(typeof(IAnalogInFrom),
                new AnalogPinStatusFrom(pin, open));
        }

        public void HandleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            for (int i = 0; i < pins.Count; i++)
            {
                AddToClassifiedStorage(typeof(IAnalogInFrom),
                   new ReportAnalogPinValuesFrom(pins[i], values[i]));
            }
        }

        public void HandleUartOpen(int uartNum)
        {
            AddToClassifiedStorage(typeof(IUartFrom), new UartOpenFrom(uartNum));
        }

        public void HandleUartClose(int uartNum)
        {
            AddToClassifiedStorage(typeof(IUartFrom), new UartCloseFrom(uartNum));
        }

        public void HandleUartData(int uartNum, int numBytes, byte[] data)
        {
            AddToClassifiedStorage(typeof(IUartFrom), new UartDataFrom(uartNum, numBytes, data));
        }


        public void HandleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            AddToClassifiedStorage(typeof(IUartFrom), new HandleUartReportTxStatusFrom(uartNum, bytesRemaining));
        }

        public void HandleSpiOpen(int spiNum)
        {
            AddToClassifiedStorage(typeof(ISpiFrom), new SpiOpenFrom(spiNum));
        }

        public void HandleSpiClose(int spiNum)
        {
            AddToClassifiedStorage(typeof(ISpiFrom), new SpiCloseFrom(spiNum));
        }

        public void HandleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            AddToClassifiedStorage(typeof(ISpiFrom), new SpiDataFrom(spiNum, ssPin, data, dataBytes));
        }

        public void HandleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            AddToClassifiedStorage(typeof(ISpiFrom), new HandleSpiReportTxStatusFrom(spiNum, bytesRemaining));
        }

        public void HandleI2cOpen(int i2cNum)
        {
            AddToClassifiedStorage(typeof(II2cFrom), new I2cOpenFrom(i2cNum));
        }

        public void HandleI2cClose(int i2cNum)
        {
            AddToClassifiedStorage(typeof(II2cFrom), new I2cCloseFrom(i2cNum));
        }

        public void HandleI2cResult(int i2cNum, int size, byte[] data)
        {
            AddToClassifiedStorage(typeof(II2cFrom), new I2cResultFrom(i2cNum, size, data));
        }

        public void HandleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
            AddToClassifiedStorage(typeof(II2cFrom), new HandleI2cReportTxStatusFrom(spiNum));
        }

        // default to close
        internal IIcspFrom StateIcsp_ = new IcspCloseFrom();
        public void HandleIcspOpen()
        {
            AddToClassifiedStorage(typeof(IIcspFrom), new IcspOpenFrom());
        }

        public void HandleIcspClose()
        {
            AddToClassifiedStorage(typeof(IIcspFrom), new IcspCloseFrom());
        }

        public void HandleIcspReportRxStatus(int bytesRemaining)
        {
            AddToClassifiedStorage(typeof(IIcspFrom), new IcspReportRxStatusFrom(bytesRemaining));
        }

        public void HandleIcspResult(int size, byte[] data)
        {
            AddToClassifiedStorage(typeof(IIcspFrom), new IcspResultFrom(size, data));
        }

        public void HandleIncapReport(int incapNum, int size, byte[] data)
        {
            AddToClassifiedStorage(typeof(IIncapFrom), new IncapReportFrom(incapNum, size, data));
        }

        public void HandleIncapClose(int incapNum)
        {
            AddToClassifiedStorage(typeof(IIncapFrom), new IncapOpenFrom(incapNum));
        }

        public void HandleIncapOpen(int incapNum)
        {
            AddToClassifiedStorage(typeof(IIncapFrom), new IncapCloseFrom(incapNum));
        }

        public void HandleCapSenseReport(int pinNum, int value)
        {
            AddToClassifiedStorage(typeof(ICapSenseFrom), new CapSenseReportFrom(pinNum, value));
        }

        public void HandleSetCapSenseSampling(int pinNum, bool enable)
        {
            AddToClassifiedStorage(typeof(ICapSenseFrom), new CapSenseSamplingFrom(pinNum, enable));
        }

        public void HandleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            AddToClassifiedStorage(typeof(ISequencerEventFrom), new SequencerEventFrom(seqEvent, arg));
        }

        public void HandleSync()
        {
        }
    }
}
