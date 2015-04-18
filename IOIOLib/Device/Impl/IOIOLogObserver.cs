using IOIOLib.MessageFrom;
using IOIOLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    public class IOIOLogObserver : IObserver<IMessageFromIOIO>,  IObserverIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOLogObserver));

        /// <summary>
        /// TODO:  Use more efficient queue that retains last N
        /// </summary>
        internal List<string> CapturedLogs_ = new List<string>();

        internal int MaxCount_ = 5;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxCaptureDepth">number to retain in buffer.  
        /// value less 0 means all which cna be a lot
        /// </param>
        public IOIOLogObserver(int maxCaptureDepth)
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
            CapturedLogs_.Add(logString);
            if (MaxCount_ > 0 && CapturedLogs_.Count > MaxCount_)
            {
                CapturedLogs_.RemoveAt(0);
            }
        }
    }
}
