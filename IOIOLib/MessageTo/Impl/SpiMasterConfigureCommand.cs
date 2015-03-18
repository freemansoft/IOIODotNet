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

namespace IOIOLib.MessageTo.Impl
{
    public class SpiMasterConfigureCommand : ISpiMasterConfigureCommand
    {
        public DigitalInputSpec Miso { get; private set; }
        public DigitalOutputSpec Mosi { get; private set; }

        public DigitalOutputSpec Clock { get; private set; }

        public DigitalOutputSpec[] SlaveSelect { get; private set; }

        public SpiMasterConfig Rate { get; private set; }



        /// <summary>
        /// Do we even need all these parameters?  Isn't there only one SpiMaster on the board
        /// </summary>
        /// <param name="miso"></param>
        /// <param name="mosi"></param>
        /// <param name="clock"></param>
        /// <param name="slaveSelect"></param>
        /// <param name="rate"></param>
        internal SpiMasterConfigureCommand(DigitalInputSpec miso, DigitalOutputSpec mosi, DigitalOutputSpec clock, DigitalOutputSpec[] slaveSelect, SpiMasterConfig rate)
        {
            this.Miso = miso;
            this.Mosi = mosi;
            this.Clock = Clock;
            this.SlaveSelect = slaveSelect;
            this.Rate = rate;
            throw new NotImplementedException("Post(IOpenSpiMasterTo) not tied together in outgoing protocol");
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound, Device.Impl.ResourceManager rManager)
        {
            throw new NotImplementedException();
        }
    }
}
