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

        /// <summary>
        /// Sent by IOIO. Tells us how much buffer space is left.
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(IUartReportTxStatusFrom value)
        {
            int key = value.UartNum;
            int newRemaining = SetTXBufferState(key, value.BytesRemaining);
            LOG.Debug("Device:" + key + " BufferDepth:" + newRemaining);
        }

        /// <summary>
        /// received when we are about to send data
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(IUartSendDataCommand value)
        {
            int key = value.UartDef.UartNumber;
            // wait until we know there is room on the remote side
            int bytesBeforeAction = GetTXBufferState(key);
            while (bytesBeforeAction < value.PayloadSize())
            {
                System.Threading.Thread.Sleep(5);
                bytesBeforeAction = GetTXBufferState(key);
            }
            int newRemaining = UpdateTXBufferState(key, -value.PayloadSize());
            LOG.Debug("Device:" + key + " BufferRemaining:" + newRemaining);
        }

    }
}
