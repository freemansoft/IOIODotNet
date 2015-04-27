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
        IObserver<II2cCloseFrom>, 
        IObserver<II2cResultFrom>,
        IObserverIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(ObserverTxStatusI2c));

        public void OnCompleted()
        {
            // do nothing
        }

        public void OnError(Exception error)
        {
            // do nothing
        }

        public void OnNext(II2cCloseFrom value)
        {
            ClearCount(value.I2cNum);
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
            ObserverTxStatusPoco newRemaining = UpdateTXBufferState(key, value.BytesRemaining,0,0);
            LOG.Debug("Device:" + key + " remaining after II2cReportTxStatusFrom:" + newRemaining);
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
                LOG.Debug("waiting:" + bytesBeforeAction);
                System.Threading.Thread.Sleep(5);
                bytesBeforeAction = GetTXBufferState(key);
            }
            ObserverTxStatusPoco newRemaining = UpdateTXBufferState(
                key, -value.PayloadSize(),value.Data.Length,0);
            LOG.Debug("Device:" + key + " remaining after ITwiMasterSendDataCommand:" + newRemaining);
        }

        /// <summary>
        /// has no buffer impact
        /// </summary>
        /// <param name="value">object containing data from IOIO</param>
        public void OnNext(II2cResultFrom value)
        {
            int key = value.I2cNum;
            // could also use value.Data.Length
            UpdateTXBufferState(key, 0, 0, value.NumDataBytes);
        }
    }
}
