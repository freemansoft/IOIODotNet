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
using IOIOLib.Util;
using IOIOLib.Message;
using IOIOLib.Message.Impl;

namespace IOIOLib.MessageTo.Impl
{
    public class TwiMasterSendDataCommand : IOIOMessageNotification<ITwiMasterSendDataCommand>, ITwiMasterSendDataCommand
    {
        private int Address;
        private bool IsTenBitAddress;
        public byte[] Data { get; private set; }
        private int NumBytesRead;

        /// <summary>
        /// populated by constructor.  used by other calls
        /// </summary>
        public TwiSpec TwiDef { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twiDef"></param>
        /// <param name="address">Device address</param>
        /// <param name="isTenBitAddress">i2C addressing</param>
        /// <param name="writeData">a byte array to be written to IOIO I2C device.  Array length must be correct</param>
        /// <param name="numBytesRead">passed to the IOIO to tell it how many response bytes to expect</param>
        internal TwiMasterSendDataCommand(TwiSpec twiDef,
            int address, bool isTenBitAddress, 
            byte[] writeData, int numBytesRead)
        {
            // TODO: Complete member initialization
            this.TwiDef = twiDef;
            this.Address = address;
            this.IsTenBitAddress = isTenBitAddress;
            this.Data = writeData;
            this.NumBytesRead = numBytesRead;
        }



        /// <summary>
        /// no-op for this command
        /// </summary>
        /// <param name="rManager">the holder of resources for this board</param>
        /// <returns>true if succeeds. always true since this command doesn't allocate resources</returns>
        public bool Alloc(IResourceManager rManager)
		{
            return true;
		}

        /// <summary>
        /// This will pretty much always generate a I2cResultFrom response (ack)
        /// even if you set the expected response length to 0
        /// </summary>
        /// <param name="outBound"></param>
        /// <returns>true if executes</returns>
		public bool ExecuteMessage(IOIOProtocolOutgoing outBound)
		{
            outBound.i2cWriteRead(TwiDef.TwiNum, this.IsTenBitAddress, this.Address,
                this.Data.Length, this.NumBytesRead, this.Data);
            return true;
		}

        public override string ToString()
        {
            return this.GetType().Name +" Twi:" + this.TwiDef.TwiNum 
                + " ExpectBack:"+this.NumBytesRead
                + " SendingBytes:" + LoggingUtilities.ByteArrayToString(this.Data, this.Data.Length);
        }

        /// <summary>
        /// used in buffer calculations
        /// </summary>
        /// <returns></returns>
        public int PayloadSize()
        {
            // does not include the uart number which is used to determine which buffer payload goes in
            // address, is10BitAddress , return length, send length, 
            return 4 + this.Data.Length;
        }
    }
}
