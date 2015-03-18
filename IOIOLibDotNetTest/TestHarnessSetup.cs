﻿/*
 * Copyright 2011 Ytai Ben-Tsvi. All rights reserved.
 * Copyright 2015 Joe Freeman. All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 * 
 *    1. Redistributions of source code must retain the above copyright notice, this list of
 *       conditions and the following disclaimer.
 * 
 *    2. Redistributions in binary form must reproduce the above copyright notice, this list
 *       of conditions and the following disclaimer in the documentation and/or other materials
 *       provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED 'AS IS AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL ARSHAN POURSOHI OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied.
 */
 
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
            LOG.Debug("Starting TryAndFindIOIODevice");
            IOIOConnectionFactory factory = new SerialConnectionFactory();
            ICollection<IOIOConnection> connections = factory.CreateConnections();
            Assert.IsTrue(connections.Count > 0, "None of these tests can run because we can't find a possible IOIO Port_");
            LOG.Info("Found " + connections.Count + " possible com ports");

            // probably don't need this since we aren't connected.
            foreach (IOIOConnection oneConn in connections)
            {
                // uses custom setup because we are trying to find IOIO not trying to do work with them
                try
                {
                    LOG.Info("Trying " + oneConn.ConnectionString());
                    oneConn.WaitForConnect();
                    // logging without real capture
                    IOIOHandlerCaptureLog handlerLog = new IOIOHandlerCaptureLog(1);
                    // so we can verifys
                    IOIOHandlerCaptureConnectionState handlerState = new IOIOHandlerCaptureConnectionState();
                    IOIOIncomingHandler handler = new IOIOHandlerDistributor(
                        new List<IOIOIncomingHandler> { handlerLog, handlerState });
                    IOIOProtocolIncoming foo = new IOIOProtocolIncoming(oneConn.GetInputStream(), handler);
                    System.Threading.Thread.Sleep(100); // WaitForChangedResult for hw ids
                    if (handlerState.EstablishConnectionFrom_ != null)
                    {
                        TestHarnessSetup.GOOD_CONN_NAME = oneConn.ConnectionString();
                        LOG.Info("Selecting " + oneConn.ConnectionString());
                        oneConn.Disconnect();
                        break;
                    }
                    else
                    {
                        LOG.Info("Ignoring " + oneConn.ConnectionString());
                        oneConn.Disconnect();
                    }
                }
                catch (ConnectionLostException e)
                {
                    LOG.Debug("Cought Exception Lost " + e.Message);
                    // just ignore it because will get this when we Disconnect
                }
            }
            LOG.Debug("Starting TryAndFindIOIODevice");
        }
    }
}
