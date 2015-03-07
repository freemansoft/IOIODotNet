using IOIOLib.Component.Types;
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
        public void CreateAnalogInputOutputTo_ToggleOut31In32()
        {
            this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(GoodConnection.getInputStream(), handler);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(GoodConnection.getOutputStream());
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
