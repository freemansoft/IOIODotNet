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
using IOIOLib.MessageFrom;
using IOIOLib.MessageTo;
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
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(CreateAnalogInputOutputTest));

        [TestMethod]
        public void CreateAnalogInputOutputTo_AnalogLoopbackOut31In32()
        {
			//// TODO should use the hardware from the captured connection
			IResourceManager rManager = new ResourceManager(Hardware.IOIO0003);
			IOIOConnection ourConn = this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(ourConn.GetInputStream(), HandlerContainer_);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(ourConn.GetOutputStream());
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids
            AnalogInputConfigureCommand commandCreateIn = new AnalogInputConfigureCommand(31, true);
			commandCreateIn.Alloc(rManager);
            commandCreateIn.ExecuteMessage(fooOut);
            System.Threading.Thread.Sleep(10);
			DigitalOutputSpec pwmPinSpec = new DigitalOutputSpec(32, DigitalOutputSpecMode.NORMAL);

            // set analog "voltage"
            PwmOutputConfigureCommand commandCreatePWM = new PwmOutputConfigureCommand(pwmPinSpec, 1000, 0.3f);
			commandCreatePWM.Alloc(rManager);
			commandCreatePWM.ExecuteMessage(fooOut);
            System.Threading.Thread.Sleep(100);
			// change it after settling
			PwmOutputUpdateCommand commandChangePWM = new PwmOutputUpdateCommand(commandCreatePWM.PwmSpec,  0.7f);
			commandChangePWM.Alloc(rManager);
			commandChangePWM.ExecuteMessage(fooOut);
            System.Threading.Thread.Sleep(100);
			PwmOutputCloseCommand commandReleasePwm = new PwmOutputCloseCommand(commandCreatePWM.PwmSpec);
			commandReleasePwm.Alloc(rManager);
			commandReleasePwm.ExecuteMessage(fooOut);
			System.Threading.Thread.Sleep(50);

			IEnumerable<IReportAnalogPinValuesFrom> readValues = this.HandlerSingleQueueAllType_.CapturedMessages_
				.OfType<IReportAnalogPinValuesFrom>();
            Assert.IsTrue(readValues.Count() >= 1, "Didn't find the number of expected IReportAnalogPinValuesFrom: "+readValues.Count());
			// logging the messages with any other string doesn't show the messages themselves !?
			LOG.Debug("Captured " + +this.HandlerSingleQueueAllType_.CapturedMessages_.Count);
			LOG.Debug(this.HandlerSingleQueueAllType_.CapturedMessages_);
			// should verify close command
		}

	}
}
