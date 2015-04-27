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
using IOIOLib.Device;
using IOIOLib.Device.Impl;
using IOIOLib.Device.Types;
using IOIOLib.Message;
using IOIOLib.MessageFrom;
using IOIOLib.MessageFrom.Impl;
using IOIOLib.MessageTo;
using IOIOLib.MessageTo.Impl;
using IOIOLib.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLibDotNetTest.Device.Impl
{
    [TestClass]
    public class IOIOImplTest : BaseTest
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOImplTest));

        [TestMethod]
        public void IOIOImpl_ToggleLED()
        {
            IOIOConnection ourConn = this.CreateGoodSerialConnection(false);
            this.CreateCaptureLogHandlerSet();
            // we'll add the handler state on top of the default handlers so we don't have to peek into impl
            IOIO ourImpl = CreateIOIOImplAndConnect(ourConn,
                new List<IObserverIOIO>() { this.CapturedConnectionState_, this.CapturedSingleQueueAllType_, this.CapturedLogs_ });

            LOG.Debug("Setup Complete");
            System.Threading.Thread.Sleep(100);	// wait for us to get the hardware ids

			DigitalOutputSpec ledSpec = new DigitalOutputSpec(Spec.LED_PIN);
			// SHOULD USE THE FACTORY instead of this lame ...
			IDigitalOutputConfigureCommand confDigitalOut = new DigitalOutputConfigureCommand(
				ledSpec);
            IDigitalOutputValueSetCommand turnItOn = new DigitalOutputSetValueCommand(ledSpec, true);
            IDigitalOutputValueSetCommand turnItOff =new DigitalOutputSetValueCommand(ledSpec, false);

            ourImpl.PostMessage(confDigitalOut);
            for (int i = 0; i < 8; i++)
            {
                System.Threading.Thread.Sleep(150);
                ourImpl.PostMessage(turnItOn);
                System.Threading.Thread.Sleep(150);
                ourImpl.PostMessage(turnItOff);
            }
            // this test can fail if we can't connect to the device but otherwise no way to read pin 1 values
            Assert.IsTrue(true, "there is no status to check");
        }

        [TestMethod]
        public void IOIOImpl_DigitaLoopbackOut31In32()
        {
            IOIOConnection ourConn = this.CreateGoodSerialConnection(false);
            this.CreateCaptureLogHandlerSet();
            LOG.Debug("Setup Complete");

            // we'll add the handler state on top of the default handlers so we don't have to peek into impl
            IOIO ourImpl = CreateIOIOImplAndConnect(ourConn,
                new List<IObserverIOIO>() { this.CapturedConnectionState_, this.CapturedSingleQueueAllType_, this.CapturedLogs_ });
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids

            // SHOULD USE THE FACTORY instead of this lame ...
            IDigitalOutputConfigureCommand confDigitalOut =
                new DigitalOutputConfigureCommand(new DigitalOutputSpec(31));
            IDigitalInputConfigureCommand configDigitalIn =
                new DigitalInputConfigureCommand(new DigitalInputSpec(32, DigitalInputSpecMode.PULL_UP), true);

            IDigitalOutputValueSetCommand turnItOn =new DigitalOutputSetValueCommand(new DigitalOutputSpec(31), true);
            IDigitalOutputValueSetCommand turnItOff =new DigitalOutputSetValueCommand(new DigitalOutputSpec(31), false);

            ourImpl.PostMessage(confDigitalOut);
            ourImpl.PostMessage(configDigitalIn);
            for (int i = 0; i < 8; i++)
            {
                System.Threading.Thread.Sleep(100);
                ourImpl.PostMessage(turnItOn);
                System.Threading.Thread.Sleep(100);
                ourImpl.PostMessage(turnItOff);
            }
            System.Threading.Thread.Sleep(100);

			IEnumerable<IMessageFromIOIO> digitalMessagesIn = this.CapturedSingleQueueAllType_;
            int changeCount =
                digitalMessagesIn.OfType<IReportDigitalInStatusFrom>().Where(m => m.Pin == 32).Count();

            Assert.AreEqual(1 + (2 * 8), changeCount, "trying to figure out how many changes we'd see");
        }

    }
}
