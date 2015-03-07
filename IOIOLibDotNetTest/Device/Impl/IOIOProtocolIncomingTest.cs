using IOIOLib.Component.Types;
using IOIOLib.Connection;
using IOIOLib.Connection.Impl;
using IOIOLib.Device;
using IOIOLib.Device.Impl;
using IOIOLib.IOIOException;
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
            this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(GoodConnection.getInputStream(), handler);
            // wait for reply
            System.Threading.Thread.Sleep(2000);
            Assert.IsNotNull(handlerState.EstablishConnectionFrom_);
        }

        [TestMethod]
        public void IOIOProtocolIncoming_CheckStreamClosure()
        {
            this.CreateCaptureLogHandlerSet();
            MemoryStream fakeStream = new MemoryStream();
            IOIOProtocolIncoming fooOut = new IOIOProtocolIncoming(fakeStream, handler);
            fakeStream.Close();
            System.Threading.Thread.Sleep(3000);
        }

        [TestMethod]
        public void IOIOProtocolOutgoing_ToggleOut31In32()
        {
            this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(GoodConnection.getInputStream(), handler);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(GoodConnection.getOutputStream());
            System.Threading.Thread.Sleep(100); // receive the HW ID
            LOG.Info("This test requires pin 31 and 32 be shorted together");
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
            // we get one change event as soon as the pin input pin is configured + 2 changes in test
            int matchingLogs = this.handlerLog.capturedLogs.Count(s => s.StartsWith("handleReportDigitalInStatus"));
            Assert.AreEqual(2, matchingLogs, "Should have captured input changes, not " + matchingLogs + ".  Are pins 31 and 32 shorted together");
            // verify the system acknowledged our request to be notified of state change
            Assert.IsTrue(this.handlerState.StateSetChangeNotify_.ContainsKey(31));
            // verify we got pin state changes for 31
            Assert.IsTrue(this.handlerState.StateReportDigitalInStatus_.ContainsKey(31));
        }
    }
}
