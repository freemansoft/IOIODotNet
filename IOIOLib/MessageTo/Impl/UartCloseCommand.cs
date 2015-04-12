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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOIOLib.Device;
using IOIOLib.Component.Types;
using IOIOLib.Util;

namespace IOIOLib.MessageTo.Impl
{
    public class UartCloseCommand : IUartCloseCommand
    {
		private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(UartCloseCommand));

		public UartSpec UartDef { get; private set; }

        internal UartCloseCommand(UartSpec uart)
        {
            this.UartDef = uart;
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            // luckily? outbound ignores resource allcoation
            try
            {
				outBound.uartClose(this.UartDef.UartNumber);
			}
			catch (Exception e)
			{
				LOG.Debug("Caught exception while closing UART ", e);
			}
			if (this.UartDef.RxSpec != null)
			{
				outBound.setPinDigitalIn(this.UartDef.RxSpec.Pin, DigitalInputSpecMode.FLOATING);
			}
			if (this.UartDef.TxSpec != null)
			{
				outBound.setPinDigitalIn(this.UartDef.TxSpec.Pin, DigitalInputSpecMode.FLOATING);
			}
			return true;
		}



		/// <summary>
		/// TODO really needs to be a Free() method in this interface
		/// this is actually in the wrong order -- should be done AFTER the command
		/// </summary>
		/// <param name="rManager"></param>
		/// <returns></returns>
		public bool Alloc(IResourceManager rManager)
		{
			try
			{
				rManager.Free(new Resource(ResourceType.UART, this.UartDef.UartNumber));
			}
			catch (Exception e)
			{
				LOG.Debug("Caught exception while releasing UART ", e);
			}
			if (this.UartDef.RxSpec != null)
			{
				rManager.Free(new Resource(ResourceType.PIN, this.UartDef.RxSpec.Pin));
			}
			if (this.UartDef.TxSpec != null)
			{
				rManager.Free(new Resource(ResourceType.PIN, this.UartDef.TxSpec.Pin));
			}
			return true;
		}
	}
}
