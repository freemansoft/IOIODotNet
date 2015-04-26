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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOIOLib.Device;
using IOIOLib.Device.Impl;
using IOIOLib.Device.Types;

namespace IOIOLib.MessageTo.Impl
{
    public class TwiMasterConfigureCommand : ITwiMasterConfigureCommand
    {
        /// <summary>
        /// populated after alloc.  used by other calls
        /// </summary>
        public TwiSpec TwiDef { get; private set; }

        internal TwiMasterConfigureCommand(int twiNum, TwiMasterRate rate, bool smbus)
        {
            // TODO: Complete member initialization
            this.TwiDef = new TwiSpec(twiNum, rate, smbus);
        }



        public bool Alloc(IResourceManager rManager)
		{
            Resource twi = new Resource(ResourceType.TWI, this.TwiDef.TwiNum);
            Resource pin0 = new Resource(ResourceType.PIN, rManager.BoundHardware.TwiPins[this.TwiDef.TwiNum, 0]);
            Resource pin1 = new Resource(ResourceType.PIN, rManager.BoundHardware.TwiPins[this.TwiDef.TwiNum, 1]);
            List<Resource> resources = new List<Resource>() { twi, pin0, pin1 };
            rManager.Alloc(resources);
            return true;
		}

		public bool ExecuteMessage(IOIOProtocolOutgoing outBound)
		{
            outBound.i2cConfigureMaster(this.TwiDef.TwiNum, this.TwiDef.Rate, this.TwiDef.SmBus);
            return true;
		}
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
