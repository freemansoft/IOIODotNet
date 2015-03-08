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

using IOIOLib.Device.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOIOLib.Component;
using IOIOLib.Component.Types;
using IOIOLib.Connection;
using IOIOLib.Util;
using System.Threading;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// Handles outgoing communication to a single device (connection).  
    /// It does not have its own thread.  
    /// Windows COM driver, and probably TCP, calls are non-blocking
    /// </summary>
    public class IOIOProtocolOutgoing
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOProtocolOutgoing));

        private int BatchCounter_ = 0;
        private Stream Stream_;

        public IOIOProtocolOutgoing(Stream stream)
        {
            this.Stream_ = stream;
        }

        private void writeByte(int b)
        {
            // wish this guy had a write() blog post
            // http://www.sparxeng.com/blog/software/must-use-net-system-io-ports-serialport
            System.Diagnostics.Debug.Assert(b >= 0 && b < 256);
            LOG.Debug("sending: 0x" + b.ToString("X"));
            Stream_.WriteByte((byte)b);
        }

        private void writeBytes(byte[] buf, int offset, int size)
        {
            while (size-- > 0)
            {
                writeByte(((int)buf[offset++]) & 0xFF);
            }
        }

        public void beginBatch()
        {
            ++BatchCounter_;
        }

        public void endBatch()
        {
            if (--BatchCounter_ == 0)
            {
                Stream_.Flush();
            }
        }

        private void writeTwoBytes(int i)
        {
            writeByte(i & 0xFF);
            writeByte(i >> 8);
        }

        private void writeThreeBytes(int i)
        {
            writeByte(i & 0xFF);
            writeByte((i >> 8) & 0xFF);
            writeByte((i >> 16) & 0xFF);
        }

        public void sync()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SYNC);
            endBatch();
        }

        public void hardReset()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.HARD_RESET);
            writeByte('I');
            writeByte('O');
            writeByte('I');
            writeByte('O');
            endBatch();
        }

        public void softReset()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SOFT_RESET);
            endBatch();
        }

        public void softClose()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SOFT_CLOSE);
            endBatch();
        }

        public void checkInterfaceVersion(byte[] interfaceId)
        {
            if (interfaceId.Length != 8)
            {
                throw new ArgumentException("interface ID must be exactly 8 bytes long");
            }
            LOG.Debug("Sending CHECK_INTERFACE");
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.CHECK_INTERFACE);
            for (int i = 0; i < 8; ++i)
            {
                writeByte(interfaceId[i]);
            }
            endBatch();
        }

        public void checkInterfaceVersion()
        {
            this.checkInterfaceVersion(IOIORequiredInterfaceId.REQUIRED_INTERFACE_ID);
        }

        public void setDigitalOutLevel(int pin, bool level)
        {
            LOG.Debug("Sending SET_DIGITAL_OUT_LEVEL");
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_DIGITAL_OUT_LEVEL);
            writeByte(pin << 2 | (level ? 1 : 0));
            endBatch();
        }

        public void setPinPwm(int pin, int pwmNum, bool enable)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_PIN_PWM);
            writeByte(pin & 0x3F);
            writeByte((enable ? 0x80 : 0x00) | (pwmNum & 0x0F));
            endBatch();
        }

        public void setPwmDutyCycle(int pwmNum, int dutyCycle, int fraction)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_PWM_DUTY_CYCLE);
            writeByte(pwmNum << 2 | fraction);
            writeTwoBytes(dutyCycle);
            endBatch();
        }

        public void setPwmPeriod(int pwmNum, int period, PwmScale scale)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_PWM_PERIOD);
            writeByte(((scale.encoding & 0x02) << 6) | (pwmNum << 1) | (scale.encoding & 0x01));
            writeTwoBytes(period);
            endBatch();
        }

        public void setPinIncap(int pin, int incapNum, bool enable)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_PIN_INCAP);
            writeByte(pin);
            writeByte(incapNum | (enable ? 0x80 : 0x00));
            endBatch();
        }

        public void incapClose(int incapNum, bool double_prec)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.INCAP_CONFIGURE);
            writeByte(incapNum);
            writeByte(double_prec ? 0x80 : 0x00);
            endBatch();
        }

        public void incapConfigure(int incapNum, bool double_prec, int mode, int clock)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.INCAP_CONFIGURE);
            writeByte(incapNum);
            writeByte((double_prec ? 0x80 : 0x00) | (mode << 3) | clock);
            endBatch();
        }

        public void i2cWriteRead(int i2cNum, bool tenBitAddr, int address,
                int writeSize, int readSize, byte[] writeData)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.I2C_WRITE_READ);
            writeByte(((address >> 8) << 6) | (tenBitAddr ? 0x20 : 0x00) | i2cNum);
            writeByte(address & 0xFF);
            writeByte(writeSize);
            writeByte(readSize);
            for (int i = 0; i < writeSize; ++i)
            {
                writeByte(((int)writeData[i]) & 0xFF);
            }
            endBatch();
        }

        public void setPinDigitalOut(int pin, bool value, DigitalOutputSpecMode mode)
        {
            LOG.Debug("Sending SET_PIN_DIGITAL_OUT");
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_PIN_DIGITAL_OUT);
            writeByte((pin << 2) | (mode == DigitalOutputSpecMode.OPEN_DRAIN ? 0x01 : 0x00)
                    | (value ? 0x02 : 0x00));
            endBatch();
        }

        public void setPinDigitalIn(int pin, DigitalInputSpecMode mode)
        {
            LOG.Debug("Sending SET_PIN_DIGITAL_IN");
            int pull = 0;
            if (mode == DigitalInputSpecMode.PULL_UP)
            {
                pull = 1;
            }
            else if (mode == DigitalInputSpecMode.PULL_DOWN)
            {
                pull = 2;
            }
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_PIN_DIGITAL_IN);
            writeByte((pin << 2) | pull);
            endBatch();
        }

        public void setChangeNotify(int pin, bool changeNotify)
        {
            LOG.Debug("Sending SET_CHANGE_NOTIFY");
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_CHANGE_NOTIFY);
            writeByte((pin << 2) | (changeNotify ? 0x01 : 0x00));
            endBatch();
        }

        public void registerPeriodicDigitalSampling(int pin, int freqScale)
        {
            // TODO: implement
        }

        public void setPinAnalogIn(int pin)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_PIN_ANALOG_IN);
            writeByte(pin);
            endBatch();
        }

        public void setAnalogInSampling(int pin, bool enable)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_ANALOG_IN_SAMPLING);
            writeByte((enable ? 0x80 : 0x00) | (pin & 0x3F));
            endBatch();
        }

        public void uartData(int uartNum, int numBytes, byte[] data)
        {
            if (numBytes > 64)
            {
                throw new ArgumentException(
                        "A maximum of 64 bytes can be sent in one uartData message. Got: " + numBytes);
            }
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.UART_DATA);
            writeByte((numBytes - 1) | uartNum << 6);
            for (int i = 0; i < numBytes; ++i)
            {
                writeByte(((int)data[i]) & 0xFF);
            }
            endBatch();
        }

        public void uartConfigure(int uartNum, int rate, bool speed4x,
                UartStopBits stopbits, UartParity parity)
        {
            int parbits = parity == UartParity.EVEN ? 1 : (parity == UartParity.ODD ? 2 : 0);
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.UART_CONFIG);
            writeByte((uartNum << 6) | (speed4x ? 0x08 : 0x00)
                    | (stopbits == UartStopBits.TWO ? 0x04 : 0x00) | parbits);
            writeTwoBytes(rate);
            endBatch();
        }

        public void uartClose(int uartNum)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.UART_CONFIG);
            writeByte(uartNum << 6);
            writeTwoBytes(0);
            endBatch();
        }

        public void setPinUart(int pin, int uartNum, bool tx, bool enable)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_PIN_UART);
            writeByte(pin);
            writeByte((enable ? 0x80 : 0x00) | (tx ? 0x40 : 0x00) | uartNum);
            endBatch();
        }

        public void spiConfigureMaster(int spiNum, SpiMasterConfig config)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SPI_CONFIGURE_MASTER);
            writeByte((spiNum << 5) | ScaleDivider.ScaleDiv[(int)config.rate]); // cast enum is horrible C# feature
            writeByte((config.sampleOnTrailing ? 0x00 : 0x02) | (config.invertClk ? 0x01 : 0x00));
            endBatch();
        }

        public void spiClose(int spiNum)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SPI_CONFIGURE_MASTER);
            writeByte(spiNum << 5);
            writeByte(0x00);
            endBatch();
        }

        public void setPinSpi(int pin, int mode, bool enable, int spiNum)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_PIN_SPI);
            writeByte(pin);
            writeByte((1 << 4) | (mode << 2) | spiNum);
            endBatch();
        }

        public void spiMasterRequest(int spiNum, int ssPin, byte[] data, int dataBytes,
                int totalBytes, int responseBytes)
        {
            bool dataNeqTotal = (dataBytes != totalBytes);
            bool resNeqTotal = (responseBytes != totalBytes);
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SPI_MASTER_REQUEST);
            writeByte((spiNum << 6) | ssPin);
            writeByte((dataNeqTotal ? 0x80 : 0x00) | (resNeqTotal ? 0x40 : 0x00) | totalBytes - 1);
            if (dataNeqTotal)
            {
                writeByte(dataBytes);
            }
            if (resNeqTotal)
            {
                writeByte(responseBytes);
            }
            for (int i = 0; i < dataBytes; ++i)
            {
                writeByte(((int)data[i]) & 0xFF);
            }
            endBatch();
        }

        public void i2cConfigureMaster(int i2cNum, TwiMasterRate rate, bool smbusLevels)
        {
            int rateBits = (rate == TwiMasterRate.RATE_1MHz ? 3 : (rate == TwiMasterRate.RATE_400KHz ? 2 : 1));
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.I2C_CONFIGURE_MASTER);
            writeByte((smbusLevels ? 0x80 : 0) | (rateBits << 5) | i2cNum);
            endBatch();
        }

        public void i2cClose(int i2cNum)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.I2C_CONFIGURE_MASTER);
            writeByte(i2cNum);
            endBatch();
        }

        public void icspOpen()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.ICSP_CONFIG);
            writeByte(0x01);
            endBatch();
        }

        public void icspClose()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.ICSP_CONFIG);
            writeByte(0x00);
            endBatch();
        }

        public void icspEnter()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.ICSP_PROG_ENTER);
            endBatch();
        }

        public void icspExit()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.ICSP_PROG_EXIT);
            endBatch();
        }

        public void icspSix(int instruction)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.ICSP_SIX);
            writeThreeBytes(instruction);
            endBatch();
        }

        public void icspRegout()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.ICSP_REGOUT);
            endBatch();
        }

        public void setPinCapSense(int pinNum)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_PIN_CAPSENSE);
            writeByte(pinNum & 0x3F);
            endBatch();
        }

        public void setCapSenseSampling(int pinNum, bool enable)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SET_CAPSENSE_SAMPLING);
            writeByte((pinNum & 0x3F) | (enable ? 0x80 : 0x00));
            endBatch();
        }

        public void sequencerOpen(byte[] config, int size)
        {
            System.Diagnostics.Debug.Assert(config != null);
            System.Diagnostics.Debug.Assert(size >= 0 && size <= 68);

            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SEQUENCER_CONFIGURE);
            writeByte(size);
            writeBytes(config, 0, size);
            endBatch();
        }

        public void sequencerClose()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SEQUENCER_CONFIGURE);
            writeByte(0);
            endBatch();
        }

        public void sequencerPush(int duration, byte[] cue, int size)
        {
            System.Diagnostics.Debug.Assert(cue != null);
            System.Diagnostics.Debug.Assert(size >= 0 && size <= 68);
            System.Diagnostics.Debug.Assert(duration < (1 << 16));

            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SEQUENCER_PUSH);
            writeTwoBytes(duration);
            writeBytes(cue, 0, size);
            endBatch();
        }

        public void sequencerStop()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SEQUENCER_CONTROL);
            writeByte(0);
            endBatch();
        }

        public void sequencerStart()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SEQUENCER_CONTROL);
            writeByte(1);
            endBatch();
        }

        public void sequencerPause()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SEQUENCER_CONTROL);
            writeByte(2);
            endBatch();
        }

        public void sequencerManualStart(byte[] cue, int size)
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SEQUENCER_CONTROL);
            writeByte(3);
            writeBytes(cue, 0, size);
            endBatch();
        }

        public void sequencerManualStop()
        {
            beginBatch();
            writeByte((byte)IOIOProtocolCommands.SEQUENCER_CONTROL);
            writeByte(4);
            endBatch();
        }

    }
}
