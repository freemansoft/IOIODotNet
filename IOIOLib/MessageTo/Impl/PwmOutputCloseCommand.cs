﻿/*
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
    public class PwmOutputCloseCommand: IPwmOutputCloseCommand
    {

		public PwmOutputSpec PwmDef { get; private set; }


		public PwmOutputCloseCommand(PwmOutputSpec spec)
        {
            this.PwmDef = spec;
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
			outBound.setPinDigitalIn(this.PwmDef.PinSpec.Pin, DigitalInputSpecMode.FLOATING);
			outBound.setPwmPeriod(this.PwmDef.PwmNumber, 0, PwmScale.Scale1X);
            return true;
        }

		/// <summary>
		/// TODO really needs to be a Free() method in this interface
		/// this is actually in the wrong order -- should be done AFTER the command
		/// </summary>
		/// <param name="rManager"></param>
		/// <returns></returns>
		public bool Alloc(Device.IResourceManager rManager)
		{
			rManager.Free(new Resource(ResourceType.PIN, PwmDef.PinSpec.Pin));
			rManager.Free(new Resource(ResourceType.OUTCOMPARE, this.PwmDef.PwmNumber));
			return true;
		}
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
