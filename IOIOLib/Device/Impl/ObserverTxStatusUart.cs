using IOIOLib.Message;
using IOIOLib.MessageFrom;
using IOIOLib.MessageTo;
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
    class ObserverTxStatusUart : ObserverTxStatusBase, 
        IObserver<IUartSendDataCommand>,
        IObserver<IUartReportTxStatusFrom>, 
        IObserver<IUartOpenFrom>, 
        IObserver<IUartCloseFrom>, IObserverIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(ObserverTxStatusUart));

        /// <summary>
        /// buffer space remainin
        /// </summary>
        private int Remaining_ = 0;

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

        public void OnNext(IUartSendDataCommand value)
        {
            int key = value.UartDef.UartNumber;
            this.Remaining_ = UpdateTXBufferState(key, -value.PayloadSize());
            LOG.Debug("Device:" + key + " BufferDepth:" + Remaining_);
        }

        public void OnNext(IUartOpenFrom value)
        {
            ClearCount(value.UartNum);
        }

        public void OnNext(IUartReportTxStatusFrom value)
        {
            int key = value.UartNum;
            this.Remaining_ = SetTXBufferState(key, value.BytesRemaining);
            LOG.Debug("Device:" + key + " BufferDepth:" + Remaining_);
        }

    }
}
