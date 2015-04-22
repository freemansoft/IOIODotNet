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
using IOIOLib.MessageFrom;
using System.Collections.Concurrent;
using IOIOLib.IOIOException;
using IOIOLib.Util;
using IOIOLib.Message;

/// <summary>
/// THE THREADING IN THIS CLASS SHOULD BE REVIEWED
/// </summary>
namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// Notify observers when messages are received.
    /// Observers subscribe for notification.
    /// Observers are notified serially in a new Task.
    /// </summary>
    public class IOIOHandlerObservableNoWait : IOIOHandleAbstract,IObservableIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerObservableNoWait));

        /// <summary>
        /// Notify the observers in a task so we can return to the read thread
        /// </summary>
        /// <param name="message"></param>
		internal override void  HandleMessage(IMessageFromIOIO message)
		{
            // messages must support notification to do notification...
            IMessageNotificationIOIO notifier = message as IMessageNotificationIOIO;
            if (notifier != null) {
                Task.Factory.StartNew(() =>
                {
                    foreach (IObserverIOIO observer in Interested_)
                    {
                        try { 
                            notifier.Notify(observer);
                        } catch (Exception e) {
                            LOG.Error("Caught exception ", e);
                        }
                    }
                }
                );
            }
        }

		internal ConcurrentBag<IObserverIOIO> Interested_ =	new ConcurrentBag<IObserverIOIO>();

		/// <summary>
		/// Does not yet return an actual unsubscriber
		/// </summary>
		/// <param name="t">the specific subtype of IMessageFromIOIO you are interested in</param>
		/// <param name="observer">object that wishes to be notified</param>
		/// <returns></returns>
		public virtual IDisposable Subscribe(IObserverIOIO observer)
		{
			Interested_.Add(observer);
			return null;
		}

	}
}
