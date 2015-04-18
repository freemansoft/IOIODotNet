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
            IOIOConnection ourConn = this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            LOG.Debug("Setup Complete");
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(ourConn.GetInputStream(), HandlerObservable_);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(ourConn.GetOutputStream());
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids
            fooOut.checkInterfaceVersion();
            // wait for reply
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(CapturedConnectionState_.Supported_.IsSupported, " the HandlerContainer_ returned not supported interface.");
        }

        [TestMethod]
        public void IOIOProtocolOutgoing_ToggleLED()
        {
            IOIOConnection ourConn = this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            LOG.Debug("Setup Complete");

            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(ourConn.GetInputStream(), HandlerObservable_);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(ourConn.GetOutputStream());
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids

            fooOut.setPinDigitalOut(Spec.LED_PIN, false, DigitalOutputSpecMode.NORMAL);
            for (int i = 0; i < 8; i++)
            {
                System.Threading.Thread.Sleep(200);
                fooOut.setDigitalOutLevel(Spec.LED_PIN, true);
                System.Threading.Thread.Sleep(200);
                fooOut.setDigitalOutLevel(Spec.LED_PIN, false);
            }
            Assert.IsTrue(true, "there is no status to check");
        }


    }
}
