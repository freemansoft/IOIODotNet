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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOIOLib.Device;
using IOIOLib.Device.Impl;

namespace IOIOLib.MessageTo.Impl
{
    class AnalogInputConfigureCommand : IAnalogInputConfigureCommand
    {
        public  int BoundPin { get; private set; }

        public Boolean? ChangeNotify { get; private set; }


        internal AnalogInputConfigureCommand(int pin)
        {
            this.BoundPin = pin;
            this.ChangeNotify = null;
        }

        /// <summary>
        /// create an inbound analong channel that samples data
        /// </summary>
        /// <param name="Pin"></param>
        /// <param name="notifyOnChange">samples at a 1khz rate you 
        /// must enable notification changes if you want the device to send youvalues</param>
        public AnalogInputConfigureCommand(int pin, bool notifyOnChange)
        {
            this.BoundPin = pin;
            ChangeNotify = notifyOnChange;
        }


        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            outBound.setPinAnalogIn(this.BoundPin);
            if (ChangeNotify.HasValue && ChangeNotify.Value)
            {
                outBound.setAnalogInSampling(this.BoundPin, ChangeNotify.Value);
            }
            return true;
        }

		public bool Alloc(IResourceManager rManager)
		{
			return true;
		}

        public override string ToString()
        {
            return this.GetType().Name;
        }

    }
}
