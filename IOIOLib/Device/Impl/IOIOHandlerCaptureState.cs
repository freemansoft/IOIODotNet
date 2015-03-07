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
    /// TODO: This class should be a dispatcher for event listeners.
    /// </summary>
    public class IOIOHandlerCaptureState : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerCaptureState));
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

        internal IDictionary<int, ISetChangeNotifyMessageFrom> StateSetChangeNotify_ = new Dictionary<int, ISetChangeNotifyMessageFrom>();
        public void handleSetChangeNotify(int pin, bool changeNotify)
        {
            StateSetChangeNotify_.Add(pin, new SetChangeNotifyMessageFrom(pin, changeNotify));
        }

        internal IDictionary<int, IReportDigitalInStatusFrom> StateReportDigitalInStatus_ = new Dictionary<int, IReportDigitalInStatusFrom>();
        public void handleReportDigitalInStatus(int pin, bool level)
        {
            StateReportDigitalInStatus_.Add(pin, new ReportDigitalInStatusFrom(pin, level));
        }

        internal IDictionary<int, IRegisterPeriodicDigitalSamplingFrom> StatePeriodicDigitalSampling_ = new Dictionary<int, IRegisterPeriodicDigitalSamplingFrom>();
        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            StatePeriodicDigitalSampling_.Add(pin, new RegisterPeriodicDigitalSamplingFrom(pin, freqScale));
        }

        internal IDictionary<int, IReportPeriodicDigitalInStatusFrom> StateReportPeriodicDigitalInStatus_ = new Dictionary<int, IReportPeriodicDigitalInStatusFrom>();
        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            StateReportPeriodicDigitalInStatus_.Add(frameNum, new ReportPeriodicDigitalInStatusFrom(frameNum, values));
        }

        internal IDictionary<int, IAnalogPinStatusFrom> StateAnalogPinStatus_ = new Dictionary<int, IAnalogPinStatusFrom>();
        public void handleAnalogPinStatus(int pin, bool open)
        {
            StateAnalogPinStatus_.Add(pin, new AnalogPinStatusFrom(pin, open));
        }

        internal IDictionary<int, IReportAnalogPinValuesFrom> StateReportAnalogInStatus_ = new Dictionary<int, IReportAnalogPinValuesFrom>();
        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            for (int i = 0; i < pins.Count; i++)
            {
                StateReportAnalogInStatus_.Add(pins[i], new ReportAnalogPinValuesFrom(pins[i], values[i]));
            }
        }

        /// <summary>
        ///  empty or close means closed
        ///  IsOpen means IsOpen
        /// </summary>
        internal IDictionary<int, IUartFrom> StateUart_ = new Dictionary<int, IUartFrom>();
        public void handleUartOpen(int uartNum)
        {
            StateUart_.Add(uartNum, new UartOpenFrom(uartNum));
        }

        public void handleUartClose(int uartNum)
        {
            StateUart_.Add(uartNum, new UartCloseFrom(uartNum));
        }

        internal IDictionary<int, IUartDataFrom> StateUartData_ = new Dictionary<int, IUartDataFrom>();
        public void handleUartData(int uartNum, int numBytes, byte[] data)
        {
            StateUartData_.Add(uartNum, new UartDataFrom(uartNum, numBytes, data));
        }


        internal IDictionary<int, IHandleUartReportTxStatusFrom> StatehandleUartReportTxStatus_ = new Dictionary<int, IHandleUartReportTxStatusFrom>();
        public void handleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            StatehandleUartReportTxStatus_.Add(uartNum, new HandleUartReportTxStatusFrom(uartNum, bytesRemaining));
        }

        internal IDictionary<int, ISpiFrom> StateSpi_ = new Dictionary<int, ISpiFrom>();
        public void handleSpiOpen(int spiNum)
        {
            StateSpi_.Add(spiNum, new SpiOpenFrom(spiNum));
        }

        public void handleSpiClose(int spiNum)
        {
            StateSpi_.Add(spiNum, new SpiCloseFrom(spiNum));
        }

        internal IDictionary<int, ISpiDataFrom> StateSpiData_ = new Dictionary<int, ISpiDataFrom>();
        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            StateSpiData_.Add(spiNum, new SpiDataFrom(spiNum, ssPin, data, dataBytes));
        }

        internal IDictionary<int, IHandleSpiReportTxStatusFrom> StatehandleSpiReportTxStatus_ = new Dictionary<int, IHandleSpiReportTxStatusFrom>();
        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            StatehandleSpiReportTxStatus_.Add(spiNum, new HandleSpiReportTxStatusFrom(spiNum, bytesRemaining));
        }

        internal IDictionary<int, II2cFrom> StateI2c_ = new Dictionary<int, II2cFrom>();
        public void handleI2cOpen(int i2cNum)
        {
            StateI2c_.Add(i2cNum, new I2cOpenFrom(i2cNum));
        }

        public void handleI2cClose(int i2cNum)
        {
            StateI2c_.Add(i2cNum, new I2cCloseFrom(i2cNum));
        }

        internal IDictionary<int, II2cResultFrom> StateI2cResult_ = new Dictionary<int, II2cResultFrom>();
        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
            StateI2cResult_.Add(i2cNum, new I2cResultFrom(i2cNum, size, data));
        }

        internal IDictionary<int, IHandleI2cReportTxStatusFrom> StateHandleI2cReportTxStatus_ = new Dictionary<int, IHandleI2cReportTxStatusFrom>();
        public void handleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
            StateHandleI2cReportTxStatus_.Add(spiNum, new HandleI2cReportTxStatusFrom(spiNum));
        }

        // default to close
        internal IIcspFrom StateIcsp_ = new IcspCloseFrom();
        public void handleIcspOpen()
        {
            StateIcsp_ = new IcspOpenFrom();
        }

        public void handleIcspClose()
        {
            StateIcsp_ = new IcspCloseFrom();
        }

        internal IIcspReportRxStatusFrom IcspReportRxStatus_ = new IcspReportRxStatusFrom(-1);
        public void handleIcspReportRxStatus(int bytesRemaining)
        {
            IcspReportRxStatus_ = new IcspReportRxStatusFrom(bytesRemaining);
        }

        internal IIcspResultFrom StateIcspResult_ = new IcspResultFrom(-1, null);
        public void handleIcspResult(int size, byte[] data)
        {
            StateIcspResult_ = new IcspResultFrom(size, data);
        }

        internal IDictionary<int, IIncapReportFrom> StateIncapReport_ = new Dictionary<int, IIncapReportFrom>();
        public void handleIncapReport(int incapNum, int size, byte[] data)
        {
            StateIncapReport_.Add(incapNum, new IncapReportFrom(incapNum, size, data));
        }

        internal IDictionary<int, IIncapFrom> StateIncapOpen_ = new Dictionary<int, IIncapFrom>();
        public void handleIncapClose(int incapNum)
        {
            StateIncapOpen_.Add(incapNum, new IncapOpenFrom(incapNum));
        }

        public void handleIncapOpen(int incapNum)
        {
            StateIncapOpen_.Add(incapNum, new IncapCloseFrom(incapNum));
        }

        internal IDictionary<int, ICapSenseReportFrom> StateCapSenseReport_ = new Dictionary<int, ICapSenseReportFrom>();
        public void handleCapSenseReport(int pinNum, int value)
        {
            StateCapSenseReport_.Add(pinNum, new CapSenseReportFrom(pinNum, value));
        }

        internal IDictionary<int, ICapSenseSamplingFrom> StateCapSenseSampling_ = new Dictionary<int, ICapSenseSamplingFrom>();
        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
            StateCapSenseSampling_.Add(pinNum, new CapSenseSamplingFrom(pinNum, enable));
        }

        internal IDictionary<Types.SequencerEvent, ISequencerEventFrom> StateSequencerEvent_ = new Dictionary<Types.SequencerEvent, ISequencerEventFrom>();
        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            StateSequencerEvent_.Add(seqEvent, new SequencerEventFrom(seqEvent, arg));
        }

        public void handleSync()
        {
        }
    }
}
