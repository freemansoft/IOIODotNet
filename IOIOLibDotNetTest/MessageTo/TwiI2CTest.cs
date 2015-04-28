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

namespace IOIOLibDotNetTest.MessageTo
{
	[TestClass]
	public class TwiI2CTest : BaseTest
	{
		private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(TwiI2CTest));
        //from sparkfun L3G4200D.h also available in the docs
        // WHO_AM_I 0x0F
        // CTRL_REG1 0x20
        // CTRL_REG2 0x21
        // CTRL_REG3 0x22
        // CTRL_REG4 0x23
        // CTRL_REG5 0x24
        // REFERENCE 0x25
        // OUT_TEMP 0x26
        // STATUS_REG 0x27
        // OUT_X_L 0x28
        // OUT_X_H 0x29
        // OUT_Y_L 0x2A
        // OUT_Y_H 0x2B
        // OUT_Z_L 0x2C
        // OUT_Z_H 0x2D
        // FIFO_CTRL_REG 0x2E
        // FIFO_SRC_REG 0x2F
        // INT1_CFG 0x30
        // INT1_SRC 0x31
        // INT1_TSH_XH 0x32
        // INT1_TSH_XL 0x33
        // INT1_TSH_YH 0x34
        // INT1_TSH_YL 0x35
        // INT1_TSH_ZH 0x36
        // INT1_TSH_ZL 0x37
        // INT1_DURATION 0x38

        /// <summary>
        /// Parallax Gyroscope  L3G4200D found at Radio Shack in 2014
        /// This is a Gyroscope not an Accelerometer https://www.sparkfun.com/pages/accel_gyro_guide
        /// You must use pullups on the IOIO I2C pins for this to work. I used 10K Ohm
        /// Portions of this adapted from Sparkfun L3G4200D_Example.pde
        /// You can find a tutorial on gyroscopes at http://morf.lv/modules.php?name=tutorials&lasit=32
        /// </summary>
        [TestMethod]
        public void TwiI2CTest_L3G4200D_Integration()
		{


            IOIOConnection ourConn = this.CreateGoodSerialConnection(false);
            // create our custom I2C result observer and add it to the default set
            ObserverI2cResultTest observer = new ObserverI2cResultTest();
            this.CreateCaptureLogHandlerSet();
            // we'll inject our observers on top of the default handlers so we don't have to peek into impl
            IOIO ourImpl = CreateIOIOImplAndConnect(ourConn, new List<IObserverIOIO>() {
                    this.CapturedConnectionState_, this.CapturedSingleQueueAllType_, this.CapturedLogs_,
                observer});
            LOG.Debug("Setup Complete");
            System.Threading.Thread.Sleep(100);  // wait for us to get the hardware ids

            ourImpl.SoftReset();
            System.Threading.Thread.Sleep(100);

            IOIOMessageCommandFactory factory = new IOIOMessageCommandFactory();
            ITwiMasterConfigureCommand startCommand = factory.CreateTwiConfigure(0, TwiMasterRate.RATE_400KHz, false);
            ourImpl.PostMessage(startCommand);
            System.Threading.Thread.Sleep(50);
            TwiSpec twiDef = startCommand.TwiDef;

            LOG.Debug("Ask for Who Am I");
            // send the whoami command - we expect the id to be Gyro_WhoAmI_ID
            byte[] ReadWhoAmiRegisterData = new byte[] { L3G4200DConstants.Gyro_WhoAmI_Register };
            ITwiMasterSendDataCommand startupCommand1 = factory.CreateTwiSendData(twiDef, L3G4200DConstants.GyroSlaveAddress1, false, ReadWhoAmiRegisterData, 1);
            ourImpl.PostMessage(startupCommand1);
            System.Threading.Thread.Sleep(50);
            // should check for Gyro_WhoAmI_ID_L3G4200D !

            // Enable x, y, z and turn off power down
            // auto increment registers
            byte ControlRegisterAutoIncrement = L3G4200DConstants.Gyro_CTRL_REG1 |= Convert.ToByte(0x80);
            byte[] RegisterConfigurationData = new byte[] { ControlRegisterAutoIncrement,
                Convert.ToByte("00001111", 2),
                Convert.ToByte("00000000", 2),
                Convert.ToByte("00000000", 2),
                L3G4200DConstants.Gyro_Range_DPS_2000,
                //Enable High pass filter
                Convert.ToByte("00000000", 2)
            };
            LOG.Debug("Updating Registers starting with " + L3G4200DConstants.Gyro_CTRL_REG1.ToString("X")+ ":"+ RegisterConfigurationData);
            ITwiMasterSendDataCommand ConfigureRegisters = factory.CreateTwiSendData(twiDef, L3G4200DConstants.GyroSlaveAddress1, false, RegisterConfigurationData, 0);
            ourImpl.PostMessage(ConfigureRegisters);

            LOG.Debug("Reading Registers starting with " + L3G4200DConstants.Gyro_CTRL_REG1.ToString("X"));
            byte[] ReadRegisterControl = new byte[] { ControlRegisterAutoIncrement };
            ITwiMasterSendDataCommand ReadRegistersCommand = factory.CreateTwiSendData(twiDef, L3G4200DConstants.GyroSlaveAddress1, false, ReadRegisterControl, 5);
            ourImpl.PostMessage(ReadRegistersCommand);
            System.Threading.Thread.Sleep(50);

            // clear the list
            observer.allEvents = new ConcurrentQueue<II2cResultFrom>();

            // Read back the current values -- could wait for int to go high but can't see it....
            // Top most bit in address turns on auto inc.  That is weirder than usual i2c
            // Who thought that there should be no bitwise byte operators but then came up with |= ?
            // never get more than allowableOutstanding behind
            // on my machine it takes 15msec to receive a message after sending
            int allowableOutstanding = 10;
            int numReps = 50;
            int count = 0;
            int maxWaitCount = 50;
            for (int i = 1; i <= numReps; i++)
            {
                LOG.Debug("Send read-only command retreive xyz with auto increment sendCount: " +i);
                observer.LastResult_ = null;
                byte[] ReadFromFirstOutRegisterWithAutoInc = new byte[] { L3G4200DConstants.Gyro_First_Out_Register |= Convert.ToByte(0x80) };
                ITwiMasterSendDataCommand ReadXYZ = factory.CreateTwiSendData(twiDef, L3G4200DConstants.GyroSlaveAddress1, false, ReadFromFirstOutRegisterWithAutoInc, 6);
                ourImpl.PostMessage(ReadXYZ);
                count = 0;
                while (count <= maxWaitCount && i > observer.allEvents.Count + allowableOutstanding)
                {
                    // waiting for some reply
                    LOG.Debug("waiting: sendCount:" + i + ", observCount:" + observer.allEvents.Count);
                    System.Threading.Thread.Sleep(5);
                    count++;
                }
                if (count >= maxWaitCount)
                {
                    Assert.Fail("waitedCount:" + count + " while trying to send");
                }
            }
            // this is only needed if maxWaitCount > 0
            count = 0;
            while (count < maxWaitCount && numReps > observer.allEvents.Count)
            {
                LOG.Debug("i:" + numReps + ",count:" + observer.allEvents.Count);
                System.Threading.Thread.Sleep(10);
                count++;
            }
            if (count >= maxWaitCount)
            {
                Assert.Fail("waited " + count + " while trying to receive");
            }

            System.Threading.Thread.Sleep(50);
            LOG.Debug("Close Gyroscope");
            ITwiMasterCloseCommand closeCommand = factory.CreateTwiClose(twiDef);
            ourImpl.PostMessage(closeCommand);
            System.Threading.Thread.Sleep(100);

            // logging the messages with any other string doesn't show the messages themselves !?
            LOG.Debug("Captured (all):" + this.CapturedSingleQueueAllType_.Count());
            // expect 6 + number of reports we requested
            LOG.Debug("Captured (i2c):" + observer.allEvents.Count);
			LOG.Debug(this.CapturedSingleQueueAllType_.GetEnumerator());
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
            IOIO ourImpl = CreateIOIOImplAndConnect(ourConn,
                new List<IObserverIOIO>() { this.CapturedConnectionState_, this.CapturedSingleQueueAllType_, this.CapturedLogs_ });
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
            for (int i = 0; i < 2; i++) {
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
            LOG.Debug("Captured:" + +this.CapturedSingleQueueAllType_.Count()+ " Expected:"+ExpectedReceiveCount);
            LOG.Debug(this.CapturedSingleQueueAllType_.GetEnumerator());
            // should verify close command!
            // should verify results of the latch checks.!
            // instead do this lame test!
            Assert.AreEqual(ExpectedReceiveCount, this.CapturedSingleQueueAllType_.Count(), 
                "This test will fail if you do not have a JeeNodes port expander at I2C address "+JeeExpanderAddress
                +" on Twi "+TwiVirtualDevice);
        }
    }

    public class ObserverI2cResultTest : IObserver<II2cResultFrom>, IObserverIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(ObserverI2cResultTest));
        internal ConcurrentQueue<II2cResultFrom> allEvents = new ConcurrentQueue<II2cResultFrom>();
        internal II2cResultFrom LastResult_;

        public void OnCompleted()
        {
            // do nothing
        }

        public void OnError(Exception error)
        {
            // do nothing
        }

        public void OnNext(II2cResultFrom value)
        {
            LastResult_ = value;
            allEvents.Enqueue(value);
            if (value.NumDataBytes > 0) { 
            LOG.Debug("Received:" + value + " xyz: " + this.ToXYZ(value)); ;
            } else
            {
                LOG.Debug("Received:" + value); ;
            }
        }

        public Tuple<int,int,int> ToXYZ(II2cResultFrom value)
        {
            if (value == null || value.Data == null || value.NumDataBytes != 6)
            {
                return null;
            }
            else
            {
                int x = (value.Data[1] << 8) + value.Data[0];
                int y = (value.Data[3] << 8) + value.Data[2];
                int z = (value.Data[5] << 8) + value.Data[4];
                Tuple<int, int, int> result = new Tuple<int, int, int>(x,y,z);
                //LOG.Debug("Values:" + result);
                return result;
            }
        }
    }
}
