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
    public class SerialConnectionFactory : IOIOConnectionFactory
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(SerialConnectionFactory));

        /// <summary>
        /// Returns connection object if we can IsOpen the port.
        /// Otherwise throws ConnectioncreatedException
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IOIOConnection createConnection(string connectionString)
        {
            try
            {
                // use the serial port object directly.
                SerialPort foo = new SerialPort(connectionString);
                foo.Open();
                LOG.Debug(connectionString + " is available port");
                foo.Close();
                foo.Dispose();
                IOIOConnection conn = new SerialConnection(connectionString);
                return conn;
            }
            catch (Exception e)
            {
                LOG.Warn("Unable to connect to " + connectionString);
                throw new ConnectionCreationException("Cannot connect to " + connectionString, e);
            }
        }


        public ICollection<IOIOConnection> createConnections(ICollection<string> connectionStrings)
        {
            List<IOIOConnection> createdConnections = new List<IOIOConnection>();
            foreach (string singleConnectionString in connectionStrings)
            {
                try
                {
                    IOIOConnection oneConnector = this.createConnection(singleConnectionString);
                    createdConnections.Add(oneConnector);
                }
                catch (ConnectionCreationException e)
                {
                    LOG.Debug("Couldn't IsOpen while iterating " + singleConnectionString, e);
                }
            }
            return createdConnections;
        }

        /// <summary>
        /// auto-find ports we can IsOpen
        /// </summary>
        /// <returns></returns>
        public ICollection<IOIOConnection> createConnections()
        {
            string[] portNames = SerialPort.GetPortNames();
            ICollection<string> nameCollection = new List<string>();
            foreach (string name in portNames)
            {
                SerialPort foo = new SerialPort(name);
                try
                {
                    foo.Open();
                    LOG.Debug(name + " is available port");
                    foo.Close();
                    foo.Dispose();
                    nameCollection.Add(name);
                }
                catch (Exception e)
                {
                    LOG.Debug("Couldn't IsOpen while scanning " + name, e);
                }

            }
            return createConnections(nameCollection);
        }

    }
}
