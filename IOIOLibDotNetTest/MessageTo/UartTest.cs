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
    public class UartTest : BaseTest
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(UartTest));

        [TestMethod]
        public void UartTest_LoopbackOut31In32()
        {
			//// TODO should use the hardware from the captured connection
			IResourceManager rManager = new ResourceManager(Hardware.IOIO0003);
			IOIOConnection ourConn = this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            ObserverTxStatusUart bufferObserver = new ObserverTxStatusUart();
            this.HandlerObservable_.Subscribe(bufferObserver);
            // add our own handler so we don't have to grovel aroudn in there
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(ourConn.GetInputStream(), HandlerObservable_);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(ourConn.GetOutputStream());
            System.Threading.Thread.Sleep(100);	// wait for us to get the hardware ids


			UartConfigureCommand commandCreate = new UartConfigureCommand(
				new DigitalInputSpec(32), new DigitalOutputSpec(31), 38400, UartParity.NONE, UartStopBits.ONE);
			commandCreate.Alloc(rManager);
			commandCreate.ExecuteMessage(fooOut);
            System.Threading.Thread.Sleep(10);

			string helloWorld = "Hello World";
			byte[] helloWorldBytes = System.Text.Encoding.ASCII.GetBytes(helloWorld);

            LOG.Debug("Sending Hello World");
			UartSendDataCommand commandSend = new UartSendDataCommand(commandCreate.UartDef, helloWorldBytes);
			commandSend.Alloc(rManager);
			commandSend.ExecuteMessage(fooOut);
			System.Threading.Thread.Sleep(50);

            LOG.Debug("Closing Uart");
            UartCloseCommand commandClose = new UartCloseCommand(commandCreate.UartDef);
			commandClose.Alloc(rManager);
			commandClose.ExecuteMessage(fooOut);
			System.Threading.Thread.Sleep(50);

			// IUartFrom is the parent interface for all messages coming from the UARt
			Assert.AreEqual(1+1+helloWorldBytes.Count()+1, this.CapturedSingleQueueAllType_.OfType<IUartFrom>().Count());

			Assert.AreEqual(1, this.CapturedSingleQueueAllType_.OfType<IUartOpenFrom>().Count(), "didn't get IUartOpenFrom");
			Assert.AreEqual(1, this.CapturedSingleQueueAllType_.OfType<IUartReportTxStatusFrom>().Count(), "didn't get IUartReportTXStatusFrom");

			IEnumerable<IUartDataFrom> readValues = this.CapturedSingleQueueAllType_.OfType<IUartDataFrom>();
            Assert.AreEqual(helloWorldBytes.Count(), readValues.Count(), "Didn't find the number of expected IUartFrom: "+readValues.Count());
			// logging the messages with any other string doesn't show the messages themselves !?
			LOG.Debug("Captured " + +this.CapturedSingleQueueAllType_.Count());
			LOG.Debug(this.CapturedSingleQueueAllType_.GetEnumerator());

			Assert.AreEqual (1, this.CapturedSingleQueueAllType_.OfType<IUartCloseFrom>().Count());
			// should verify close command in the resource
		}

        /// <summary>
        /// Assumes pins 31 and 32 are tied together
        /// </summary>
        [TestMethod]
        public void UartTest_BigBufferOut31In32()
        {
            //// TODO should use the hardware from the captured connection
            IOIOConnection ourConn = this.CreateGoodSerialConnection(false);
            this.CreateCaptureLogHandlerSet();
            // MUST use IOIOImpl to get "send" notifications for buffer management
            // add our own handler so we don't have to grovel around in there
            IOIO ourImpl = CreateIOIOImplAndConnect(ourConn, HandlerObservable_);
            LOG.Debug("Setup Complete");
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids

            UartConfigureCommand commandCreate = new UartConfigureCommand(
                new DigitalInputSpec(32), new DigitalOutputSpec(31), 19200, UartParity.NONE, UartStopBits.ONE);
            ourImpl.PostMessage(commandCreate);
            System.Threading.Thread.Sleep(10);

            // create byte buffer
            // only 64 can be sent in a single message
            StringBuilder builder = new StringBuilder();
            int numBlock10 = 3;
            for (int i = 0; i < numBlock10; i++)
            {
                builder.Append( "0123456789");
            }
            string helloWorld = builder.ToString();

            // should overrun the internal buffer to make sure observer flow control is working
            // buffer is 256 so cnt=4 means get one buffer update : cnt=7 means more than one full buffer
            char hack = 'A';
            int numBufferSend = 4;
            for (int i = 0; i < numBufferSend; i++) {
                byte[] helloWorldBytes = System.Text.Encoding.ASCII.GetBytes(helloWorld+hack);
                LOG.Debug("Sending long string " + i + ":" + helloWorldBytes.Length);
                UartSendDataCommand commandSend = new UartSendDataCommand(commandCreate.UartDef, helloWorldBytes);
                ourImpl.PostMessage(commandSend);
                hack++;
                System.Threading.Thread.Sleep(50);
            }
            // this will get blocked by data sends but it may run before all data sent from IOIO buffers
            System.Threading.Thread.Sleep(300);

            LOG.Debug("Closing Uart");
            UartCloseCommand commandClose = new UartCloseCommand(commandCreate.UartDef);
            ourImpl.PostMessage(commandClose);

            // either sleep for some time 
            //System.Threading.Thread.Sleep(5000);
            // or wait until we get the close ack
            int maxTimeMsec = 5000;
            while (this.CapturedSingleQueueAllType_.OfType<IUartCloseFrom>().Count() == 0) {
                Assert.IsTrue(maxTimeMsec > 0, "Did not receive IUartCloseFrom in the expected amount of time");
                System.Threading.Thread.Sleep(10);
                maxTimeMsec -= 10;
            }

            Assert.AreEqual(1, this.CapturedSingleQueueAllType_.OfType<IUartOpenFrom>().Count(), "didn't get IUartOpenFrom");
            // we should have counted bytes,  count is first buffer size + each update at the 130 mark
            int numberTxMessages = 1 + ((numBufferSend * numBlock10) / 130);
            Assert.AreEqual(numberTxMessages, this.CapturedSingleQueueAllType_.OfType<IUartReportTxStatusFrom>().Count(), "didn't get IUartReportTXStatusFrom");

            int expectedDataPacketsReceived = (numBlock10 * 10 + 1) * numBufferSend;
            IEnumerable<IUartDataFrom> readValues = this.CapturedSingleQueueAllType_.OfType<IUartDataFrom>();
            Assert.AreEqual(expectedDataPacketsReceived, readValues.Count(), "Didn't find the number of expected IUartFrom: " + readValues.Count());

            // logging the messages with any other string doesn't show the messages themselves !?
            LOG.Debug("Captured " + +this.CapturedSingleQueueAllType_.Count());
            LOG.Debug(this.CapturedSingleQueueAllType_.GetEnumerator());

            Assert.AreEqual(1, this.CapturedSingleQueueAllType_.OfType<IUartCloseFrom>().Count());
            // should verify close command in the Resource manager

            // We get back one packet for each character send + open, TX buffer status ,close
            int expectedNumDataPackets = expectedDataPacketsReceived;
            Assert.AreEqual(1 + 1 + expectedNumDataPackets + 1, this.CapturedSingleQueueAllType_.OfType<IUartFrom>().Count());

        }
    }
}
