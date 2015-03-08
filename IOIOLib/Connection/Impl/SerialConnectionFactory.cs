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
    public class SerialConnectionFactory : IOIOConnectionFactory
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(SerialConnectionFactory));

        /// <summary>
        /// Returns connection object if we can IsOpen the Port_.
        /// Otherwise throws ConnectioncreatedException
        /// </summary>
        /// <param name="ConnectionString_"></param>
        /// <returns></returns>
        public IOIOConnection CreateConnection(string connectionString)
        {
            try
            {
                // use the serial Port_ object directly.
                SerialPort foo = new SerialPort(connectionString);
                foo.Open();
                LOG.Debug(connectionString + " is available Port_");
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


        public ICollection<IOIOConnection> CreateConnections(ICollection<string> connectionStrings)
        {
            List<IOIOConnection> createdConnections = new List<IOIOConnection>();
            foreach (string singleConnectionString in connectionStrings)
            {
                try
                {
                    IOIOConnection oneConnector = this.CreateConnection(singleConnectionString);
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
        public ICollection<IOIOConnection> CreateConnections()
        {
            string[] portNames = SerialPort.GetPortNames();
            ICollection<string> nameCollection = new List<string>();
            foreach (string name in portNames)
            {
                SerialPort foo = new SerialPort(name);
                try
                {
                    foo.Open();
                    LOG.Debug(name + " is available Port_");
                    foo.Close();
                    foo.Dispose();
                    nameCollection.Add(name);
                }
                catch (Exception e)
                {
                    LOG.Debug("Couldn't IsOpen while scanning " + name, e);
                }

            }
            return CreateConnections(nameCollection);
        }

    }
}
