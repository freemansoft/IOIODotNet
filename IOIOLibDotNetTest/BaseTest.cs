using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IOIOLib.Device.Impl;
using IOIOLib.Connection;
using System.Collections.Generic;
using IOIOLib.Util;
using IOIOLib.Connection.Impl;
using IOIOLib.Device;

namespace IOIOLibDotNetTest
{
    [TestClass]
    public class BaseTest
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(BaseTest));

        public BaseTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        /// Capture all connections here so we can make sure we clean them up
        /// </summary>
        internal List<IOIOConnection> ConnectionsOpenedDuringTest;

        internal IOIOConnection GoodConnection_ = null;

        internal IOIOHandlerCaptureLog HandlerLog_;
        internal IOIOHandlerCaptureSeparateQueue HandlerQueuePerType_;
        internal IOIOHandlerCaptureSingleQueue HandlerSingleQueueAllType_;
        internal IOIOHandlerDistributor HandlerContainer_;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }

        /// <summary>
        /// Create new GoodConnection_ test collections before each test
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize()
        {
            ConnectionsOpenedDuringTest = new List<IOIOConnection>();
            GoodConnection_ = null;
            LOG.Debug("Done MyTestInitialize");

        }


        /// <summary>
        /// Close all opened IOIO connections. Do this here instead of tests 
        /// so that it gets done even if tests fail or throw excpeiotns
        /// </summary>
        [TestCleanup()]
        public void MyTestCleanup()
        {
            ConnectionsOpenedDuringTest.ForEach(x =>
                {
                    if (x.canClose())
                    {
                        x.disconnect();
                        LOG.Info("Disconnecting " + x.ToString());
                    }
                });
            GoodConnection_ = null;
            System.Threading.Thread.Sleep(100);
            LOG.Debug("Done MyTestCleanup");
        }

        /// <summary>
        /// Creates a "good" serial GoodConnection_ and registeres it for automatic closure
        /// </summary>
        /// <param name="leaveConnectionOpen">defaults to true because that is the way the first tests ran.
        ///     set to false for IOIOImpl</param>
        /// <returns>connected that is set on instance variable</returns>
        internal void CreateGoodSerialConnection(bool leaveConnectionOpen = true)
        {
            IOIOConnectionFactory factory = new SerialConnectionFactory();
            GoodConnection_ = factory.createConnection(TestHarnessSetup.GOOD_CONN_NAME);
            this.ConnectionsOpenedDuringTest.Add(GoodConnection_); // always add connections used by incoming
            if (leaveConnectionOpen)
            {
                GoodConnection_.waitForConnect(); // actually IsOpen the GoodConnection_
            }
            LOG.Debug("Done CreateGoodSerialConnection");

        }

        /// <summary>
        /// Creates a standard HandlerContainer_ set and put it in instance variables so all tests can use.
        /// Create one of each of the standard types
        /// </summary>
        internal void CreateCaptureLogHandlerSet()
        {
            HandlerLog_ = new IOIOHandlerCaptureLog(10);
            HandlerQueuePerType_ = new IOIOHandlerCaptureSeparateQueue();
            HandlerSingleQueueAllType_ = new IOIOHandlerCaptureSingleQueue();
            HandlerContainer_ = new IOIOHandlerDistributor(
               new List<IOIOIncomingHandler> { HandlerLog_, 
                   HandlerQueuePerType_, 
                   HandlerSingleQueueAllType_ 
               });
        }


    }
}
