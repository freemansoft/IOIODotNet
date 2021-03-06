﻿using IOIOLib.Message;
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
        IObserver<IUartCloseFrom>, 
        IObserver<IUartDataFrom>,
        IObserverIOIO
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
            SetTXBufferState(value.UartNum,0);
        }

        /// <summary>
        /// Sent by IOIO. Tells us how much buffer space is left.
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(IUartReportTxStatusFrom value)
        {
            int key = value.UartNum;
            ObserverTxStatusPoco newRemaining = UpdateTXBufferState(key, value.BytesRemaining,0,0);
            LOG.Debug("Device:" + key + " remaining after IUartReportTxStatusFrom:" + newRemaining);
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
                LOG.Debug("waiting:" + bytesBeforeAction);
                System.Threading.Thread.Sleep(5);
                bytesBeforeAction = GetTXBufferState(key);
            }
            ObserverTxStatusPoco newRemaining = UpdateTXBufferState(
                key, -value.PayloadSize(),value.Data.Length,0);
            LOG.Debug("Device:" + key + " remaining after IUartSendDataCommand:" + newRemaining);
        }

        /// <summary>
        /// has no buffer impact
        /// </summary>
        /// <param name="value">object containing data from IOIO</param>
        public void OnNext(IUartDataFrom value)
        {
            int key = value.UartNum;
            UpdateTXBufferState(key, 0, 0, value.NumDataBytes);
        }

    }
}
