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
	public class TwiI2CTest : BaseTest
	{
		private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(TwiI2CTest));

        /// <summary>
        /// Parallax Gyroscope  L3G4200D found at Radio Shack in 2014
        /// You must use pullups on the IOIO I2C pins for this to work. I used 10K Ohm
        /// This test has not yet been run with pullups -- joe needs to teset this more
        /// </summary>
        //[TestMethod]
        public void TwiI2CTest_L3G4200D_Integration()
		{
            // slave address is 7 bits, the last bit is set by SD0 line
            int GyroSlaveAddress0 = Convert.ToInt32("1101000", 2);
            int GyroSlaveAddress1 = Convert.ToInt32("1101001", 2);
            byte Gyro_WhoAmI_Register = 0x0f;
            //byte Gyro_CTRL_REG1 = 0x20;
            byte Gyro_WhoAmI_ID = 0xD3;
            byte[] ReadWhoAmiRegisterData = new Byte[] { Gyro_WhoAmI_Register };
            //byte[] ReatCtrlReg1RegisterData = new Byte[] { Gyro_CTRL_REG1 };


            IOIOConnection ourConn = this.CreateGoodSerialConnection(false);
            this.CreateCaptureLogHandlerSet();
            LOG.Debug("Setup Complete");

            // we'll add the handler state on top of the default handlers so we don't have to peek into impl
            IOIO ourImpl = CreateIOIOImplAndConnect(ourConn, this.HandlerSingleQueueAllType_);
            System.Threading.Thread.Sleep(100);  // wait for us to get the hardware ids

            IOIOMessageCommandFactory factory = new IOIOMessageCommandFactory();
            ITwiMasterConfigureCommand startCommand = factory.CreateTwiConfigure(0, TwiMasterRate.RATE_400KHz, false);
            ourImpl.PostMessage(startCommand);
            System.Threading.Thread.Sleep(50);
            TwiSpec twiDef = startCommand.TwiDef;

            // send the whoami command
            ITwiMasterSendDataCommand startupCommand0 = factory.CreateTwiSendData(twiDef, GyroSlaveAddress0, false, ReadWhoAmiRegisterData, 1);
            ourImpl.PostMessage(startupCommand0);
            System.Threading.Thread.Sleep(50);

            //ITwiMasterSendDataCommand readRegister1 = factory.CreateTwiSendData(twiDef, GyroSlaveAddress0, false, ReatCtrlReg1RegisterData, 1);
            //ourImpl.PostMessage(readRegister1);
            //System.Threading.Thread.Sleep(50);

            ITwiMasterSendDataCommand startupCommand1 = factory.CreateTwiSendData(twiDef, GyroSlaveAddress1, false, ReadWhoAmiRegisterData, 1);
            ourImpl.PostMessage(startupCommand1);
            System.Threading.Thread.Sleep(50);

            ITwiMasterCloseCommand closeCommand = factory.CreateTwiClose(twiDef);
            ourImpl.PostMessage(closeCommand);
            System.Threading.Thread.Sleep(100);

            // logging the messages with any other string doesn't show the messages themselves !?
            LOG.Debug("Captured " + +this.HandlerSingleQueueAllType_.Count());
			LOG.Debug(this.HandlerSingleQueueAllType_.GetEnumerator());
			// should verify close command
		}

        /// <summary>
        /// http://jeelabs.net/projects/hardware/wiki/Expander_Plug
        /// It uses the MCP23008 expander chip http://www.microchip.com/wwwproducts/Devices.aspx?product=MCP23008
        /// You must use pullups on the IOIO I2C pins for this to work. I used 10K Ohm
        /// </summary>
        [TestMethod]
        public void TwiI2CTest_JEELabsExpander_Integration()
        {
            int ExpectedReceiveCount = 0;

            int TwiVirtualDevice = 0;
            // slave address is 7 bits, ExpanderPlug uses 0x20, 0x21, 0x22 0x23 based on jumpers
            int JeeExpanderAddress = 0x20;
            byte RegisterIoDir = 0x00;
            byte RegisterIPol = 0x01;       // input polarity
            byte RegisterGpIntEna = 0x02;
            byte RegisterDefVal = 0x03;
            byte RegisterIntCon = 0x04;
            byte RegisterIoCon = 0x05;      // controls auto increment for read
            byte RegisterGppu = 0x06;
            byte RegisterIntf = 0x07;
            byte RegisterIntCap = 0x08;
            byte RegisterGpio = 0x09;       // Port values.  Writing modifes the OLat
            byte RegisterOLat = 0x0a;       // Output Latch

            // all ouitput and inverted
            byte[] ConfigureAllOutput = new Byte[] { RegisterIoDir, 0x00 };
            byte[] ReadRegisterIoDir = new Byte[] { RegisterIoDir };
            byte[] ReadRegisterOutputLatch = new Byte[] { RegisterOLat };

            byte[] WriteAllHigh = new Byte[] { RegisterGpio, 0xFF };
            byte[] WriteAllLow = new Byte[] { RegisterGpio, 0x00 };


            IOIOConnection ourConn = this.CreateGoodSerialConnection(false);
            this.CreateCaptureLogHandlerSet();
            LOG.Debug("Setup Complete");

            // we'll add the handler state on top of the default handlers so we don't have to peek into impl
            ExpectedReceiveCount++;
            IOIO ourImpl = CreateIOIOImplAndConnect(ourConn, this.HandlerSingleQueueAllType_);
            System.Threading.Thread.Sleep(100);  // wait for us to get the hardware ids

            LOG.Debug("Configuring TWI");
            IOIOMessageCommandFactory factory = new IOIOMessageCommandFactory();
            ITwiMasterConfigureCommand startCommand = factory.CreateTwiConfigure(TwiVirtualDevice, TwiMasterRate.RATE_400KHz, false);
            ourImpl.PostMessage(startCommand);
            ExpectedReceiveCount+=2;    // HandleI2cOpen HandleI2cReportTxStatus
            System.Threading.Thread.Sleep(50);
            TwiSpec twiDef = startCommand.TwiDef;

            LOG.Debug("Reading Initial Register Status");
            ITwiMasterSendDataCommand readInitalRegisterState = factory.CreateTwiSendData(twiDef, JeeExpanderAddress, false, ReadRegisterIoDir, 11);
            ourImpl.PostMessage(readInitalRegisterState);
            ExpectedReceiveCount++; // I2cResultFrom
            System.Threading.Thread.Sleep(50);
            LOG.Debug("Configuring port direction as all output ");
            ITwiMasterSendDataCommand configureDirectionCommand = factory.CreateTwiSendData(twiDef, JeeExpanderAddress, false, ConfigureAllOutput, 0);
            ourImpl.PostMessage(configureDirectionCommand);
            ExpectedReceiveCount++; // I2cResultFrom
            System.Threading.Thread.Sleep(50);
            LOG.Debug("Reading Post-config Register Status");
            ITwiMasterSendDataCommand readPostConfigRegisterState = factory.CreateTwiSendData(twiDef, JeeExpanderAddress, false, ReadRegisterIoDir, 11);
            ourImpl.PostMessage(readPostConfigRegisterState);
            ExpectedReceiveCount++; // I2cResultFrom
            System.Threading.Thread.Sleep(50);

            // not really safe to reuse commands because you don't know if they are modified
            ITwiMasterSendDataCommand commandHigh = factory.CreateTwiSendData(twiDef, JeeExpanderAddress, false, WriteAllHigh, 0);
            ITwiMasterSendDataCommand commandLow = factory.CreateTwiSendData(twiDef, JeeExpanderAddress, false, WriteAllLow, 0);
            ITwiMasterSendDataCommand queryOLat = factory.CreateTwiSendData(twiDef, JeeExpanderAddress, false, ReadRegisterOutputLatch, 1);
            for (int i = 0; i < 4; i++) {
                LOG.Debug("Post Low");
                ourImpl.PostMessage(commandLow);
                ExpectedReceiveCount++; // I2cResultFrom
                System.Threading.Thread.Sleep(150);
                LOG.Debug("Check Are Latches Low");
                ourImpl.PostMessage(queryOLat);
                ExpectedReceiveCount++; // I2cResultFrom
                System.Threading.Thread.Sleep(50);
                LOG.Debug("Post High");
                ourImpl.PostMessage(commandHigh);
                ExpectedReceiveCount++; // I2cResultFrom
                System.Threading.Thread.Sleep(150);
                LOG.Debug("Check Are Latches High");
                ourImpl.PostMessage(queryOLat);
                ExpectedReceiveCount++; // I2cResultFrom
                System.Threading.Thread.Sleep(50);
            }

            ITwiMasterCloseCommand closeCommand = factory.CreateTwiClose(twiDef);
            ourImpl.PostMessage(closeCommand);
            ExpectedReceiveCount++; // HandleI2cClose
            System.Threading.Thread.Sleep(100);

            // logging the messages with any other string doesn't show the messages themselves !?
            LOG.Debug("Captured:" + +this.HandlerSingleQueueAllType_.Count()+ " Expected:"+ExpectedReceiveCount);
            LOG.Debug(this.HandlerSingleQueueAllType_.GetEnumerator());
            // should verify close command
            Assert.AreEqual(ExpectedReceiveCount, this.HandlerSingleQueueAllType_.Count(), 
                "This test will fail if you do not have a JeeNodes port expander on Twi "+TwiVirtualDevice);
        }
    }
}
