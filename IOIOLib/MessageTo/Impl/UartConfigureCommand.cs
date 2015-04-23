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
using IOIOLib.Device;
using IOIOLib.Device.Impl;

namespace IOIOLib.MessageTo.Impl
{
	public class UartConfigureCommand : IUartConfigureCommand
	{
		public UartParity Parity { get; private set; }
		public UartSpec UartDef { get; private set; }
		public UartStopBits StopBits { get; private set; }
		public int Baud { get; private set; }

		public int RateCalculated { get; private set; }
		public bool Speed4xCalculated { get; private set; }



		/// <summary>
		/// Pins 3-7, 10-14, 27-32, 34-40 and 45-48 can be used with UART
		/// https://github.com/ytai/ioio/wiki/UART 
		/// </summary>
		/// <param name="digitalInputSpec"></param>
		/// <param name="digitalOutputSpec"></param>
		/// <param name="baud"></param>
		/// <param name="parity"></param>
		/// <param name="stopbits"></param>
		internal UartConfigureCommand(Component.Types.DigitalInputSpec digitalInputSpec, Component.Types.DigitalOutputSpec digitalOutputSpec, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
		{
			this.Baud = baud;
			this.Parity = parity;
			this.StopBits = stopbits;
			this.UartDef = new UartSpec(digitalInputSpec, digitalOutputSpec);
		}

		private void CalculateRateAndSpeed4X(int baud)
		{
			Speed4xCalculated = true;
			RateCalculated = (int)(Math.Round(4000000.0f / baud) - 1);
			if (RateCalculated > 65535)
			{
				Speed4xCalculated = false;
				RateCalculated = (int)(Math.Round(1000000.0f / baud) - 1);
			}
		}

		public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
		{
			if (UartDef.RxSpec != null)
			{
				outBound.setPinDigitalIn(UartDef.RxSpec.Pin, UartDef.RxSpec.Mode);
				outBound.setPinUart(UartDef.RxSpec.Pin, UartDef.UartNumber, false, true);
			}
			if (UartDef.TxSpec != null)
			{
				outBound.setPinDigitalOut(UartDef.TxSpec.Pin, true, UartDef.TxSpec.Mode);
				outBound.setPinUart(UartDef.TxSpec.Pin, UartDef.UartNumber, true, true);
			}
			CalculateRateAndSpeed4X(this.Baud);
			outBound.uartConfigure(UartDef.UartNumber, RateCalculated, Speed4xCalculated, StopBits, Parity);
			return true;
		}

		public bool Alloc(IResourceManager rManager)
		{
			if (UartDef.TxSpec != null)
			{
				rManager.BoundHardware.CheckSupportsPeripheralInput(UartDef.RxSpec.Pin);
			}
			if (UartDef.TxSpec != null)
			{
				rManager.BoundHardware.CheckSupportsPeripheralOutput(UartDef.TxSpec.Pin);
			}
			Resource rxPin = null;
			if (UartDef.TxSpec != null)
			{
				rxPin = new Resource(ResourceType.PIN, UartDef.RxSpec.Pin);
			}
			Resource txPin = null;
			if (UartDef.TxSpec != null)
			{
				txPin = new Resource(ResourceType.PIN, UartDef.TxSpec.Pin);
			}
			Resource uart = new Resource(ResourceType.UART);
			rManager.Alloc(rxPin, txPin, uart);
			// update the spec with the allocated id
			this.UartDef = new UartSpec(this.UartDef.RxSpec, this.UartDef.TxSpec, uart.Id_);
			return true;
		}

        public override string ToString()
        {
            return base.ToString() + "Uart:" + this.UartDef.UartNumber
                + " TX:"+UartDef.TxSpec.Pin+ " RX:"+UartDef.RxSpec.Pin;
        }
    }
}
