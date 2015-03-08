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

        private string connectionString;
        private SerialPort port = null;

        public SerialConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public string ConnectionString()
        {
            return connectionString;
        }

        public void waitForConnect()
        {
            port = new SerialPort(connectionString);
            port.ReceivedBytesThreshold = 1;
            port.ReadTimeout = 2000;
            //port.ReadTimeout = SerialPort.InfiniteTimeout;
            // from the IOIO java code
            port.DtrEnable = true;
            port.Open();
            System.Threading.Thread.Sleep(100);
        }

        public void disconnect()
        {
            if (port != null && port.IsOpen)
            {
                port.Close();
                port = null;
            }
            else
            {
                port = null;
            }
        }

        /// <summary>
        /// throws ConnectionLostException if connection closed or was never opened
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream getInputStream()
        {
            if (port != null)
            {
                return port.BaseStream;
            }
            else
            {
                throw new ConnectionLostException("Did you run waitForConnection on " + this.connectionString);
            }
        }

        /// <summary>
        /// throws ConnectionLostException if connection closed or was never opened
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream getOutputStream()
        {
            if (port != null)
            {
                return port.BaseStream;
            }
            else
            {
                throw new ConnectionLostException("Did you run waitForConnection on " + this.connectionString);
            }
        }

        public bool canClose()
        {
            // should we check more state here?
            if (port != null)
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
