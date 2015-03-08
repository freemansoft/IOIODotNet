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
    public class IOIOHandlerDistributor : IOIOIncomingHandler
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerDistributor));

        private List<IOIOIncomingHandler> Distributees = new List<IOIOIncomingHandler>();

        public IOIOHandlerDistributor(List<IOIOIncomingHandler> distributees)
        {
            this.Distributees.AddRange(distributees);
        }
        public void handleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleEstablishConnection(hardwareId, bootloaderId, firmwareId);
            }
        }

        public void handleConnectionLost()
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleConnectionLost();
            }
        }

        public void handleSoftReset()
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleSoftReset();
            }
        }

        public void handleCheckInterfaceResponse(bool supported)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleCheckInterfaceResponse(supported);
            }
        }

        public void handleSetChangeNotify(int pin, bool changeNotify)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleSetChangeNotify(pin, changeNotify);
            }
        }

        public void handleReportDigitalInStatus(int pin, bool level)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleReportDigitalInStatus(pin, level);
            }
        }

        public void handleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleRegisterPeriodicDigitalSampling(pin, freqScale);
            }
        }

        public void handleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleReportPeriodicDigitalInStatus(frameNum, values);
            }
        }

        public void handleAnalogPinStatus(int pin, bool open)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleAnalogPinStatus(pin, open);
            }
        }

        public void handleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleReportAnalogInStatus(pins, values);
            }
        }

        public void handleUartOpen(int uartNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleUartOpen(uartNum);
            }
        }

        public void handleUartClose(int uartNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleUartClose(uartNum);
            }
        }

        public void handleUartData(int uartNum, int numBytes, byte[] data)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleUartData(uartNum, numBytes, data);
            }
        }

        public void handleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleUartReportTxStatus(uartNum, bytesRemaining);
            }
        }

        public void handleSpiOpen(int spiNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleSpiOpen(spiNum);
            }
        }

        public void handleSpiClose(int spiNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleSpiClose(spiNum);
            }
        }

        public void handleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleSpiData(spiNum, ssPin, data, dataBytes);
            }
        }

        public void handleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleSpiReportTxStatus(spiNum, bytesRemaining);
            }
        }

        public void handleI2cOpen(int i2cNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleI2cOpen(i2cNum);
            }
        }

        public void handleI2cClose(int i2cNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleI2cClose(i2cNum);
            }
        }

        public void handleI2cResult(int i2cNum, int size, byte[] data)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleI2cResult(i2cNum, size, data);
            }
        }

        public void handleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleI2cReportTxStatus(spiNum, bytesRemaining);
            }
        }

        public void handleIcspOpen()
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleIcspOpen();
            }
        }

        public void handleIcspClose()
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleIcspClose();
            }
        }

        public void handleIcspReportRxStatus(int bytesRemaining)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleIcspReportRxStatus(bytesRemaining);
            }
        }

        public void handleIcspResult(int size, byte[] data)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleIcspResult(size, data);
            }
        }

        public void handleIncapReport(int incapNum, int size, byte[] data)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleIncapReport(incapNum, size, data);
            }
        }

        public void handleIncapClose(int incapNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleIncapClose(incapNum);
            }
        }

        public void handleIncapOpen(int incapNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleIncapOpen(incapNum);
            }
        }

        public void handleCapSenseReport(int pinNum, int value)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleCapSenseReport(pinNum, value);
            }
        }

        public void handleSetCapSenseSampling(int pinNum, bool enable)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleSetCapSenseSampling(pinNum, enable);
            }
        }

        public void handleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleSequencerEvent(seqEvent, arg);
            }
        }

        public void handleSync()
        {
            foreach (IOIOIncomingHandler Distributee in Distributees)
            {
                Distributee.handleSync();
            }
        }
    }
}
