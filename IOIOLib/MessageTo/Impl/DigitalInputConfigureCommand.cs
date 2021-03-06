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
using IOIOLib.Device;

namespace IOIOLib.MessageTo.Impl
{
    public class DigitalInputConfigureCommand : IDigitalInputConfigureCommand
    {
        public DigitalInputSpec PinDef { get; private set; }

        /// <summary>
        /// Should the IOIO notify on state changes
        /// </summary>
        public Boolean? ChangeNotify { get; private set; }


        internal DigitalInputConfigureCommand(DigitalInputSpec digitalInputSpec)
        {
            this.PinDef = digitalInputSpec;
            ChangeNotify = null;
        }

        /// <summary>
        /// Create an inbound digital Port_ that notifies on state change
        /// </summary>
        /// <param name="digitalInputSpec"></param>
        /// <param name="notifyOnChange">You must notify on state change if you ever want to know the state of a pin</param>
        public DigitalInputConfigureCommand(DigitalInputSpec digitalInputSpec, bool notifyOnChange)
        {
            this.PinDef = digitalInputSpec;
            ChangeNotify = notifyOnChange;
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
			outBound.setPinDigitalIn(this.PinDef.Pin, this.PinDef.Mode);
            if (ChangeNotify.HasValue && ChangeNotify.Value)
            {
                outBound.setChangeNotify(this.PinDef.Pin, ChangeNotify.Value);
            }
            return true;

        }

		public bool Alloc(IResourceManager rManager)
		{
			rManager.Alloc(new Resource(ResourceType.PIN, PinDef.Pin));
			return true;
		}
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
