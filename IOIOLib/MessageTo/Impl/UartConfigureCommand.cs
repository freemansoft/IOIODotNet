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

using IOIOLib.Component.Types;
using IOIOLib.Device.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class UartConfigureCommand : IUartConfigureCommand
    {
        public UartParity Parity { get; private set; }
        public int UartNum { get; private set; }
        public UartStopBits StopBits { get; private set; }
        public int Rate { get; private set; }
        public bool Speed4x { get; private set; }
        /// <summary>
        /// Retained for debugging
        /// </summary>
        public DigitalInputSpec RXSpec { get; private set; }
        /// <summary>
        /// Retained for debugging
        /// </summary>
        public DigitalOutputSpec TXSpec { get; private set; }



        internal UartConfigureCommand(Component.Types.DigitalInputSpec digitalInputSpec, Component.Types.DigitalOutputSpec digitalOutputSpec, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
        {
            // TODO: Complete resource allocation
            this.RXSpec = digitalInputSpec;
            this.TXSpec = digitalOutputSpec;
            this.CalculateRateAndSpeed4X(baud);
            this.Parity = parity;
            this.StopBits = stopbits;
            this.UartNum = 0;
            /*
                checkState();
                if (rx != null) {
                    hardware_.CheckSupportsPeripheralInput(rx.Pin);
                }
                if (isTx != null) {
                    hardware_.CheckSupportsPeripheralOutput(isTx.Pin);
                }
                Resource rxPin = rx != null ? new Resource(ResourceType.PIN, rx.Pin)
                        : null;
                Resource txPin = isTx != null ? new Resource(ResourceType.PIN, isTx.Pin)
                        : null;
                Resource uart = new Resource(ResourceType.UART);
                resourceManager_.Alloc(rxPin, txPin, uart);

                UartImpl result = new UartImpl(this, txPin, rxPin, uart);
                addDisconnectListener(result);
                incomingState_.addUartListener(uart.id, result);
             * ***************************
        */
        }

        private void CalculateRateAndSpeed4X(int baud)
        {
            Speed4x = true;
            Rate = (int)(Math.Round(4000000.0f / baud) - 1);
            if (Rate > 65535)
            {
                Speed4x = false;
                Rate = (int)(Math.Round(1000000.0f / baud) - 1);
            }
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
