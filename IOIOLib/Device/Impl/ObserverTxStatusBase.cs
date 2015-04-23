﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    class ObserverTxStatusBase
    {
        /// <summary>
        /// dictionary of buffer depths keyed by bus number/id/uart
        /// </summary>
        internal ConcurrentDictionary<int, int> BufferDepth_ { get; private set; }

        public ObserverTxStatusBase()
        {
            BufferDepth_ = new ConcurrentDictionary<int, int>();
        }

        /// <summary>
        /// adds the passed in bytes to the dcurrent number of bytes
        /// </summary>
        /// <param name="key">bus unit identifier, uart num, Twi bus num...</param>
        /// <returns>number of bytes left in remote buffer.  -1 if bus not initialized</returns>
        internal int GetTXBufferState(int key)
        {
            // update requires the old value
            int oldRemaining;
            bool gotValue = BufferDepth_.TryGetValue(key, out oldRemaining);
            if (gotValue) { 
                return oldRemaining;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// adds the passed in bytes to the dcurrent number of bytes
        /// </summary>
        /// <param name="key">bus unit identifier, uart num, Twi bus num...</param>
        /// <param name="numBytesChange">number of bytes to increment or decrement the buffer</param>
        /// <returns>avalable buffer space</returns>
        internal int UpdateTXBufferState(int key, int numBytesChange)
        {
            // make sure we always have a key with an initial value. Ignore success/fail code
            // this used to be in a conditional block. TryAdd fails if key exists
            BufferDepth_.TryAdd(key, 0);
            // update requires the old value
            int oldRemaining;
            bool gotValue = BufferDepth_.TryGetValue(key, out oldRemaining);
            int newRemaining = oldRemaining + numBytesChange;
            // this could fail if we have multiple threads manipulating the map and oldRemaining has changed
            // we should RETRY if we fail
            bool success = BufferDepth_.TryUpdate(key, newRemaining, oldRemaining);
            return newRemaining;
        }

        /// <summary>
        /// adds the passed in bytes to the dcurrent number of bytes
        /// </summary>
        /// <param name="key">bus unit identifier, uart num, Twi bus num...</param>
        /// <param name="numBytesRemaining">current buffer size</param>
        /// <returns>available buffer space.  should be same as numBytesRemaining</returns>
        internal int SetTXBufferState(int key, int numBytesRemaining)
        {
            // make sure we always have a key with an initial value. Ignore success/fail code
            // this used to be in a conditional block. TryAdd fails if key exists
            BufferDepth_.TryAdd(key, 0);
            // update requires the old value
            int oldRemaining;
            bool gotValue = BufferDepth_.TryGetValue(key, out oldRemaining);
            int newRemaining = numBytesRemaining;
            // this could fail if we have multiple threads manipulating the map and oldRemaining has changed
            // we should RETRY if we fail
            bool success = BufferDepth_.TryUpdate(key, newRemaining, oldRemaining);
            return newRemaining;
        }

        internal virtual void ClearCount(int key)
        {
            int removedValue;
            BufferDepth_.TryRemove(key, out removedValue);
        }

        public override string ToString()
        {
            // this should be smarter about iterating to build a buffer
            return base.ToString() + BufferDepth_;
        }
    }
}
