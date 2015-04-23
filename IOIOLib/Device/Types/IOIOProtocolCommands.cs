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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    internal enum IOIOProtocolCommands
    {
        HARD_RESET = 0x00,
        ESTABLISH_CONNECTION = 0x00,
        SOFT_RESET = 0x01,
        CHECK_INTERFACE = 0x02,
        CHECK_INTERFACE_RESPONSE = 0x02,
        SET_PIN_DIGITAL_OUT = 0x03,
        SET_DIGITAL_OUT_LEVEL = 0x04,
        REPORT_DIGITAL_IN_STATUS = 0x04,
        SET_PIN_DIGITAL_IN = 0x05,
        REPORT_PERIODIC_DIGITAL_IN_STATUS = 0x05,
        SET_CHANGE_NOTIFY = 0x06,
        REGISTER_PERIODIC_DIGITAL_SAMPLING = 0x07,
        SET_PIN_PWM = 0x08,
        SET_PWM_DUTY_CYCLE = 0x09,
        SET_PWM_PERIOD = 0x0A,
        SET_PIN_ANALOG_IN = 0x0B,
        REPORT_ANALOG_IN_STATUS = 0x0B,
        SET_ANALOG_IN_SAMPLING = 0x0C,
        REPORT_ANALOG_IN_FORMAT = 0x0C,
        UART_CONFIG = 0x0D, // outbound open / close status
        UART_STATUS = 0x0D, // inbound open / close status
        UART_DATA = 0x0E,
        SET_PIN_UART = 0x0F,
        UART_REPORT_TX_STATUS = 0x0F,
        SPI_CONFIGURE_MASTER = 0x10,
        SPI_STATUS = 0x10,
        SPI_MASTER_REQUEST = 0x11,
        SPI_DATA = 0x11,
        SET_PIN_SPI = 0x12,
        SPI_REPORT_TX_STATUS = 0x12,
        I2C_CONFIGURE_MASTER = 0x13,
        I2C_STATUS = 0x13,
        I2C_WRITE_READ = 0x14,
        I2C_RESULT = 0x14,
        I2C_REPORT_TX_STATUS = 0x15,
        ICSP_SIX = 0x16,
        ICSP_REPORT_RX_STATUS = 0x16,
        ICSP_REGOUT = 0x17,
        ICSP_RESULT = 0x17,
        ICSP_PROG_ENTER = 0x18,
        ICSP_PROG_EXIT = 0x19,
        ICSP_CONFIG = 0x1A,
        INCAP_CONFIGURE = 0x1B,
        INCAP_STATUS = 0x1B,
        SET_PIN_INCAP = 0x1C,
        INCAP_REPORT = 0x1C,
        SOFT_CLOSE = 0x1D,
        SET_PIN_CAPSENSE = 0x1E,
        CAPSENSE_REPORT = 0x1E,
        SET_CAPSENSE_SAMPLING = 0x1F,
        SEQUENCER_CONFIGURE = 0x20,
        SEQUENCER_EVENT = 0x20,
        SEQUENCER_PUSH = 0x21,
        SEQUENCER_CONTROL = 0x22,
        SYNC = 0x23
    }

}
