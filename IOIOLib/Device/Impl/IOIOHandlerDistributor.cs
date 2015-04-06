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

        private List<IOIOIncomingHandler> Distributees_ = new List<IOIOIncomingHandler>();

        public IOIOHandlerDistributor(List<IOIOIncomingHandler> distributees)
        {
            this.Distributees_.AddRange(distributees);
        }
        public virtual void HandleEstablishConnection(byte[] hardwareId, byte[] bootloaderId, byte[] firmwareId)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleEstablishConnection(hardwareId, bootloaderId, firmwareId);
            }
        }

        public virtual void HandleConnectionLost()
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleConnectionLost();
            }
        }

        public virtual void HandleSoftReset()
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleSoftReset();
            }
        }

        public virtual void HandleCheckInterfaceResponse(bool supported)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleCheckInterfaceResponse(supported);
            }
        }

        public virtual void HandleSetChangeNotify(int pin, bool changeNotify)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleSetChangeNotify(pin, changeNotify);
            }
        }

        public virtual void HandleReportDigitalInStatus(int pin, bool level)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleReportDigitalInStatus(pin, level);
            }
        }

        public virtual void HandleRegisterPeriodicDigitalSampling(int pin, int freqScale)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleRegisterPeriodicDigitalSampling(pin, freqScale);
            }
        }

        public virtual void HandleReportPeriodicDigitalInStatus(int frameNum, bool[] values)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleReportPeriodicDigitalInStatus(frameNum, values);
            }
        }

        public virtual void HandleAnalogPinStatus(int pin, bool open)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleAnalogPinStatus(pin, open);
            }
        }

        public virtual void HandleReportAnalogInStatus(List<int> pins, List<int> values)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleReportAnalogInStatus(pins, values);
            }
        }

        public virtual void HandleUartOpen(int uartNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleUartOpen(uartNum);
            }
        }

        public virtual void HandleUartClose(int uartNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleUartClose(uartNum);
            }
        }

        public virtual void HandleUartData(int uartNum, int numBytes, byte[] data)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleUartData(uartNum, numBytes, data);
            }
        }

        public virtual void HandleUartReportTxStatus(int uartNum, int bytesRemaining)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleUartReportTxStatus(uartNum, bytesRemaining);
            }
        }

        public virtual void HandleSpiOpen(int spiNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleSpiOpen(spiNum);
            }
        }

        public virtual void HandleSpiClose(int spiNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleSpiClose(spiNum);
            }
        }

        public virtual void HandleSpiData(int spiNum, int ssPin, byte[] data, int dataBytes)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleSpiData(spiNum, ssPin, data, dataBytes);
            }
        }

        public virtual void HandleSpiReportTxStatus(int spiNum, int bytesRemaining)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleSpiReportTxStatus(spiNum, bytesRemaining);
            }
        }

        public virtual void HandleI2cOpen(int i2cNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleI2cOpen(i2cNum);
            }
        }

        public virtual void HandleI2cClose(int i2cNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleI2cClose(i2cNum);
            }
        }

        public virtual void HandleI2cResult(int i2cNum, int size, byte[] data)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleI2cResult(i2cNum, size, data);
            }
        }

        public virtual void HandleI2cReportTxStatus(int spiNum, int bytesRemaining)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleI2cReportTxStatus(spiNum, bytesRemaining);
            }
        }

        public virtual void HandleIcspOpen()
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleIcspOpen();
            }
        }

        public virtual void HandleIcspClose()
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleIcspClose();
            }
        }

        public virtual void HandleIcspReportRxStatus(int bytesRemaining)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleIcspReportRxStatus(bytesRemaining);
            }
        }

        public virtual void HandleIcspResult(int size, byte[] data)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleIcspResult(size, data);
            }
        }

        public virtual void HandleIncapReport(int incapNum, int size, byte[] data)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleIncapReport(incapNum, size, data);
            }
        }

        public virtual void HandleIncapClose(int incapNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleIncapClose(incapNum);
            }
        }

        public virtual void HandleIncapOpen(int incapNum)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleIncapOpen(incapNum);
            }
        }

        public virtual void HandleCapSenseReport(int pinNum, int value)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleCapSenseReport(pinNum, value);
            }
        }

        public virtual void HandleSetCapSenseSampling(int pinNum, bool enable)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleSetCapSenseSampling(pinNum, enable);
            }
        }

        public virtual void HandleSequencerEvent(Types.SequencerEvent seqEvent, int arg)
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleSequencerEvent(seqEvent, arg);
            }
        }

        public virtual void HandleSync()
        {
            foreach (IOIOIncomingHandler Distributee in Distributees_)
            {
                Distributee.HandleSync();
            }
        }
    }
}
