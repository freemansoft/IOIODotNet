﻿using IOIOLib.Connection;
using IOIOLib.Connection.Impl;
using IOIOLib.Device;
using IOIOLib.Device.Impl;
using IOIOLib.IOIOException;
using IOIOLib.MessageFrom;
using IOIOLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Convenience
{
    public class FindDeviceHack
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(FindDeviceHack));

        public static string TryAndFindIOIODevice()
        {
            LOG.Debug("Starting TryAndFindIOIODevice");
            IOIOConnectionFactory factory = new SerialConnectionFactory();
            ICollection<IOIOConnection> connections = factory.CreateConnections();
            LOG.Info("Found " + connections.Count + " possible com ports");
            string goodConnectionName = null;

            // probably don't need this since we aren't connected.
            foreach (IOIOConnection oneConn in connections)
            {
                // uses custom setup because we are trying to find IOIO not trying to do work with them
                try
                {
                    LOG.Info("Trying " + oneConn.ConnectionString());
                    try {
                        oneConn.WaitForConnect();
                        // logging without real capture
                        ObserverLogAndCaptureLog handlerLog = new ObserverLogAndCaptureLog(1);
                        // so we can verify
                        ObserverConnectionState handlerState = new ObserverConnectionState();
                        IOIOHandlerObservable observers = new IOIOHandlerObservable();
                        observers.Subscribe(handlerState);
                        observers.Subscribe(handlerLog);
                        IOIOProtocolIncoming foo = new IOIOProtocolIncoming(oneConn.GetInputStream(), observers);
                        System.Threading.Thread.Sleep(50); // WaitForChangedResult for hw ids
                        if (handlerState.EstablishConnectionFrom_ != null)
                        {
                            goodConnectionName = oneConn.ConnectionString();
                            LOG.Info("Selecting " + oneConn.ConnectionString());
                            oneConn.Disconnect();
                            break;
                        }
                        else
                        {
                            LOG.Info("Ignoring " + oneConn.ConnectionString());
                            oneConn.Disconnect();
                        }
                    }
                    catch (System.UnauthorizedAccessException e)
                    {
                        LOG.Info("No Permission " + oneConn.ConnectionString() + e.Message);
                    }
                }
                catch (ConnectionLostException e)
                {
                    LOG.Debug("Cought Exception Lost " + e.Message);
                    // just ignore it because will get this when we Disconnect
                }
            }
            if (goodConnectionName != null)
            {
                LOG.Debug("TryAndFindIOIODevice successfull");
            }
            else
            {
                LOG.Debug("TryAndFindIOIODevice failed");
            }
            return goodConnectionName;
        }

    }
}
