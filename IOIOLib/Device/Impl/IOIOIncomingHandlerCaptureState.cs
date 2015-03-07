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
    public class IOIOIncomingHandlerCaptureState : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOIncomingHandlerCaptureState));
        /// <summary>
        /// response from the checkInterfaceResponse call 
        /// </summary>
        internal bool Supported_;

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
            this.Supported_ = supported;
        }

        internal IDictionary<int, IStateSetChangeNotifyMessageFrom> StateSetChangeNotify_ = new Dictionary<int, IStateSetChangeNotifyMessageFrom>();
        public void handleSetChangeNotify(int pin, bool changeNotify)
        {
            StateSetChangeNotify_.Add(pin, new StateSetChangeNotifyMessageFrom(pin, changeNotify));
        }

        internal IDictionary<int, IStateReportDigitalInStatusFrom> StateReportDigitalInStatus_ = new Dictionary<int, IStateReportDigitalInStatusFrom>();
        public void handleReportDigitalInStatus(int pin, bool level)
        {
            StateReportDigitalInStatus_.Add(pin, new StateReportDigitalInStatusFrom(pin, level));
        }

        internal IDictionary<int, IStatePeriodicDigitalSamplingFrom> StatePeriodicDigitalSampling_ = new Dictionary<int, IStatePeriodicDigitalSamplingFrom>();
        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            StatePeriodicDigitalSampling_.Add(pin, new StatePeriodicDigitalSamplingFrom(pin, freqScale));
        }

        internal IDictionary<int, IStateReportPeriodicDigitalInStatusFrom> StateReportPeriodicDigitalInStatus_ = new Dictionary<int, IStateReportPeriodicDigitalInStatusFrom>();
        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            StateReportPeriodicDigitalInStatus_.Add(frameNum, new StateReportPeriodicDigitalInStatusFrom(frameNum, values));
        }

        internal IDictionary<int, Tuple<int, bool>> StateAnalogPinStatus_ = new Dictionary<int, Tuple<int, bool>>();
        public void handleAnalogPinStatus(int pin, bool open)
        {
            StateAnalogPinStatus_.Add(pin, new Tuple<int, bool>(pin, open));
        }

        internal IDictionary<int, IStateAnalogPinStatusFrom> StateReportAnalogInStatus_ = new Dictionary<int, IStateAnalogPinStatusFrom>();
        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            for (int i = 0; i < pins.Count; i++)
            {
                StateReportAnalogInStatus_.Add(pins[i], new StateAnalogPinStatusFrom(pins[i], values[i]));
            }
        }

        /// <summary>
        ///  empty or close means closed
        ///  open means open
        /// </summary>
        internal IDictionary<int, IStateUartFrom> StateUart_ = new Dictionary<int, IStateUartFrom>();
        public void handleUartOpen(int uartNum)
        {
            StateUart_.Add(uartNum, new StateUartOpenFrom(uartNum));
        }

        public void handleUartClose(int uartNum)
        {
            StateUart_.Add(uartNum, new StateUartCloseFrom(uartNum));
        }

        internal IDictionary<int, Tuple<int, int, byte[]>> StateUartData_ = new Dictionary<int, Tuple<int, int, byte[]>>();
        public void handleUartData(int uartNum, int numBytes, byte[] data)
        {
            StateUartData_.Add(uartNum, new Tuple<int, int, byte[]>(uartNum, numBytes, data));
        }


        internal IDictionary<int, IStateHandleUartReportTxStatusFrom> StatehandleUartReportTxStatus_ = new Dictionary<int, IStateHandleUartReportTxStatusFrom>();
        public void handleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            StatehandleUartReportTxStatus_.Add(uartNum, new StateHandleUartReportTxStatusFrom(uartNum, bytesRemaining));
        }

        internal IDictionary<int, IStateSpiFrom> StateSpi_ = new Dictionary<int, IStateSpiFrom>();
        public void handleSpiOpen(int spiNum)
        {
            StateSpi_.Add(spiNum, new StateSpiOpenFrom(spiNum));
        }

        public void handleSpiClose(int spiNum)
        {
            StateSpi_.Add(spiNum, new StateSpiCloseFrom(spiNum));
        }

        internal IDictionary<int, IStateSpiDataFrom> StateSpiData_ = new Dictionary<int, IStateSpiDataFrom>();
        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            StateSpiData_.Add(spiNum, new StateSpiDataFrom(spiNum, ssPin, data, dataBytes));
        }

        internal IDictionary<int, IStateHandleSpiReportTxStatusFrom> StatehandleSpiReportTxStatus_ = new Dictionary<int, IStateHandleSpiReportTxStatusFrom>();
        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            StatehandleSpiReportTxStatus_.Add(spiNum, new StateHandleSpiReportTxStatusFrom(spiNum, bytesRemaining));
        }

        internal IDictionary<int, IStateI2cFrom> StateI2c_ = new Dictionary<int, IStateI2cFrom>();
        public void handleI2cOpen(int i2cNum)
        {
            StateI2c_.Add(i2cNum, new StateI2cOpenFrom(i2cNum));
        }

        public void handleI2cClose(int i2cNum)
        {
            StateI2c_.Add(i2cNum, new StateI2cCloseFrom(i2cNum));
        }

        internal IDictionary<int, IStateI2cResultFrom> StateI2cResult_ = new Dictionary<int, IStateI2cResultFrom>();
        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
            StateI2cResult_.Add(i2cNum, new StateI2cResultFrom(i2cNum, size, data));
        }

        internal IDictionary<int, IStateHandleI2cReportTxStatusFrom> StateHandleI2cReportTxStatus_ = new Dictionary<int, IStateHandleI2cReportTxStatusFrom>();
        public void handleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
            StateHandleI2cReportTxStatus_.Add(spiNum, new StateHandleI2cReportTxStatusFrom(spiNum));
        }

        // default to close
        internal IStateIcspFrom StateIcsp_ = new StateIcspCloseFrom();
        public void handleIcspOpen()
        {
            StateIcsp_ = new StateIcspOpenFrom();
        }

        public void handleIcspClose()
        {
            StateIcsp_ = new StateIcspCloseFrom();
        }

        internal IIcspReportRxStatusFrom IcspReportRxStatus_ = new IcspReportRxStatusFrom(-1);
        public void handleIcspReportRxStatus(int bytesRemaining)
        {
            IcspReportRxStatus_ = new IcspReportRxStatusFrom(bytesRemaining);
        }

        internal IStateIcspResultFrom StateIcspResult_ = new StateIcspResultFrom(-1, null);
        public void handleIcspResult(int size, byte[] data)
        {
            StateIcspResult_ = new StateIcspResultFrom(size, data);
        }

        internal IDictionary<int, IStateIncapReportFrom> StateIncapReport_ = new Dictionary<int, IStateIncapReportFrom>();
        public void handleIncapReport(int incapNum, int size, byte[] data)
        {
            StateIncapReport_.Add(incapNum, new StateIncapReportFrom(incapNum, size, data));
        }

        internal IDictionary<int, IStateIncapFrom> StateIncapOpen_ = new Dictionary<int, IStateIncapFrom>();
        public void handleIncapClose(int incapNum)
        {
            StateIncapOpen_.Add(incapNum, new StateIncapOpenFrom(incapNum));
        }

        public void handleIncapOpen(int incapNum)
        {
            StateIncapOpen_.Add(incapNum, new StateIncapCloseFrom(incapNum));
        }

        internal IDictionary<int, IStateCapSenseReportFrom> StateCapSenseReport_ = new Dictionary<int, IStateCapSenseReportFrom>();
        public void handleCapSenseReport(int pinNum, int value)
        {
            StateCapSenseReport_.Add(pinNum, new StateCapSenseReportFrom(pinNum, value));
        }

        internal IDictionary<int, IStateCapSenseSamplingFrom> StateCapSenseSampling_ = new Dictionary<int, IStateCapSenseSamplingFrom>();
        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
            StateCapSenseSampling_.Add(pinNum, new StateCapSenseSamplingFrom(pinNum, enable));
        }

        internal IDictionary<Types.SequencerEvent, IStateSequencerEventFrom> StateSequencerEvent_ = new Dictionary<Types.SequencerEvent, IStateSequencerEventFrom>();
        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            StateSequencerEvent_.Add(seqEvent, new StateSequencerEventFrom(seqEvent, arg));
        }

        public void handleSync()
        {
        }
    }
}
