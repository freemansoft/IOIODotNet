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
