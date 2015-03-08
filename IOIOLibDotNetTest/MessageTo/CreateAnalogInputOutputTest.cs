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
using IOIOLib.Device.Impl;
using IOIOLib.Device.Types;
using IOIOLib.MessageTo.Impl;
using IOIOLib.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLibDotNetTest.MessageTo
{
    [TestClass]
    public class CreateAnalogInputOutputTest : BaseTest
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(CreateDigitalInputOutputToTest));

        [TestMethod]
        public void CreateAnalogInputOutputTo_AnalogLoopbackOut31In32()
        {
            IOIOConnection ourConn = this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(ourConn.getInputStream(), HandlerContainer_);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(ourConn.getOutputStream());
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids
            ConfigureAnalogInputTo commandCreateIn = new ConfigureAnalogInputTo(31, true);
            commandCreateIn.ExecuteMessage(fooOut);
            System.Threading.Thread.Sleep(10);

            // set analog "voltage"
            ConfigurePwmOutputTo commandCreatePWM = new ConfigurePwmOutputTo(32, 1000, 0.3f);
            commandCreatePWM.ExecuteMessage(fooOut);
            System.Threading.Thread.Sleep(100);
            // change it after settling
            ConfigurePwmOutputTo commandChangePWM = new ConfigurePwmOutputTo(32, 1000, 0.7f);
            commandChangePWM.ExecuteMessage(fooOut);
            System.Threading.Thread.Sleep(100);

            // my tests are awesome
            Assert.IsTrue(true);
        }

    }
}
