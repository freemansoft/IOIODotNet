using IOIOLib.Component.Types;
using IOIOLib.Device.Impl;
using IOIOLib.Device.Types;
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
            this.CreateGoodSerialConnection(false);
            this.CreateCaptureLogHandlerSet();
            LOG.Debug("Setup Complete");

            // we'll add the handler state on top of the default handlers so we don't have to peek into impl
            IOIOImpl ourImpl = new IOIOImpl(this.GoodConnection_, this.HandlerQueuePerType_);
            ourImpl.waitForConnect();
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids

            // SHOULD USE THE FACTORY instead of this lame ...
            IConfigureDigitalOutputTo confDigitalOut = new ConfigureDigitalOutputTo(
                new IOIOLib.Component.Types.DigitalOutputSpec(SpecialPin.LED_PIN));
            ISetDigitalOutputValueTo turnItOn = new SetDigitalOutputValueTo(SpecialPin.LED_PIN, true);
            ISetDigitalOutputValueTo turnItOff = new SetDigitalOutputValueTo(SpecialPin.LED_PIN, false);

            ourImpl.postMessage(confDigitalOut);
            for (int i = 0; i < 8; i++)
            {
                System.Threading.Thread.Sleep(200);
                ourImpl.postMessage(turnItOn);
                System.Threading.Thread.Sleep(200);
                ourImpl.postMessage(turnItOff);
            }
            // there is no status to check
        }

        [TestMethod]
        public void IOIOImpl_DigitaLoopbackOut31In32()
        {
            this.CreateGoodSerialConnection(false);
            this.CreateCaptureLogHandlerSet();
            LOG.Debug("Setup Complete");

            // we'll add the handler state on top of the default handlers so we don't have to peek into impl
            IOIOImpl ourImpl = new IOIOImpl(this.GoodConnection_, this.HandlerQueuePerType_);
            ourImpl.waitForConnect();
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids

            // SHOULD USE THE FACTORY instead of this lame ...
            IConfigureDigitalOutputTo confDigitalOut =
                new ConfigureDigitalOutputTo(new DigitalOutputSpec(31));
            IConfigureDigitalInputTo configDigitalIn =
                new ConfigureDigitalInputTo(new DigitalInputSpec(32, DigitalInputSpecMode.PULL_UP), true);

            ISetDigitalOutputValueTo turnItOn = new SetDigitalOutputValueTo(31, true);
            ISetDigitalOutputValueTo turnItOff = new SetDigitalOutputValueTo(31, false);

            ourImpl.postMessage(confDigitalOut);
            ourImpl.postMessage(configDigitalIn);
            for (int i = 0; i < 8; i++)
            {
                System.Threading.Thread.Sleep(100);
                ourImpl.postMessage(turnItOn);
                System.Threading.Thread.Sleep(100);
                ourImpl.postMessage(turnItOff);
            }
            System.Threading.Thread.Sleep(100);

            ConcurrentQueue<IMessageFromIOIO> digitalMessagesIn = this.HandlerQueuePerType_.GetClassified(typeof(IDigitalInFrom));
            int changeCount =
                digitalMessagesIn.OfType<IReportDigitalInStatusFrom>().Where(m => m.Pin == 32).Count();

            Assert.AreEqual(1 + (2 * 8), changeCount, "trying to figure out how many changes we'd see");
        }
    }
}
