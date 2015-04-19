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
        internal ConcurrentDictionary<int, int> BufferDepth_ { get; private set; }

        public ObserverTxStatusBase()
        {
            BufferDepth_ = new ConcurrentDictionary<int, int>();
        }
        internal int UpdateTXBufferState(int key, int bytesRemaining)
        {
            // make sure we always have a key with an initial value. Ignore success/fail code
            // this used to be in a conditional block. TryAdd fails if key exists
            BufferDepth_.TryAdd(key, 0);

            int oldRemaining;
            bool gotValue = BufferDepth_.TryGetValue(key, out oldRemaining);
            int newRemaining = oldRemaining + bytesRemaining;
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
