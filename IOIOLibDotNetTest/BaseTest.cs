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

        internal IOIOConnection GoodConnection = null;

        internal IOIOIncomingHandlerCaptureLog handlerLog;
        internal IOIOIncomingHandlerCaptureState handlerState;
        internal IOIOIncomingHandler handler;
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
        /// Create new GoodConnection test collections before each test
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize()
        {
            ConnectionsOpenedDuringTest = new List<IOIOConnection>();
            GoodConnection = null;
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
            GoodConnection = null;
            System.Threading.Thread.Sleep(100);
        }

        /// <summary>
        /// Creates a "good" serial GoodConnection and registeres it for automatic closure
        /// </summary>
        /// <returns>connected that is set on instance variable</returns>
        internal void CreateGoodSerialConnection()
        {
            IOIOConnectionFactory factory = new SerialConnectionFactory();
            GoodConnection = factory.createConnection(TestHarnessSetup.GOOD_CONN_NAME);
            this.ConnectionsOpenedDuringTest.Add(GoodConnection); // always add connections used by incoming
            GoodConnection.waitForConnect(); // actually open the GoodConnection
        }

        /// <summary>
        /// Creates a standard handler set and put sit in instance variables
        /// </summary>
        internal void CreateCaptureLogHandlerSet()
        {
            handlerLog = new IOIOIncomingHandlerCaptureLog(10);
            handlerState = new IOIOIncomingHandlerCaptureState();
            handler = new IOIOIncomingHandlerDistributor(
               new List<IOIOIncomingHandler> { handlerLog, handlerState });
        }


    }
}
