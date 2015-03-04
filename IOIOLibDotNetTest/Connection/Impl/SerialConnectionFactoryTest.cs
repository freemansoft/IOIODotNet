using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IOIOLib.Connection.Impl;
using IOIOLib.Connection;
using log4net.Config;
using IOIOLib.Util;
using System.IO;
using IOIOLib.IOIOException;

namespace IOIOLibDotNetTest.Connection.Impl
{
    /// <summary>
    /// Summary description for SerialConnectionFactoryTest
    /// </summary>
    [TestClass]
    public class SerialConnectionFactoryTest : BaseTest
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(SerialConnectionFactoryTest));
        [TestMethod]
        public void SerialConnectionFactory_CreateConnections()
        {
            IOIOConnectionFactory factory = new SerialConnectionFactory();
            ICollection<IOIOConnection> connections = factory.createConnections();
            Assert.IsTrue(connections.Count > 0);
            LOG.Info("Found " + connections.Count + " possible com ports");

            /// probably don't need this since we aren't connected.
            foreach (IOIOConnection oneConn in connections)
            {
                oneConn.disconnect();
            }
        }


        [TestMethod]
        [ExpectedExceptionAttribute(typeof(ConnectionCreationException))]
        public void SerialConnectionFactory_CreateConnectionBad()
        {
            IOIOConnectionFactory factory = new SerialConnectionFactory();
            IOIOConnection connection = factory.createConnection(TestHarnessSetup.BAD_CONN_NAME);
            LOG.Info("Should have failed test on " + TestHarnessSetup.BAD_CONN_NAME);
        }
    }
}
