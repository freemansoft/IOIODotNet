using IOIOLib.Message;
using IOIOLib.MessageFrom;
using IOIOLib.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    public class ObserverLog : IObserver<IMessageFromIOIO>,  IObserverIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(ObserverLog));

        /// <summary>
        /// TODO:  Use more efficient queue that retains last N
        /// </summary>
        internal ConcurrentQueue<string> CapturedLogs_ = new ConcurrentQueue<string>();

        internal int MaxCount_ = 5;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxCaptureDepth">number to retain in buffer.  
        /// value less 0 means all which cna be a lot
        /// </param>
        public ObserverLog(int maxCaptureDepth)
        {
            MaxCount_ = maxCaptureDepth;
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
            string logString = message.ToString();
            LOG.Debug(logString);
            CapturedLogs_.Enqueue(logString);
            while (MaxCount_ > 0 && CapturedLogs_.Count > MaxCount_)
            {
                // we dont' care what the results are
                string foo;
                CapturedLogs_.TryDequeue(out foo);
            }
        }
    }
}
