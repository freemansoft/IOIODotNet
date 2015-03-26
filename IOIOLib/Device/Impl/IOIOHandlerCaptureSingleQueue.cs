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
using IOIOLib.MessageFrom;
using IOIOLib.MessageFrom.Impl;
using IOIOLib.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// This class leaks if you don't read the messages it captures
    /// </summary>
    public class IOIOHandlerCaptureSingleQueue : IOIOHandleAbstract, IOIOIncomingHandler, IEnumerable<IMessageFromIOIO>
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerCaptureSingleQueue));

        /// <summary>
        /// Use GetMessage to pull a message or IEnumerable to get matching messages without Dequeuing
        /// </summary>
        private ConcurrentQueue<IMessageFromIOIO> CapturedMessages_ =
            new ConcurrentQueue<IMessageFromIOIO>();

        internal override void HandleMessage(IMessageFromIOIO message)
        {
            CapturedMessages_.Enqueue(message);
        }

		/// <summary>
		/// Tries to retrieve / dequeue the next message
		/// </summary>
		/// <returns>null if no message dequeued</returns>
		public virtual IMessageFromIOIO GetMessage()
		{
			IMessageFromIOIO buffer = null;
            bool didDeQueue = CapturedMessages_.TryDequeue(out buffer);
			return buffer;
		}



		public virtual IEnumerator<IMessageFromIOIO> GetEnumerator()
		{
			return ((IEnumerable<IMessageFromIOIO>)CapturedMessages_).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IMessageFromIOIO>)CapturedMessages_).GetEnumerator();
		}
	}
}
