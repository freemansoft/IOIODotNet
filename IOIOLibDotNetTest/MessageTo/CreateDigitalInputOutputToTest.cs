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
    public class CreateDigitalInputOutputToTest : BaseTest
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(CreateDigitalInputOutputToTest));

        [TestMethod]
        public void CreateDigitalOutputTo_ToggleLED()
        {
            this.CreateGoodSerialConnection();
            this.CreateCaptureLogHandlerSet();
            IOIOProtocolIncoming fooIn = new IOIOProtocolIncoming(GoodConnection.getInputStream(), handler);
            IOIOProtocolOutgoing fooOut = new IOIOProtocolOutgoing(GoodConnection.getOutputStream());
            System.Threading.Thread.Sleep(100); // wait for us to get the hardware ids

            ConfigureDigitalOutputTo commandSetup = new ConfigureDigitalOutputTo(
                new DigitalOutputSpec(SpecialPin.LED_PIN, DigitalOutputSpecMode.NORMAL), false);
            SetDigitalOutputValueTo commandOn = new SetDigitalOutputValueTo(SpecialPin.LED_PIN, true);
            SetDigitalOutputValueTo commandOff = new SetDigitalOutputValueTo(SpecialPin.LED_PIN, false);

            commandSetup.ExecuteMessage(fooOut);
            for (int i = 0; i < 8; i++)
            {
                System.Threading.Thread.Sleep(200);
                commandOn.ExecuteMessage(fooOut);
                System.Threading.Thread.Sleep(200);
                commandOff.ExecuteMessage(fooOut);
            }
            // my tests are awesome
            Assert.IsTrue(true); ;
        }

    }
}
