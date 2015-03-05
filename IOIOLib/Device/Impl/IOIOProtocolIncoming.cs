using IOIOLib.Device.Types;
using IOIOLib.IOIOException;
using IOIOLib.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// Handles all incoming messages for a given single device (connection) in its own thread.
    /// This lets us capture incoming data without polling or blocking the main thread
    /// </summary>
    public class IOIOProtocolIncoming
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOProtocolIncoming));

        private List<int> analogPinValues_ = new List<int>();
        private List<int> analogFramePins_ = new List<int>();
        private List<int> newFramePins_ = new List<int>();
        // use type HashSet because it implements RemoveWhere
        private HashSet<int> removedPins_ = new HashSet<int>();
        private HashSet<int> addedPins_ = new HashSet<int>();
        private Stream stream_;
        private IOIOIncomingHandler handler_;
        /// <summary>
        /// this should go somewhere else
        /// </summary
        internal CancellationTokenSource cancelTokenSource_;
        /// <summary>
        /// Why do we retain reference to this when we have cancel token access?
        /// </summary>
        private Task incomingThread_;

        public IOIOProtocolIncoming(Stream stream, IOIOIncomingHandler handler)
        {
            this.stream_ = stream;
            this.handler_ = handler;
            cancelTokenSource_ = new CancellationTokenSource();
            incomingThread_ = new Task(run, cancelTokenSource_.Token, TaskCreationOptions.LongRunning);
            incomingThread_.Start();
        }

        private void calculateAnalogFrameDelta()
        {
            // would have tried .Except() here but that returns IEnumerable instead of a list or set
            removedPins_.Clear();
            removedPins_.Union(analogFramePins_);
            addedPins_.Clear();
            addedPins_.Union(newFramePins_);
            // Remove the intersection from both.
            List<int> toRemove = new List<int>();
            foreach (int onePin in removedPins_)
            {
                if (addedPins_.Contains(onePin))
                {
                    toRemove.Add(onePin);
                }
            }
            removedPins_.RemoveWhere(x => toRemove.Contains(x));
            addedPins_.RemoveWhere(x => toRemove.Contains(x));
            // swap
            List<int> temp = analogFramePins_;
            analogFramePins_ = newFramePins_;
            newFramePins_ = temp;
        }

        private int readByte()
        {
            try
            {
                while (true)
                {
                    cancelTokenSource_.Token.ThrowIfCancellationRequested();
                    try
                    {
                        int b = stream_.ReadByte();
                        if (b < 0)
                        {
                            throw new IOException("Unexpected stream_ closure");
                        }

                        LOG.Debug("received 0x" + b.ToString("X"));
                        return b;
                    }
                    catch (TimeoutException e)
                    {
                        LOG.Debug("readByte " + e.Message + " retrying");
                    }
                }
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LOG.Warn("Thread aborted while in read ", e);
                throw e;
            }
            catch (IOException e)
            {
                LOG.Warn("IOIO disconnected while in read");
                throw e;
            }
        }

        private void readBytes(int size, byte[] buffer)
        {
            for (int i = 0; i < size; ++i)
            {
                buffer[i] = (byte)readByte();
            }
        }

        public void run()
        {
            // should set to highest priority
            int arg1;
            int arg2;
            int numPins;
            int size;
            byte[] data = new byte[256];
            try
            {
                // this was while(true) in the java code
                while (true)
                {
                    cancelTokenSource_.Token.ThrowIfCancellationRequested();
                    arg1 = readByte();
                    LOG.Debug("Processing reply type " + arg1.ToString("X"));
                    switch (arg1)
                    {
                        case (int)IOIOProtocolCommands.ESTABLISH_CONNECTION:
                            if (readByte() != 'I' || readByte() != 'O' || readByte() != 'I'
                                    || readByte() != 'O')
                            {
                                throw new IOException("Bad establish connection magic");
                            }
                            byte[] hardwareId = new byte[8];
                            byte[] bootloaderId = new byte[8];
                            byte[] firmwareId = new byte[8];
                            readBytes(8, hardwareId);
                            readBytes(8, bootloaderId);
                            readBytes(8, firmwareId);

                            handler_.handleEstablishConnection(hardwareId, bootloaderId, firmwareId);
                            break;

                        case (int)IOIOProtocolCommands.SOFT_RESET:
                            analogFramePins_.Clear();
                            handler_.handleSoftReset();
                            break;

                        case (int)IOIOProtocolCommands.REPORT_DIGITAL_IN_STATUS:
                            // pin number and state are in same byte
                            arg1 = readByte();
                            handler_.handleReportDigitalInStatus(arg1 >> 2, (arg1 & 0x01) == 1);
                            break;

                        case (int)IOIOProtocolCommands.SET_CHANGE_NOTIFY:
                            // pin number and state are in same byte
                            arg1 = readByte();
                            handler_.handleSetChangeNotify(arg1 >> 2, (arg1 & 0x01) == 1);
                            break;

                        case (int)IOIOProtocolCommands.REGISTER_PERIODIC_DIGITAL_SAMPLING:
                            // TODO: implement
                            break;

                        case (int)IOIOProtocolCommands.REPORT_PERIODIC_DIGITAL_IN_STATUS:
                            // TODO: implement
                            break;

                        case (int)IOIOProtocolCommands.REPORT_ANALOG_IN_FORMAT:
                            numPins = readByte();
                            newFramePins_.Clear();
                            for (int i = 0; i < numPins; ++i)
                            {
                                newFramePins_.Add(readByte());
                            }
                            calculateAnalogFrameDelta();
                            foreach (int i in removedPins_)
                            {
                                handler_.handleAnalogPinStatus(i, false);
                            }
                            foreach (int i in addedPins_)
                            {
                                handler_.handleAnalogPinStatus(i, true);
                            }
                            break;

                        case (int)IOIOProtocolCommands.REPORT_ANALOG_IN_STATUS:
                            numPins = analogFramePins_.Count();
                            int header = 0;
                            analogPinValues_.Count();
                            for (int i = 0; i < numPins; ++i)
                            {
                                if (i % 4 == 0)
                                {
                                    header = readByte();
                                }
                                analogPinValues_.Add((readByte() << 2) | (header & 0x03));
                                header >>= 2;
                            }
                            handler_.handleReportAnalogInStatus(analogFramePins_, analogPinValues_);
                            break;

                        case (int)IOIOProtocolCommands.UART_REPORT_TX_STATUS:
                            arg1 = readByte();
                            arg2 = readByte();
                            handler_.handleUartReportTxStatus(arg1 & 0x03, (arg1 >> 2) | (arg2 << 6));
                            break;

                        case (int)IOIOProtocolCommands.UART_DATA:
                            arg1 = readByte();
                            size = (arg1 & 0x3F) + 1;
                            readBytes(size, data);
                            handler_.handleUartData(arg1 >> 6, size, data);
                            break;

                        case (int)IOIOProtocolCommands.UART_STATUS:
                            arg1 = readByte();
                            if ((arg1 & 0x80) != 0)
                            {
                                handler_.handleUartOpen(arg1 & 0x03);
                            }
                            else
                            {
                                handler_.handleUartClose(arg1 & 0x03);
                            }
                            break;

                        case (int)IOIOProtocolCommands.SPI_DATA:
                            arg1 = readByte();
                            arg2 = readByte();
                            size = (arg1 & 0x3F) + 1;
                            readBytes(size, data);
                            handler_.handleSpiData(arg1 >> 6, arg2 & 0x3F, data, size);
                            break;

                        case (int)IOIOProtocolCommands.SPI_REPORT_TX_STATUS:
                            arg1 = readByte();
                            arg2 = readByte();
                            handler_.handleSpiReportTxStatus(arg1 & 0x03, (arg1 >> 2) | (arg2 << 6));
                            break;

                        case (int)IOIOProtocolCommands.SPI_STATUS:
                            arg1 = readByte();
                            if ((arg1 & 0x80) != 0)
                            {
                                handler_.handleSpiOpen(arg1 & 0x03);
                            }
                            else
                            {
                                handler_.handleSpiClose(arg1 & 0x03);
                            }
                            break;

                        case (int)IOIOProtocolCommands.I2C_STATUS:
                            arg1 = readByte();
                            if ((arg1 & 0x80) != 0)
                            {
                                handler_.handleI2cOpen(arg1 & 0x03);
                            }
                            else
                            {
                                handler_.handleI2cClose(arg1 & 0x03);
                            }
                            break;

                        case (int)IOIOProtocolCommands.I2C_RESULT:
                            arg1 = readByte();
                            arg2 = readByte();
                            if (arg2 != 0xFF)
                            {
                                readBytes(arg2, data);
                            }
                            handler_.handleI2cResult(arg1 & 0x03, arg2, data);
                            break;

                        case (int)IOIOProtocolCommands.I2C_REPORT_TX_STATUS:
                            arg1 = readByte();
                            arg2 = readByte();
                            handler_.handleI2cReportTxStatus(arg1 & 0x03, (arg1 >> 2) | (arg2 << 6));
                            break;

                        case (int)IOIOProtocolCommands.CHECK_INTERFACE_RESPONSE:
                            // this is 0x63 on my sparkfun 016 running 503 sw
                            arg1 = readByte();
                            handler_.handleCheckInterfaceResponse((arg1 & 0x01) == 1);
                            break;

                        case (int)IOIOProtocolCommands.ICSP_REPORT_RX_STATUS:
                            arg1 = readByte();
                            arg2 = readByte();
                            handler_.handleIcspReportRxStatus(arg1 | (arg2 << 8));
                            break;

                        case (int)IOIOProtocolCommands.ICSP_RESULT:
                            readBytes(2, data);
                            handler_.handleIcspResult(2, data);
                            break;

                        case (int)IOIOProtocolCommands.ICSP_CONFIG:
                            arg1 = readByte();
                            if ((arg1 & 0x01) == 1)
                            {
                                handler_.handleIcspOpen();
                            }
                            else
                            {
                                handler_.handleIcspClose();
                            }
                            break;

                        case (int)IOIOProtocolCommands.INCAP_STATUS:
                            arg1 = readByte();
                            if ((arg1 & 0x80) != 0)
                            {
                                handler_.handleIncapOpen(arg1 & 0x0F);
                            }
                            else
                            {
                                handler_.handleIncapClose(arg1 & 0x0F);
                            }
                            break;

                        case (int)IOIOProtocolCommands.INCAP_REPORT:
                            arg1 = readByte();
                            size = arg1 >> 6;
                            if (size == 0)
                            {
                                size = 4;
                            }
                            readBytes(size, data);
                            handler_.handleIncapReport(arg1 & 0x0F, size, data);
                            break;

                        case (int)IOIOProtocolCommands.SOFT_CLOSE:
                            LOG.Debug("Received soft close.");
                            throw new IOException("Soft close");

                        case (int)IOIOProtocolCommands.CAPSENSE_REPORT:
                            arg1 = readByte();
                            arg2 = readByte();
                            handler_.handleCapSenseReport(arg1 & 0x3F, (arg1 >> 6) | (arg2 << 2));
                            break;

                        case (int)IOIOProtocolCommands.SET_CAPSENSE_SAMPLING:
                            arg1 = readByte();
                            handler_.handleSetCapSenseSampling(arg1 & 0x3F, (arg1 & 0x80) != 0);
                            break;

                        case (int)IOIOProtocolCommands.SEQUENCER_EVENT:
                            arg1 = readByte();
                            // OPEN and STOPPED events has an additional argument.
                            if (arg1 == 2 || arg1 == 4)
                            {
                                arg2 = readByte();
                            }
                            else
                            {
                                arg2 = 0;
                            }
                            try
                            {
                                // should be able to cast since enums are really int (gag)
                                handler_.handleSequencerEvent((SequencerEvent)arg1, arg2);
                            }
                            catch (Exception e)
                            {
                                throw new IOException("Unexpected eveent: " + arg1, e);
                            }
                            break;

                        case (int)IOIOProtocolCommands.SYNC:
                            handler_.handleSync();
                            break;

                        default:
                            throw new ProtocolError("Received unexpected command: 0x"
                                    + arg1.ToString("X"));
                    }

                }
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LOG.Error("Probably aborted thread (TAE): ", e);
            }
            catch (ObjectDisposedException e)
            {
                //// see this when steram is closed
                LOG.Error("Probably closed incoming stream: (ODE)", e);
            }
            catch (Exception e)
            {
                LOG.Error("Probably stopping incoming: (E)", e);
            }
            finally
            {
                // we don't play swith stream since we didn't create it
                handler_.handleConnectionLost();
                LOG.Info("Throwing thread cancel to stop incoming thread");
                cancelTokenSource_.Cancel();
                // debugger will always stop here in unit tests if test dynamically determines what port ot use
                // just hit continue in the debugger
                cancelTokenSource_.Token.ThrowIfCancellationRequested();
                stream_ = null;
                this.incomingThread_ = null;
            }
        }
    }
}
