using IOIOLib.Component;
using IOIOLib.Component.Types;
using IOIOLib.Connection;
using IOIOLib.Device.Types;
using IOIOLib.IOIOException;
using IOIOLib.MessageTo;
using IOIOLib.MessageTo.Impl;
using IOIOLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// This class will SOMEDAY act as an object oriented wrapper around the IOIO boards.
    /// </summary>
    class IOIOImpl : IOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOImpl));

        private IOIOConnection Conn;
        /// <summary>
        /// TODO Need to get on this and make state be correct!
        /// </summary>
        private IOIOState State = IOIOState.INIT;
        private IOIOProtocolOutgoing OutProt;
        private IOIOProtocolIncoming InProt;
        private IOIOIncomingHandler InboundHandler;
        private IOIOIncomingHandlerCaptureState InbountStateCapture;
        private IOIOIncomingHandlerCaptureLog InboundCaptureAndLog;


        public IOIOImpl(IOIOConnection conn)
        {
            this.Conn = conn;
        }


        /// <summary>
        /// This method is not yet finished.
        /// </summary>
        public void waitForConnect()
        {
            Conn.waitForConnect();
            InbountStateCapture = new IOIOIncomingHandlerCaptureState();
            InboundCaptureAndLog = new IOIOIncomingHandlerCaptureLog(10);
            InboundHandler = new IOIOIncomingHandlerDistributor(
                new List<IOIOIncomingHandler> { InbountStateCapture, InboundCaptureAndLog });

            OutProt = new IOIOProtocolOutgoing(this.Conn.getOutputStream());
            InProt = new IOIOProtocolIncoming(this.Conn.getInputStream(), this.InboundHandler);
            //Joe's COM4 @ 115200 spits out HW,BootLoader,InterfaceVersion: IOIOSPRK0016IOIO0311IOIO0500
            initBoardVersion();
            //checkInterfaceVersion();
        }

        /// <summary>
        /// Verify we are connected and set our state accordingly
        /// </summary>
        private void initBoardVersion()
        {
            // hack until we figure out where state should be and how we accesses
            // Should this build the hardware object and retain it instead of doing it in the handler?
            // the inbound handler actually has already processed the board version.  
            if (InbountStateCapture.EstablishConnectionFrom_ == null)
            {
                State = IOIOState.DEAD;
            }
            else
            {
                State = IOIOState.CONNECTED;
            }
            LOG.Info("Hardware is " + InbountStateCapture.EstablishConnectionFrom_);
        }

        /// <summary>
        /// This method is not yet finished.
        /// This is really an outbound call to the IOIO.  
        /// It will tell us someitme later if it is the right version
        /// </summary>
        private void checkInterfaceVersion()
        {
            ICheckInterfaceVersionTo CheckInterfaceVersionTo_ = new CheckInterfaceVersionTo(IOIORequiredInterfaceId.REQUIRED_INTERFACE_ID);
            this.postMessage(CheckInterfaceVersionTo_);
            LOG.Warn("checkInterfaceVersion should poll for the response");
            //State = IOIOState.INCOMPATIBLE;
        }

        public void disconnect()
        {
            throw new NotImplementedException();
        }

        public void waitForDisconnect()
        {
            throw new NotImplementedException();
        }

        public IOIOState getState()
        {
            return State;
        }

        public void softReset()
        {
            throw new NotImplementedException();
        }

        public void hardReset()
        {
            throw new NotImplementedException();
        }

        public void postMessage(IPostMessageTo message)
        {
            message.ExecuteMessage(this.OutProt);
        }


        public void beginBatch()
        {
            throw new NotImplementedException();
        }

        public void endBatch()
        {
            throw new NotImplementedException();
        }

        public void sync()
        {
            throw new NotImplementedException();
        }

    }
}
