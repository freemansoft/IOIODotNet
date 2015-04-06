using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IOIOLib.Device.Impl;
using IOIOLib.MessageFrom;
using IOIOLib.MessageFrom.Impl;

namespace IOIOLibDotNetTest.Device.Impl
{
    /// <summary>
    /// Summary description for IOIOHandlerNotifierTest
    /// </summary>
    [TestClass]
    public class IOIOHandlerNotifierTest
    {

        [TestMethod]
        public void IOIOHandlerNotifierTest_Filter()
        {
            IOIOHandlerNotifier notifier = new IOIOHandlerNotifier();
            UartFromObserver fromObsv = new UartFromObserver();
            UartFromPolyObserver dataFromObsv = new UartFromPolyObserver();
            notifier.Subscribe(fromObsv);
            notifier.Subscribe(dataFromObsv);

            notifier.HandleMessage(new UartDataFrom(0,0,null));
            notifier.HandleMessage(new UartCloseFrom(0));
            notifier.HandleMessage(new UartOpenFrom(0));

            Assert.AreEqual(3, fromObsv.count);
            Assert.AreEqual(1, dataFromObsv.countData);
            Assert.AreEqual(1, dataFromObsv.countOpen);
            Assert.AreEqual(1, dataFromObsv.countClose);
        }

    }

    public class UartFromObserver : IObserver<IUartFrom>, IObserverIOIO
    {
        internal int count;

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IUartFrom value)
        {
            count++;
        }
    }

    public class UartFromPolyObserver : IObserver<IUartDataFrom>, IObserver<IUartOpenFrom>, IObserver<IUartCloseFrom>, IObserverIOIO

    {
        internal int countData;
        internal int countClose;
        internal int countOpen;

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IUartDataFrom value)
        {
            countData++;
        }

        public void OnNext(IUartOpenFrom value)
        {
            countOpen++;
        }
        public void OnNext(IUartCloseFrom value)
        {
            countClose++;
        }
    }
}
