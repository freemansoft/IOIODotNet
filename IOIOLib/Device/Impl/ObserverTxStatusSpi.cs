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
    class ObserverTxStatusSpi : ObserverTxStatusBase, IObserver<ISpiReportTxStatusFrom>, IObserver<ISpiOpenFrom>, IObserver<ISpiCloseFrom>, IObserverIOIO
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

        public void OnNext(ISpiCloseFrom value)
        {
            ClearCount(value.SpiNum);
        }

        public void OnNext(ISpiOpenFrom value)
        {
            SetTXBufferState(value.SpiNum,0);
        }

        public void OnNext(ISpiReportTxStatusFrom value)
        {
            int key = value.SpiNum;
            int newRemaining = UpdateTXBufferState(key, value.BytesRemaining);
            LOG.Debug("Device:" + key + " BufferDepth:" + newRemaining);
        }
    }
}
