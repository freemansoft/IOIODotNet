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
    /// holds the I2c TX buffer status for all busses that have been configured / seen traffic
    /// </summary>
    class ObserverTxStatusI2c : ObserverTxStatusBase, 
        IObserver<ITwiMasterSendDataCommand>,
        IObserver<II2cReportTxStatusFrom>, 
        IObserver<II2cOpenFrom>, 
        IObserver<II2cCloseFrom>, IObserverIOIO
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

        public void OnNext(II2cOpenFrom value)
        {
            SetTXBufferState(value.I2cNum,0);
        }

        /// <summary>
        /// Sent by IOIO. Tells us how much buffer space is left.
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(II2cReportTxStatusFrom value)
        {
            int key = value.I2cNum;
            int newRemaining = UpdateTXBufferState(key, value.BytesRemaining);
            LOG.Debug("Device:" + key + " BufferDepth:" + newRemaining);
        }

        public void OnNext(II2cCloseFrom value)
        {
            ClearCount(value.I2cNum);
        }

        /// <summary>
        /// received when we are about to send data
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(ITwiMasterSendDataCommand value)
        {
            int key = value.TwiDef.TwiNum;
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
