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
        private byte[] HardwareId;
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        private byte[] BootloaderId;
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        private byte[] FirmwareId;
        /// <summary>
        /// response from the checkInterfaceResponse call 
        /// </summary>
        internal bool Supported;
        /// <summary>
        /// should we have a variable here or just look it up each time?
        /// this should probably not exist.  It should be posted as part of event to listeners
        /// </summary>
        internal Hardware OurHardware = null;

        public void handleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            this.HardwareId = hardwareId;
            this.BootloaderId = bootloaderId;
            this.FirmwareId = firmwareId;
            OurHardware = Board.AllBoards[System.Text.Encoding.ASCII.GetString(hardwareId)];
        }

        public void handleConnectionLost()
        {
        }

        public void handleSoftReset()
        {
        }

        public void handleCheckInterfaceResponse(bool supported)
        {
            this.Supported = supported;
        }

        internal IDictionary<int, Tuple<int, bool>> StateSetChangeNotify = new Dictionary<int, Tuple<int, bool>>();
        public void handleSetChangeNotify(int pin, bool changeNotify)
        {
            StateSetChangeNotify.Add(pin, new Tuple<int, bool>(pin, changeNotify));
        }

        internal IDictionary<int, Tuple<int, bool>> StateReportDigitalInStatus = new Dictionary<int, Tuple<int, bool>>();
        public void handleReportDigitalInStatus(int pin, bool level)
        {
            StateReportDigitalInStatus.Add(pin, new Tuple<int, bool>(pin, level));
        }

        internal IDictionary<int, Tuple<int, int>> StatePeriodicDigitalSampling = new Dictionary<int, Tuple<int, int>>();
        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            StatePeriodicDigitalSampling.Add(pin, new Tuple<int, int>(pin, freqScale));
        }

        internal IDictionary<int, Tuple<int, bool[]>> StateReportPeriodicDigitalInStatus = new Dictionary<int, Tuple<int, bool[]>>();
        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            StateReportPeriodicDigitalInStatus.Add(frameNum, new Tuple<int, bool[]>(frameNum, values));
        }

        internal IDictionary<int, Tuple<int, bool>> StateAnalogPinStatus = new Dictionary<int, Tuple<int, bool>>();
        public void handleAnalogPinStatus(int pin, bool open)
        {
            StateAnalogPinStatus.Add(pin, new Tuple<int, bool>(pin, open));
        }

        internal IDictionary<int, Tuple<int, int>> StateReportAnalogInStatus = new Dictionary<int, Tuple<int, int>>();
        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            for (int i = 0; i < pins.Count; i++)
            {
                StateReportAnalogInStatus.Add(pins[i], new Tuple<int, int>(pins[i], values[i]));
            }
        }

        internal IDictionary<int, int> StateUartOpen = new Dictionary<int, int>();
        public void handleUartOpen(int uartNum)
        {
            StateUartOpen.Add(uartNum, uartNum);
        }

        public void handleUartClose(int uartNum)
        {
            if (StateUartOpen.ContainsKey(uartNum))
            {
                StateUartOpen.Remove(uartNum);
            }
        }

        internal IDictionary<int, Tuple<int, int, byte[]>> StateUartData = new Dictionary<int, Tuple<int, int, byte[]>>();
        public void handleUartData(int uartNum, int numBytes, byte[] data)
        {
            StateUartData.Add(uartNum, new Tuple<int, int, byte[]>(uartNum, numBytes, data));
        }

        public void handleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
        }

        internal IDictionary<int, int> StateSpiOpen = new Dictionary<int, int>();
        public void handleSpiOpen(int spiNum)
        {
            StateSpiOpen.Add(spiNum, spiNum);
        }

        public void handleSpiClose(int spiNum)
        {
            if (StateSpiOpen.ContainsKey(spiNum))
            {
                StateSpiOpen.Remove(spiNum);
            }
        }

        internal IDictionary<int, Tuple<int, int, byte[], int>> StateSpiData = new Dictionary<int, Tuple<int, int, byte[], int>>();
        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            StateSpiData.Add(spiNum, new Tuple<int, int, byte[], int>(spiNum, ssPin, data, dataBytes));
        }

        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        internal IDictionary<int, int> StateI2cOpen = new Dictionary<int, int>();
        public void handleI2cOpen(int i2cNum)
        {
            StateI2cOpen.Add(i2cNum, i2cNum);
        }

        public void handleI2cClose(int i2cNum)
        {
            if (StateI2cOpen.ContainsKey(i2cNum))
            {
                StateI2cOpen.Remove(i2cNum);
            }
        }

        internal IDictionary<int, Tuple<int, int, byte[]>> StateI2cResult = new Dictionary<int, Tuple<int, int, byte[]>>();
        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
            StateI2cResult.Add(i2cNum, new Tuple<int, int, byte[]>(i2cNum, size, data));
        }

        public void handleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
        }

        internal Boolean StateIcspOpen = false;
        public void handleIcspOpen()
        {
            StateIcspOpen = true;
        }

        public void handleIcspClose()
        {
            StateIcspOpen = false;
        }

        public void handleIcspReportRxStatus(int bytesRemaining)
        {
        }

        internal Tuple<int, byte[]> StateIcspResult = new Tuple<int, byte[]>(-1, null);
        public void handleIcspResult(int size, byte[] data)
        {
            StateIcspResult = new Tuple<int, byte[]>(size, data);
        }

        internal IDictionary<int, Tuple<int, int, byte[]>> StateIncapReport = new Dictionary<int, Tuple<int, int, byte[]>>();
        public void handleIncapReport(int incapNum, int size, byte[] data)
        {
            StateIncapReport.Add(incapNum, new Tuple<int, int, byte[]>(incapNum, size, data));
        }

        internal IDictionary<int, int> StateIncapOpen = new Dictionary<int, int>();
        public void handleIncapClose(int incapNum)
        {
            if (StateIncapOpen.ContainsKey(incapNum))
            {
                StateIncapOpen.Remove(incapNum);
            }
        }

        public void handleIncapOpen(int incapNum)
        {
            StateIncapOpen.Add(incapNum, incapNum);
        }

        internal IDictionary<int, Tuple<int, int>> StateCapSenseReport = new Dictionary<int, Tuple<int, int>>();
        public void handleCapSenseReport(int pinNum, int value)
        {
            StateCapSenseReport.Add(pinNum, new Tuple<int, int>(pinNum, value));
        }

        internal IDictionary<int, Tuple<int, bool>> StateCapSenseSampling = new Dictionary<int, Tuple<int, bool>>();
        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
            StateCapSenseSampling.Add(pinNum, new Tuple<int, bool>(pinNum, enable));
        }

        internal IDictionary<Types.SequencerEvent, Tuple<Types.SequencerEvent, int>> StateSequencerEvent = new Dictionary<Types.SequencerEvent, Tuple<Types.SequencerEvent, int>>();
        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            StateSequencerEvent.Add(seqEvent, new Tuple<SequencerEvent, int>(seqEvent, arg));
        }

        public void handleSync()
        {
        }
    }
}
