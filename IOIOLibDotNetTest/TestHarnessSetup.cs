using IOIOLib.Util;
using log4net.Config;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;
using IOIOLib.Connection.Impl;
using IOIOLib.Connection;
using System.Collections.Generic;
using IOIOLib.Device.Impl;
using IOIOLib.IOIOException;
using IOIOLib.Device;

namespace IOIOLibDotNetTest
{
    [TestClass]
    public class TestHarnessSetup
    {

        /// <summary>
        /// Leaving this as NULL means we will try and autodiscover
        /// </summary>
        public static string GOOD_CONN_NAME = null;
        public static string BAD_CONN_NAME = "COMBogusName";


        [AssemblyInitialize]
        public static void Configure(TestContext tc)
        {
            //Diag output will go to the "output" logs if you add tehse two lines
            //TextWriterTraceListener writer = new TextWriterTraceListener(System.Console.Out);
            //Debug.Listeners.Add(writer);

            Debug.WriteLine("Diag Called inside Configure before log4net setup");
            XmlConfigurator.Configure(new FileInfo("log4net.properties"));
            // create the first logger AFTER we run the configuration
            ILog LOG = LogManager.GetLogger(typeof(TestHarnessSetup));
            //IOIOLog LOG = IOIOLogManager.GetLogger(typeof(LoggingSetup));
            LOG.Debug("log4net initialized for tests");
            Debug.WriteLine("Diag Called inside Configure after log4net setup");

            if (TestHarnessSetup.GOOD_CONN_NAME == null)
            {
                TestHarnessSetup.TryAndFindIOIODevice();
            }
        }

        public static void TryAndFindIOIODevice()
        {
            ILog LOG = LogManager.GetLogger(typeof(TestHarnessSetup));
            IOIOConnectionFactory factory = new SerialConnectionFactory();
            ICollection<IOIOConnection> connections = factory.createConnections();
            Assert.IsTrue(connections.Count > 0, "None of these tests can run because we can't find a possible IOIO port");
            LOG.Info("Found " + connections.Count + " possible com ports");

            /// probably don't need this since we aren't connected.
            foreach (IOIOConnection oneConn in connections)
            {
                try
                {
                    LOG.Info("Trying " + oneConn.ConnectionString());
                    oneConn.waitForConnect();
                    IOIOIncomingHandlerCaptureLog handlerLog = new IOIOIncomingHandlerCaptureLog(1);
                    IOIOIncomingHandlerCaptureState handlerState = new IOIOIncomingHandlerCaptureState();
                    IOIOIncomingHandler handler = new IOIOIncomingHandlerDistributor(
                        new List<IOIOIncomingHandler> { handlerLog, handlerState });
                    IOIOProtocolIncoming foo = new IOIOProtocolIncoming(oneConn.getInputStream(), handler);
                    System.Threading.Thread.Sleep(100); // WaitForChangedResult for hw ids
                    if (handlerState.OurHardware != null)
                    {
                        TestHarnessSetup.GOOD_CONN_NAME = oneConn.ConnectionString();
                        LOG.Info("Selecting " + oneConn.ConnectionString());
                        oneConn.disconnect();
                        break;
                    }
                    else
                    {
                        LOG.Info("Ignoring " + oneConn.ConnectionString());
                        oneConn.disconnect();
                    }
                }
                catch (ConnectionLostException e)
                {
                    // just ignore it because will get this when we disconnect
                }
            }

        }
    }
}
