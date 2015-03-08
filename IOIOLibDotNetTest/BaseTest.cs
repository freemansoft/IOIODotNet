/*
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

        /// <summary>
        /// Capture all connections here so we can make sure we clean them up
        /// </summary>
        private List<IOIOConnection> ConnectionsOpenedDuringTest;
        private List<IOIO> DevicesOpenedDuringTest;

        private IOIOConnection GoodConnection_ = null;

        internal IOIOHandlerCaptureLog HandlerLog_;
        internal IOIOHandlerCaptureSeparateQueue HandlerQueuePerType_;
        internal IOIOHandlerCaptureSingleQueue HandlerSingleQueueAllType_;
        internal IOIOHandlerDistributor HandlerContainer_;

        /// <summary>
        /// Create new GoodConnection_ test collections before each test
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize()
        {
            ConnectionsOpenedDuringTest = new List<IOIOConnection>();
            DevicesOpenedDuringTest = new List<IOIO>();
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
            DevicesOpenedDuringTest.ForEach(x =>
            {
                x.disconnect();
                LOG.Info("Disconnected " + x.ToString());
            });
            ConnectionsOpenedDuringTest.ForEach(x =>
                {
                    if (x.canClose())
                    {
                        x.disconnect();
                        LOG.Info("Disconnected " + x.ToString());
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
        internal IOIOConnection CreateGoodSerialConnection(bool leaveConnectionOpen = true)
        {
            IOIOConnectionFactory factory = new SerialConnectionFactory();
            GoodConnection_ = factory.createConnection(TestHarnessSetup.GOOD_CONN_NAME);
            this.ConnectionsOpenedDuringTest.Add(GoodConnection_); // always add connections used by incoming
            if (leaveConnectionOpen)
            {
                GoodConnection_.waitForConnect(); // actually IsOpen the GoodConnection_
            }
            LOG.Debug("Done CreateGoodSerialConnection");
            return GoodConnection_;

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

        /// <summary>
        /// Use this to create your IOIO because it retains references to Tasks that are automatically cleaned up for you
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        internal IOIO CreateIOIOImplAndConnect(IOIOConnection connection, IOIOIncomingHandler handler)
        {
            IOIO ourImpl = new IOIOImpl(connection, handler);
            DevicesOpenedDuringTest.Add(ourImpl);
            ourImpl.waitForConnect();
            return ourImpl;
        }

    }
}
