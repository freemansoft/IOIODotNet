using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IOIOLib.Device.Impl;
using IOIOLib.MessageFrom;
using IOIOLib.MessageFrom.Impl;
using IOIOLib.Util;

namespace IOIOLibDotNetTest.Device.Impl
{
    /// <summary>
    /// Summary description for IOIOHandlerNotifierTest
    /// </summary>
    [TestClass]
    public class IOIOHandlerObservableTest
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOHandlerObservableTest));

        /// <summary>
        /// This test often succeeds standalone but fails in a group test.
        /// Could be a timing / load problem
        /// </summary>
        [TestMethod]
        public void IOIOHandlerObservableTest_SingleThread()
        {
            IOIOHandlerObservable notifier = new IOIOHandlerObservable();
            UartFromObserver fromObsv = new UartFromObserver();
            UartFromPolyObserver dataFromObsv = new UartFromPolyObserver();
            notifier.Subscribe(fromObsv);
            notifier.Subscribe(dataFromObsv);

            notifier.HandleMessage(new UartDataFrom(0, 0, null));
            notifier.HandleMessage(new UartCloseFrom(0));
            notifier.HandleMessage(new UartOpenFrom(0));

            System.Threading.Thread.Sleep(20);
            Assert.AreEqual(3, fromObsv.count);
            Assert.AreEqual(1, dataFromObsv.countData);
            Assert.AreEqual(1, dataFromObsv.countOpen);
            Assert.AreEqual(1, dataFromObsv.countClose);

            Assert.AreEqual(1, fromObsv.managedThreads.Count);
            Assert.IsTrue(fromObsv.managedThreads.SetEquals(dataFromObsv.managedThreads));
        }

        /// <summary>
        /// This test often succeeds standalone but fails in a group test.
        /// Could be a timing / load problem
        /// </summary>
        //[TestMethod]
        public void IOIOHandlerObservableTest_NoWaitSingleThreaded()
        {
            IOIOHandlerObservableNoWait notifier = new IOIOHandlerObservableNoWait();
            UartFromObserver fromObsv = new UartFromObserver();
            UartFromPolyObserver dataFromObsv = new UartFromPolyObserver();
            notifier.Subscribe(fromObsv);
            notifier.Subscribe(dataFromObsv);

            notifier.HandleMessage(new UartDataFrom(0,0,null));
            notifier.HandleMessage(new UartCloseFrom(0));
            notifier.HandleMessage(new UartOpenFrom(0));

            System.Threading.Thread.Sleep(20);
            Assert.AreEqual(3, fromObsv.count);
            Assert.AreEqual(1, dataFromObsv.countData);
            Assert.AreEqual(1, dataFromObsv.countOpen);
            Assert.AreEqual(1, dataFromObsv.countClose);

            Assert.IsTrue(fromObsv.managedThreads.SetEquals(dataFromObsv.managedThreads));
        }

        [TestMethod]
        /// <summary>
        /// This test often succeeds standalone but fails in a group test.
        /// Could be a timing / load problem
        /// </summary>
        public void IOIOHandlerObservableTest_NoWaitParallel()
        {
            IOIOHandlerObservableNoWaitParallel notifier = new IOIOHandlerObservableNoWaitParallel();
            UartFromObserver fromObsv = new UartFromObserver();
            UartFromPolyObserver dataFromObsv = new UartFromPolyObserver();
            notifier.Subscribe(fromObsv);
            notifier.Subscribe(dataFromObsv);

            notifier.HandleMessage(new UartDataFrom(0, 0, null));
            notifier.HandleMessage(new UartCloseFrom(0));
            notifier.HandleMessage(new UartOpenFrom(0));

            // have to wait some time for the asynch notifications to be received
            System.Threading.Thread.Sleep(20);
            Assert.AreEqual(3, fromObsv.count);
            Assert.AreEqual(1, dataFromObsv.countData);
            Assert.AreEqual(1, dataFromObsv.countOpen);
            Assert.AreEqual(1, dataFromObsv.countClose);

            // they could be the same but it is unlikely
            Assert.IsFalse(fromObsv.managedThreads.SetEquals(dataFromObsv.managedThreads));
        }
    }

    public class UartFromObserver : IObserver<IUartFrom>, IObserverIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(UartFromObserver));
        internal int count;
        internal SortedSet<int> managedThreads = new SortedSet<int>();

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
            managedThreads.Add(System.Threading.Thread.CurrentThread.ManagedThreadId);
            LOG.Debug("thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
    }

    public class UartFromPolyObserver : IObserver<IUartDataFrom>, IObserver<IUartOpenFrom>, IObserver<IUartCloseFrom>, IObserverIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(UartFromPolyObserver));
        internal int countData;
        internal int countClose;
        internal int countOpen;
        internal SortedSet<int> managedThreads = new SortedSet<int>();

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
            managedThreads.Add(System.Threading.Thread.CurrentThread.ManagedThreadId);
            LOG.Debug("thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
        }

        public void OnNext(IUartOpenFrom value)
        {
            countOpen++;
            managedThreads.Add(System.Threading.Thread.CurrentThread.ManagedThreadId);
            LOG.Debug("thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
        public void OnNext(IUartCloseFrom value)
        {
            countClose++;
            managedThreads.Add(System.Threading.Thread.CurrentThread.ManagedThreadId);
            LOG.Debug("thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
    }
}
