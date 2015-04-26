using IOIOLib.Message;
using IOIOLib.MessageFrom;
using IOIOLib.Util;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// Captures all inbound messages in a queue.  This can get large if you don't pull the messages.
    /// Maybe we should set it up so that it only holds the last N messages like the LogObserver?
    /// One difference with the log observer is that there is no destructive read operation in that observer.
    /// </summary>
    public class ObserverCaptureSingleQueue : IObserver<IMessageFromIOIO>,  IObserverIOIO, IEnumerable<IMessageFromIOIO>
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(ObserverCaptureSingleQueue));

        /// <summary>
        /// Use GetMessage to pull a message or IEnumerable to get matching messages without Dequeuing
        /// </summary>
        private ConcurrentQueue<IMessageFromIOIO> CapturedMessages_ =
            new ConcurrentQueue<IMessageFromIOIO>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxCaptureDepth">number to retain in buffer.  
        /// value less 0 means all which cna be a lot
        /// </param>
        public ObserverCaptureSingleQueue()
        {
        }

        public void OnCompleted()
        {
            // should do something
        }

        public void OnError(Exception error)
        {
            // should do something
        }

        public void OnNext(IMessageFromIOIO message)
        {
           
            LOG.Debug("Received:" + message);
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

        /// <summary>
        /// Legacy API used for testing
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<IMessageFromIOIO> GetEnumerator()
        {
            return ((IEnumerable<IMessageFromIOIO>)CapturedMessages_).GetEnumerator();
        }

        /// <summary>
        /// Legacy API used for testing
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IMessageFromIOIO>)CapturedMessages_).GetEnumerator();
        }
    }
}
