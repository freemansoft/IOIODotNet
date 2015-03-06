using IOIOLib.Device.Types;
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
        /// provided by IOIO when connects
        /// </summary>
        internal string HardwareId_;
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        internal string BootloaderId_;
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        internal string FirmwareId_;
        /// <summary>
        /// response from the checkInterfaceResponse call 
        /// </summary>
        internal bool Supported_;
        /// <summary>
        /// should we have a variable here or just look it up each time?
        /// this should probably not exist.  It should be posted as part of event to listeners
        /// </summary>
        internal Hardware OurHardware_ = null;

        public void handleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            this.HardwareId_ = System.Text.Encoding.ASCII.GetString(hardwareId);
            this.BootloaderId_ = System.Text.Encoding.ASCII.GetString(bootloaderId);
            this.FirmwareId_ = System.Text.Encoding.ASCII.GetString(firmwareId);
            OurHardware_ = Board.AllBoards[System.Text.Encoding.ASCII.GetString(hardwareId)];
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

        internal IDictionary<int, Tuple<int, bool>> StateSetChangeNotify_ = new Dictionary<int, Tuple<int, bool>>();
        public void handleSetChangeNotify(int pin, bool changeNotify)
        {
            StateSetChangeNotify_.Add(pin, new Tuple<int, bool>(pin, changeNotify));
        }

        internal IDictionary<int, Tuple<int, bool>> StateReportDigitalInStatus_ = new Dictionary<int, Tuple<int, bool>>();
        public void handleReportDigitalInStatus(int pin, bool level)
        {
            StateReportDigitalInStatus_.Add(pin, new Tuple<int, bool>(pin, level));
        }

        internal IDictionary<int, Tuple<int, int>> StatePeriodicDigitalSampling_ = new Dictionary<int, Tuple<int, int>>();
        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            StatePeriodicDigitalSampling_.Add(pin, new Tuple<int, int>(pin, freqScale));
        }

        internal IDictionary<int, Tuple<int, bool[]>> StateReportPeriodicDigitalInStatus_ = new Dictionary<int, Tuple<int, bool[]>>();
        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            StateReportPeriodicDigitalInStatus_.Add(frameNum, new Tuple<int, bool[]>(frameNum, values));
        }

        internal IDictionary<int, Tuple<int, bool>> StateAnalogPinStatus_ = new Dictionary<int, Tuple<int, bool>>();
        public void handleAnalogPinStatus(int pin, bool open)
        {
            StateAnalogPinStatus_.Add(pin, new Tuple<int, bool>(pin, open));
        }

        internal IDictionary<int, Tuple<int, int>> StateReportAnalogInStatus_ = new Dictionary<int, Tuple<int, int>>();
        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            for (int i = 0; i < pins.Count; i++)
            {
                StateReportAnalogInStatus_.Add(pins[i], new Tuple<int, int>(pins[i], values[i]));
            }
        }

        internal IDictionary<int, int> StateUartOpen_ = new Dictionary<int, int>();
        public void handleUartOpen(int uartNum)
        {
            StateUartOpen_.Add(uartNum, uartNum);
        }

        public void handleUartClose(int uartNum)
        {
            if (StateUartOpen_.ContainsKey(uartNum))
            {
                StateUartOpen_.Remove(uartNum);
            }
        }

        internal IDictionary<int, Tuple<int, int, byte[]>> StateUartData_ = new Dictionary<int, Tuple<int, int, byte[]>>();
        public void handleUartData(int uartNum, int numBytes, byte[] data)
        {
            StateUartData_.Add(uartNum, new Tuple<int, int, byte[]>(uartNum, numBytes, data));
        }

        public void handleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
        }

        internal IDictionary<int, int> StateSpiOpen_ = new Dictionary<int, int>();
        public void handleSpiOpen(int spiNum)
        {
            StateSpiOpen_.Add(spiNum, spiNum);
        }

        public void handleSpiClose(int spiNum)
        {
            if (StateSpiOpen_.ContainsKey(spiNum))
            {
                StateSpiOpen_.Remove(spiNum);
            }
        }

        internal IDictionary<int, Tuple<int, int, byte[], int>> StateSpiData_ = new Dictionary<int, Tuple<int, int, byte[], int>>();
        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            StateSpiData_.Add(spiNum, new Tuple<int, int, byte[], int>(spiNum, ssPin, data, dataBytes));
        }

        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        internal IDictionary<int, int> StateI2cOpen_ = new Dictionary<int, int>();
        public void handleI2cOpen(int i2cNum)
        {
            StateI2cOpen_.Add(i2cNum, i2cNum);
        }

        public void handleI2cClose(int i2cNum)
        {
            if (StateI2cOpen_.ContainsKey(i2cNum))
            {
                StateI2cOpen_.Remove(i2cNum);
            }
        }

        internal IDictionary<int, Tuple<int, int, byte[]>> StateI2cResult_ = new Dictionary<int, Tuple<int, int, byte[]>>();
        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
            StateI2cResult_.Add(i2cNum, new Tuple<int, int, byte[]>(i2cNum, size, data));
        }

        public void handleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        internal Boolean StateIcspOpen_ = false;
        public void handleIcspOpen()
        {
            StateIcspOpen_ = true;
        }

        public void handleIcspClose()
        {
            StateIcspOpen_ = false;
        }

        public void handleIcspReportRxStatus(int bytesRemaining)
        {
        }

        internal Tuple<int, byte[]> StateIcspResult_ = new Tuple<int, byte[]>(-1, null);
        public void handleIcspResult(int size, byte[] data)
        {
            StateIcspResult_ = new Tuple<int, byte[]>(size, data);
        }

        internal IDictionary<int, Tuple<int, int, byte[]>> StateIncapReport_ = new Dictionary<int, Tuple<int, int, byte[]>>();
        public void handleIncapReport(int incapNum, int size, byte[] data)
        {
            StateIncapReport_.Add(incapNum, new Tuple<int, int, byte[]>(incapNum, size, data));
        }

        internal IDictionary<int, int> StateIncapOpen_ = new Dictionary<int, int>();
        public void handleIncapClose(int incapNum)
        {
            if (StateIncapOpen_.ContainsKey(incapNum))
            {
                StateIncapOpen_.Remove(incapNum);
            }
        }

        public void handleIncapOpen(int incapNum)
        {
            StateIncapOpen_.Add(incapNum, incapNum);
        }

        internal IDictionary<int, Tuple<int, int>> StateCapSenseReport_ = new Dictionary<int, Tuple<int, int>>();
        public void handleCapSenseReport(int pinNum, int value)
        {
            StateCapSenseReport_.Add(pinNum, new Tuple<int, int>(pinNum, value));
        }

        internal IDictionary<int, Tuple<int, bool>> StateCapSenseSampling_ = new Dictionary<int, Tuple<int, bool>>();
        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
            StateCapSenseSampling_.Add(pinNum, new Tuple<int, bool>(pinNum, enable));
        }

        internal IDictionary<Types.SequencerEvent, Tuple<Types.SequencerEvent, int>> StateSequencerEvent_ = new Dictionary<Types.SequencerEvent, Tuple<Types.SequencerEvent, int>>();
        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            StateSequencerEvent_.Add(seqEvent, new Tuple<SequencerEvent, int>(seqEvent, arg));
        }

        public void handleSync()
        {
        }
    }
}
