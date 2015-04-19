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
    /// <summary>
    /// holds the current buffer status for all uarts that have been configured / seen traffic
    /// </summary>
    class ObserverTxStatusUart : ObserverTxStatusBase, IObserver<IUartReportTxStatusFrom>, IObserver<IUartOpenFrom>, IObserver<IUartCloseFrom>, IObserverIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(ObserverTxStatusUart));


        public void OnCompleted()
        {
            // do nothing
        }

        public void OnError(Exception error)
        {
            // do nothing
        }

        public void OnNext(IUartCloseFrom value)
        {
            ClearCount(value.UartNum);
        }

        public void OnNext(IUartOpenFrom value)
        {
            ClearCount(value.UartNum);
        }

        public void OnNext(IUartReportTxStatusFrom value)
        {
            int key = value.UartNum;
            int newRemaining = UpdateTXBufferState(key, value.BytesRemaining);
            LOG.Debug("Device:" + key + " BufferDepth:" + newRemaining);
        }
    }
}
