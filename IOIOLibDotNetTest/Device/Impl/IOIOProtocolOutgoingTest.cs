using IOIOLib.Component.Types;
using IOIOLib.Connection;
using IOIOLib.Connection.Impl;
using IOIOLib.Device;
using IOIOLib.Device.Impl;
using IOIOLib.Device.Types;
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
    public class IOIOProtocolOutgoingTest : BaseTest
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOProtocolOutgoingTest));

        [TestMethod]
        public void IOIOProtocolOutgoing_CheckInterfaceVersionWrites()
        {
            this.CreateCaptureLogHandlerSet();
            MemoryStream fakeStream = new MemoryStream();
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(fakeStream);
            fooOut.checkInterfaceVersion();
            Assert.AreEqual(9, fakeStream.Length, "expected to send 8 characters when checking interface version");
            // could seek to 0 and read the byte from the buffer but this is easier
            byte[] streamBuffer = fakeStream.ToArray();
            Assert.AreEqual((int)IOIOProtocolCommands.CHECK_INTERFACE, streamBuffer[0]);

        }

        [TestMethod]
        public void IOIOProtocolOutgoing_CheckInterfaceVersion()
        {
            this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(GoodConnection.getInputStream(), handler);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(GoodConnection.getOutputStream());
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids
            fooOut.checkInterfaceVersion();
            // wait for reply
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(handlerState.Supported_.IsSupported(), " the handler returned not supported interface.");
        }

        [TestMethod]
        public void IOIOProtocolOutgoing_ToggleLED()
        {
            this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(GoodConnection.getInputStream(), handler);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(GoodConnection.getOutputStream());
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids

            fooOut.setPinDigitalOut(SpecialPin.LED_PIN, false, DigitalOutputSpecMode.NORMAL);
            for (int i = 0; i < 8; i++)
            {
                System.Threading.Thread.Sleep(200);
                fooOut.setDigitalOutLevel(SpecialPin.LED_PIN, true);
                System.Threading.Thread.Sleep(200);
                fooOut.setDigitalOutLevel(SpecialPin.LED_PIN, false);
            }
        }


    }
}
