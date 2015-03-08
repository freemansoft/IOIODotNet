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

using IOIOLib.Component;
using IOIOLib.Component.Types;
using IOIOLib.Connection;
using IOIOLib.Device.Types;
using IOIOLib.IOIOException;
using IOIOLib.MessageTo;
using IOIOLib.MessageTo.Impl;
using IOIOLib.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// This class will SOMEDAY act as an object oriented wrapper around the IOIO boards.
    /// This should handle the clean up of the incoming and outgoing protocols
    /// </summary>
    class IOIOImpl : IOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOImpl));

        /// <summary>
        /// communication channel
        /// </summary>
        private IOIOConnection Conn;
        /// <summary>
        /// TODO Need to get on this and make state be correct!
        /// </summary>
        private IOIOState State = IOIOState.INIT;
        private IOIOProtocolOutgoing OutProt;
        private IOIOProtocolIncoming InProt;
        private IOIOIncomingHandler InboundHandler;
        private IOIOHandlerCaptureConnectionState CapturedConnectionInformation;
        private IOIOHandlerCaptureLog CapturedLogs;

        /// <summary>
        /// Used to stop this protocol thread
        /// </summary>
        internal CancellationTokenSource CancelTokenSource_;
        /// <summary>
        /// our outbound thread
        /// </summary>
        private Task OutgoingTask_;


        /// <summary>
        /// Should use ConcurrentCollection under the hood
        /// </summary>
        private BlockingCollection<IPostMessageTo> WorkQueue = new BlockingCollection<IPostMessageTo>();



        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="customHandler">your handler.  This always adds:
        ///     IOIOHandlerCaptureConnectionState , IOIOHandlerCaptureLog</param>
        public IOIOImpl(IOIOConnection conn, IOIOIncomingHandler customHandler)
        {
            if (conn == null)
            {
                throw new IllegalStateException("Silly Rabbit: You can't create an IOIOImpl without a connection!");
            }
            this.Conn = conn;
            ConfigureHandlers(customHandler);
        }

        /// <summary>
        /// Wrap the custom handler with our instrumentation handlers
        /// </summary>
        /// <param name="customHandler">optional handler provided by object creator</param>
        private void ConfigureHandlers(IOIOIncomingHandler customHandler)
        {
            CapturedConnectionInformation = new IOIOHandlerCaptureConnectionState();
            CapturedLogs = new IOIOHandlerCaptureLog(10);
            if (customHandler != null)
            {
                InboundHandler = new IOIOHandlerDistributor(
                    new List<IOIOIncomingHandler> { CapturedConnectionInformation, CapturedLogs, customHandler });
            }
            else
            {
                InboundHandler = new IOIOHandlerDistributor(
                    new List<IOIOIncomingHandler> { CapturedConnectionInformation, CapturedLogs });
            }
        }


        /// <summary>
        /// This method is not yet finished.
        /// </summary>
        public void waitForConnect()
        {
            Conn.waitForConnect();

            // Use the same cancel token for inbound and outbound
            CancelTokenSource_ = new CancellationTokenSource();
            OutProt = new IOIOProtocolOutgoing(this.Conn.getOutputStream());
            InProt = new IOIOProtocolIncoming(this.Conn.getInputStream(), this.InboundHandler, CancelTokenSource_);
            //Joe's COM4 @ 115200 spits out HW,BootLoader,InterfaceVersion: IOIOSPRK0016IOIO0311IOIO0500
            initBoardVersion();
            //checkInterfaceVersion();
            // start the message pump
            OutgoingTask_ = new Task(run, CancelTokenSource_.Token, TaskCreationOptions.LongRunning);
            OutgoingTask_.Start();
        }

        /// <summary>
        /// Verify we are connected and set our state accordingly
        /// </summary>
        private void initBoardVersion()
        {
            // hack until we figure out where state should be and how we accesses
            // Should this build the hardware object and retain it instead of doing it in the handler?
            // the inbound handler actually has already processed the board version.  
            if (CapturedConnectionInformation.EstablishConnectionFrom_ == null)
            {
                State = IOIOState.DEAD;
            }
            else
            {
                State = IOIOState.CONNECTED;
            }
            LOG.Info("Hardware is " + CapturedConnectionInformation.EstablishConnectionFrom_);
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
            CancelTokenSource_.Cancel();
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


        public void postMessage(IPostMessageTo message)
        {
            WorkQueue.Add(message);
        }



        //////////////////////////////////////////////////////////
        //// Outbound Thread Handling section
        //////////////////////////////////////////////////////////


        public void run()
        {
            try
            {
                // pick something fast for humans but long for a computer
                TimeSpan timeout = new TimeSpan(0, 0, 0, 0, 100);
                while (true)
                {
                    this.CancelTokenSource_.Token.ThrowIfCancellationRequested();
                    IPostMessageTo result;
                    // use timeout so we can get cancellation token
                    // use blocking queue so that we aren't spinning
                    bool didTake = WorkQueue.TryTake(out result, timeout);
                    if (didTake && result != null)
                    {
                        result.ExecuteMessage(this.OutProt);
                    }
                }
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LOG.Error(OutgoingTask_.Id + " Probably aborted thread (TAE): " + e.Message);
            }
            catch (ObjectDisposedException e)
            {
                //// see this when steram is closed
                LOG.Error(OutgoingTask_.Id + " Probably closed outgoing Stream: (ODE)" + e.Message);
            }
            catch (Exception e)
            {
                LOG.Error(OutgoingTask_.Id + " Probably stopping outgoing: (E)" + e.Message);
            }
            finally
            {
                // we don't play swith Stream since we didn't create it
                LOG.Debug(OutgoingTask_.Id + " Throwing thread cancel to mae sure outgoing thread stopped");
                // this is redundant if we got here because of thread stop
                this.CancelTokenSource_.Cancel();
                // debugger will always stop here in unit tests if test dynamically determines what port ot use
                // just hit continue in the debugger
                this.CancelTokenSource_.Token.ThrowIfCancellationRequested();
                // should tell OutProt to shut down
                this.OutProt = null;
                this.OutgoingTask_ = null;
            }

        }

    }
}
