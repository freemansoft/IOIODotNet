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
 
using IOIOLib.IOIOException;
using IOIOLib.Util;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Connection.Impl
{
    class SerialConnection : IOIOConnection
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(SerialConnection));

        private string ConnectionString_;
        private SerialPort Port_ = null;

        public SerialConnection(string connectionString)
        {
            this.ConnectionString_ = connectionString;
        }

        public string ConnectionString()
        {
            return ConnectionString_;
        }

        public void WaitForConnect()
        {
            Port_ = new SerialPort(ConnectionString_);
            Port_.ReceivedBytesThreshold = 1;
            Port_.ReadTimeout = 2000;
            //Port_.ReadTimeout = SerialPort.InfiniteTimeout;
            // from the IOIO java code
            Port_.DtrEnable = true;
            Port_.Open();
            System.Threading.Thread.Sleep(100);
        }

        public void Disconnect()
        {
            if (Port_ != null && Port_.IsOpen)
            {
                Port_.Close();
                Port_ = null;
            }
            else
            {
                Port_ = null;
            }
        }

        /// <summary>
        /// throws ConnectionLostException if connection closed or was never opened
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream GetInputStream()
        {
            if (Port_ != null)
            {
                return Port_.BaseStream;
            }
            else
            {
                throw new ConnectionLostException("Did you run waitForConnection on " + this.ConnectionString_);
            }
        }

        /// <summary>
        /// throws ConnectionLostException if connection closed or was never opened
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream GetOutputStream()
        {
            if (Port_ != null)
            {
                return Port_.BaseStream;
            }
            else
            {
                throw new ConnectionLostException("Did you run waitForConnection on " + this.ConnectionString_);
            }
        }

        public bool CanClose()
        {
            // should we check more state here?
            if (Port_ != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
