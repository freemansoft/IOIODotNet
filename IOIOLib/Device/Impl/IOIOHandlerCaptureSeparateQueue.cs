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

        public void handleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            EstablishConnectionFrom_ = new EstablishConnectionFrom(
                System.Text.Encoding.ASCII.GetString(hardwareId),
                System.Text.Encoding.ASCII.GetString(bootloaderId),
                System.Text.Encoding.ASCII.GetString(firmwareId),
                Board.AllBoards[System.Text.Encoding.ASCII.GetString(hardwareId)]
                );
        }

        public void handleConnectionLost()
        {
        }

        public void handleSoftReset()
        {
        }

        public void handleCheckInterfaceResponse(bool supported)
        {
            this.Supported_ = new SupportedInterfaceFrom(supported);
        }

        internal ConcurrentDictionary<Type, ConcurrentQueue<IMessageFromIOIO>> ClassifiedStorage_ = new ConcurrentDictionary<Type, ConcurrentQueue<IMessageFromIOIO>>();

        private void AddToClassifiedStorage(Type t, IMessageFromIOIO message)
        {
            // FIXME should verify message is of type t
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
                throw new IllegalStateException("couldn't make type work with classified storage " + t.Name);
            }
        }

        /// <summary>
        /// should this be classified with IDigitalInFrom
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="changeNotify"></param>
        public void handleSetChangeNotify(int pin, bool changeNotify)
        {
            AddToClassifiedStorage(typeof(ISetChangeNotifyMessageFrom), new SetChangeNotifyMessageFrom(pin, changeNotify));
        }

        public void handleReportDigitalInStatus(int pin, bool level)
        {
            AddToClassifiedStorage(typeof(IDigitalInFrom),
                new ReportDigitalInStatusFrom(pin, level));
        }

        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            AddToClassifiedStorage(typeof(IDigitalInFrom),
                new RegisterPeriodicDigitalSamplingFrom(pin, freqScale));
        }

        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            AddToClassifiedStorage(typeof(IDigitalInFrom),
                new ReportPeriodicDigitalInStatusFrom(frameNum, values));
        }

        public void handleAnalogPinStatus(int pin, bool open)
        {
            AddToClassifiedStorage(typeof(IAnalogInFrom),
                new AnalogPinStatusFrom(pin, open));
        }

        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            for (int i = 0; i < pins.Count; i++)
            {
                AddToClassifiedStorage(typeof(IAnalogInFrom),
                   new ReportAnalogPinValuesFrom(pins[i], values[i]));
            }
        }

        public void handleUartOpen(int uartNum)
        {
            AddToClassifiedStorage(typeof(IUartFrom), new UartOpenFrom(uartNum));
        }

        public void handleUartClose(int uartNum)
        {
            AddToClassifiedStorage(typeof(IUartFrom), new UartCloseFrom(uartNum));
        }

        public void handleUartData(int uartNum, int numBytes, byte[] data)
        {
            AddToClassifiedStorage(typeof(IUartFrom), new UartDataFrom(uartNum, numBytes, data));
        }


        public void handleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            AddToClassifiedStorage(typeof(IUartFrom), new HandleUartReportTxStatusFrom(uartNum, bytesRemaining));
        }

        public void handleSpiOpen(int spiNum)
        {
            AddToClassifiedStorage(typeof(ISpiFrom), new SpiOpenFrom(spiNum));
        }

        public void handleSpiClose(int spiNum)
        {
            AddToClassifiedStorage(typeof(ISpiFrom), new SpiCloseFrom(spiNum));
        }

        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            AddToClassifiedStorage(typeof(ISpiFrom), new SpiDataFrom(spiNum, ssPin, data, dataBytes));
        }

        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            AddToClassifiedStorage(typeof(ISpiFrom), new HandleSpiReportTxStatusFrom(spiNum, bytesRemaining));
        }

        public void handleI2cOpen(int i2cNum)
        {
            AddToClassifiedStorage(typeof(II2cFrom), new I2cOpenFrom(i2cNum));
        }

        public void handleI2cClose(int i2cNum)
        {
            AddToClassifiedStorage(typeof(II2cFrom), new I2cCloseFrom(i2cNum));
        }

        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
            AddToClassifiedStorage(typeof(II2cFrom), new I2cResultFrom(i2cNum, size, data));
        }

        public void handleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
            AddToClassifiedStorage(typeof(II2cFrom), new HandleI2cReportTxStatusFrom(spiNum));
        }

        // default to close
        internal IIcspFrom StateIcsp_ = new IcspCloseFrom();
        public void handleIcspOpen()
        {
            AddToClassifiedStorage(typeof(IIcspFrom), new IcspOpenFrom());
        }

        public void handleIcspClose()
        {
            AddToClassifiedStorage(typeof(IIcspFrom), new IcspCloseFrom());
        }

        public void handleIcspReportRxStatus(int bytesRemaining)
        {
            AddToClassifiedStorage(typeof(IIcspFrom), new IcspReportRxStatusFrom(bytesRemaining));
        }

        public void handleIcspResult(int size, byte[] data)
        {
            AddToClassifiedStorage(typeof(IIcspFrom), new IcspResultFrom(size, data));
        }

        public void handleIncapReport(int incapNum, int size, byte[] data)
        {
            AddToClassifiedStorage(typeof(IIncapFrom), new IncapReportFrom(incapNum, size, data));
        }

        public void handleIncapClose(int incapNum)
        {
            AddToClassifiedStorage(typeof(IIncapFrom), new IncapOpenFrom(incapNum));
        }

        public void handleIncapOpen(int incapNum)
        {
            AddToClassifiedStorage(typeof(IIncapFrom), new IncapCloseFrom(incapNum));
        }

        public void handleCapSenseReport(int pinNum, int value)
        {
            AddToClassifiedStorage(typeof(ICapSenseFrom), new CapSenseReportFrom(pinNum, value));
        }

        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
            AddToClassifiedStorage(typeof(ICapSenseFrom), new CapSenseSamplingFrom(pinNum, enable));
        }

        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            AddToClassifiedStorage(typeof(ISequencerEventFrom), new SequencerEventFrom(seqEvent, arg));
        }

        public void handleSync()
        {
        }
    }
}
