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
 
using IOIOLib.Component.Types;
using IOIOLib.Connection;
using IOIOLib.Connection.Impl;
using IOIOLib.Device;
using IOIOLib.Device.Impl;
using IOIOLib.IOIOException;
using IOIOLib.MessageFrom;
using IOIOLib.MessageFrom.Impl;
using IOIOLib.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLibDotNetTest.Device.Impl
{
    [TestClass]
    public class IOIOProtocolIncomingTest : BaseTest
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOProtocolIncomingTest));

        [TestMethod]
        public void IOIOProtocolIncoming_TestStartRunLoop()
        {
            IOIOConnection ourConn = this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            LOG.Debug("Setup Complete");
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(ourConn.GetInputStream(), HandlerContainer_);
            // wait for reply
            System.Threading.Thread.Sleep(2000);
            Assert.IsNotNull(HandlerQueuePerType_.EstablishConnectionFrom_);
        }

        [TestMethod]
        public void IOIOProtocolIncoming_CheckStreamClosure()
        {
            this.CreateCaptureLogHandlerSet();
            MemoryStream fakeStream = new MemoryStream();
            IOIOProtocolIncoming fooOut = new IOIOProtocolIncoming(fakeStream, HandlerContainer_);
            fakeStream.Close();
            System.Threading.Thread.Sleep(3000);
        }

        [TestMethod]
        public void IOIOProtocolOutgoing_DigitalLoopbackOut31In32()
        {
            IOIOConnection ourConn = this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            LOG.Debug("Setup Complete");

            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(ourConn.GetInputStream(), HandlerContainer_);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(ourConn.GetOutputStream());
            System.Threading.Thread.Sleep(100); // receive the HW ID
            LOG.Info("This test requires Pin 31 and 32 be shorted together");
            fooOut.setPinDigitalIn(31, DigitalInputSpecMode.FLOATING);
            // request to be told of state change.  system will acknowledge this
            fooOut.setChangeNotify(31, true);
            // first change that will be captured...
            fooOut.setPinDigitalOut(32, false, DigitalOutputSpecMode.NORMAL);
            // second change that is captured
            fooOut.setDigitalOutLevel(32, true);
            // we could wait until our acknowledgements are received
            System.Threading.Thread.Sleep(200);
            // all log  methods contain method name which is in the interface so this is reasonably safe
            // we get one change event as soon as the Pin input Pin is configured + 2 changes in test
            int matchingLogs = this.HandlerLog_.CapturedLogs_.Count(s => s.StartsWith("HandleReportDigitalInStatus"));
            Assert.AreEqual(3, matchingLogs, "Should have captured input changes, not " + matchingLogs + ".  Are pins 31 and 32 shorted together");
            // verify the system acknowledged our request to be notified of state change
            Assert.AreEqual(1, this.HandlerQueuePerType_.GetClassified(typeof(ISetChangeNotifyMessageFrom))
                .OfType<ISetChangeNotifyMessageFrom>().Where(m => m.Pin == 31).Count()
                , "Unexpected count for IReportDigitalInStatusFrom");
            // verify we got Pin state changes for 31
            Assert.AreEqual(3, this.HandlerQueuePerType_.GetClassified(typeof(IDigitalInFrom))
                .OfType<IReportDigitalInStatusFrom>().Where(m => m.Pin == 31).Count()
                , "Unexpected count for IReportDigitalInStatusFrom");
        }
    }
}
