using IOIOLib.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    class ObserverTxStatusBase
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(ObserverTxStatusBase));

        /// <summary>
        /// dictionary of buffer depths keyed by bus number/id/uart
        /// </summary>
        internal ConcurrentDictionary<int, ObserverTxStatusPoco> BufferDepth_ { get; private set; }

        public ObserverTxStatusBase()
        {
            BufferDepth_ = new ConcurrentDictionary<int, ObserverTxStatusPoco>();
        }

        /// <summary>
        /// adds the passed in bytes to the dcurrent number of bytes
        /// </summary>
        /// <param name="key">bus unit identifier, uart num, Twi bus num...</param>
        /// <returns>number of bytes left in remote buffer.  -1 if bus not initialized</returns>
        internal int GetTXBufferState(int key)
        {
            // update requires the old value
            ObserverTxStatusPoco oldRemaining;
            bool gotValue = BufferDepth_.TryGetValue(key, out oldRemaining);
            if (gotValue) {
                //LOG.Debug("GotValue " + oldRemaining);
                return oldRemaining.NumBytesRemaining;
            }
            else
            {
                //LOG.Debug("NoValue ");
                return -1;
            }
        }

        /// <summary>
        /// adds the passed in bytes to the current number of bytes
        /// </summary>
        /// <param name="key">bus unit identifier, uart num, Twi bus num...</param>
        /// <param name="numBytesChange">number of bytes to increment or decrement the buffer</param>
        /// <returns>avalable buffer space</returns>
        internal ObserverTxStatusPoco UpdateTXBufferState(int key, int numBytesChange, int numSendChange, int numReceiveChange)
        {
            // make sure we always have a key with an initial value. Ignore success/fail code
            // this used to be in a conditional block. TryAdd fails if key exists
            BufferDepth_.TryAdd(key, new ObserverTxStatusPoco());
            // update requires the old value
            ObserverTxStatusPoco oldRemaining;
            bool gotValue = BufferDepth_.TryGetValue(key, out oldRemaining);
            ObserverTxStatusPoco newRemaining = new ObserverTxStatusPoco(
                oldRemaining.NumBytesRemaining + numBytesChange,
                oldRemaining.NumSent+numSendChange, 
                oldRemaining.NumReceived+numReceiveChange);
            // this could fail if we have multiple threads manipulating the map and oldRemaining has changed
            // we should RETRY if we fail
            bool success = BufferDepth_.TryUpdate(key, newRemaining, oldRemaining);
            if (!success) { 
                LOG.Info("Failed update: "+ newRemaining+ "->" + oldRemaining);
            }
            return newRemaining;
        }

        /// <summary>
        /// sets the passed in bytes to the current number of bytes
        /// </summary>
        /// <param name="key">bus unit identifier, uart num, Twi bus num...</param>
        /// <param name="numBytesRemaining">current buffer size</param>
        /// <returns>available buffer space.  should be same as numBytesRemaining</returns>
        internal ObserverTxStatusPoco SetTXBufferState(int key, int numBytesRemaining)
        {
            // make sure we always have a key with an initial value. Ignore success/fail code
            // this used to be in a conditional block. TryAdd fails if key exists
            BufferDepth_.TryAdd(key, new ObserverTxStatusPoco());
            // update requires the old value
            ObserverTxStatusPoco oldRemaining;
            bool gotValue = BufferDepth_.TryGetValue(key, out oldRemaining);
            ObserverTxStatusPoco newRemaining = new ObserverTxStatusPoco(
                 numBytesRemaining, 
                 oldRemaining.NumSent, 
                 oldRemaining.NumReceived);
            // this could fail if we have multiple threads manipulating the map and oldRemaining has changed
            // we should RETRY if we fail
            bool success = BufferDepth_.TryUpdate(key, newRemaining, oldRemaining);
            if (!success)
            {
                LOG.Info("Failed set: " + newRemaining + "->" + oldRemaining);
            }
            return newRemaining;
        }

        internal virtual void ClearCount(int key)
        {
            ObserverTxStatusPoco removedValue;
            BufferDepth_.TryRemove(key, out removedValue);
        }

        public override string ToString()
        {
            // this should be smarter about iterating to build a buffer
            return base.ToString() + BufferDepth_;
        }
    }
}
