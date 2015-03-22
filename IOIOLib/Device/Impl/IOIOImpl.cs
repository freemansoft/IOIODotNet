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
        private IOIOConnection Conn_;
        /// <summary>
        /// TODO Need to get on this and make state be correct!
        /// </summary>
        private IOIOState State_ = IOIOState.INIT;
        private IOIOProtocolOutgoing OutProt_;
        private IOIOProtocolIncoming InProt_;
        private IOIOIncomingHandler InboundHandler_;
        private IOIOHandlerCaptureConnectionState CapturedConnectionInformation_;
        private IOIOHandlerCaptureLog CapturedLogs_;
		private IResourceManager BoardResourceManager_;

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
        private BlockingCollection<IPostMessageCommand> WorkQueue = new BlockingCollection<IPostMessageCommand>();



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
            this.Conn_ = conn;
            ConfigureHandlers(customHandler);
        }

        /// <summary>
        /// Wrap the custom handler with our instrumentation handlers
        /// </summary>
        /// <param name="customHandler">optional handler provided by object creator</param>
        private void ConfigureHandlers(IOIOIncomingHandler customHandler)
        {
            CapturedConnectionInformation_ = new IOIOHandlerCaptureConnectionState();
            CapturedLogs_ = new IOIOHandlerCaptureLog(10);
            if (customHandler != null)
            {
                InboundHandler_ = new IOIOHandlerDistributor(
                    new List<IOIOIncomingHandler> { CapturedConnectionInformation_, CapturedLogs_, customHandler });
            }
            else
            {
                InboundHandler_ = new IOIOHandlerDistributor(
                    new List<IOIOIncomingHandler> { CapturedConnectionInformation_, CapturedLogs_ });
            }
        }


        /// <summary>
        /// This method is not yet finished.
        /// </summary>
        public void WaitForConnect()
        {
            Conn_.WaitForConnect();

            // Use the same cancel token for inbound and outbound
            CancelTokenSource_ = new CancellationTokenSource();
            OutProt_ = new IOIOProtocolOutgoing(this.Conn_.GetOutputStream());
            InProt_ = new IOIOProtocolIncoming(this.Conn_.GetInputStream(), this.InboundHandler_, CancelTokenSource_);
            // start the message pump
            OutgoingTask_ = new Task(run, CancelTokenSource_.Token, TaskCreationOptions.LongRunning);
            OutgoingTask_.Start();
			// message sink has to be started before we can verify board version because the board staus is picked up by the sync
			//Joe's COM4 @ 115200 spits out HW,BootLoader,InterfaceVersion: IOIOSPRK0016IOIO0311IOIO0500
			initBoardVersion();
			//checkInterfaceVersion();
		}

		/// <summary>
		/// Verify we are connected and set our state accordingly
		/// </summary>
		private void initBoardVersion()
        {
			// inbound sink must be running before we get here.
			// the inbound handler actually has already processed the board version.  
			for (int i = 0; i < 10 && CapturedConnectionInformation_.EstablishConnectionFrom_ == null; i++)
			{
					System.Threading.Thread.Sleep(10);
            }
			if (CapturedConnectionInformation_.EstablishConnectionFrom_ == null)
            {
                State_ = IOIOState.DEAD;
				LOG.Error("Failed to identify the board version");
            }
            else
            {
                State_ = IOIOState.CONNECTED;
				BoardResourceManager_ = new ResourceManager(CapturedConnectionInformation_.EstablishConnectionFrom_.Hardware);
            }
            LOG.Info("Hardware is " + CapturedConnectionInformation_.EstablishConnectionFrom_);
        }

        /// <summary>
        /// This method is not yet finished.
        /// This is really an outbound call to the IOIO.  
        /// It will tell us someitme later if it is the right version
        /// </summary>
        private void checkInterfaceVersion()
        {
            ICheckInterfaceVersionTo CheckInterfaceVersionTo_ = new CheckInterfaceVersionCommand(IOIORequiredInterfaceId.REQUIRED_INTERFACE_ID);
            this.PostMessage(CheckInterfaceVersionTo_);
            LOG.Warn("checkInterfaceVersion should poll for the response");
            //State_ = IOIOState.INCOMPATIBLE;
        }

        public void Disconnect()
        {
            CancelTokenSource_.Cancel();
        }

        public void WaitForDisconnect()
        {
            throw new NotImplementedException();
        }

        public IOIOState GetState()
        {
            return State_;
        }

        public void SoftReset()
        {
            throw new NotImplementedException();
        }

        public void HardReset()
        {
            throw new NotImplementedException();
        }

        public void BeginBatch()
        {
            throw new NotImplementedException();
        }

        public void EndBatch()
        {
            throw new NotImplementedException();
        }

        public void Sync()
        {
            throw new NotImplementedException();
        }


        public void PostMessage(IPostMessageCommand message)
        {
			if (this.BoardResourceManager_ == null)
			{
				LOG.Error("Can't post messages without a resource manager");
				throw new IllegalStateException("Can't post messages without a resource manager");
			}
			else
			{
				message.Alloc(this.BoardResourceManager_);
			}
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
					IPostMessageCommand result;
					// use timeout so we can get cancellation token
					// use blocking queue so that we aren't spinning
					bool didTake = WorkQueue.TryTake(out result, timeout);
					if (didTake && result != null)
					{
						result.ExecuteMessage(this.OutProt_);
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
				LOG.Error(OutgoingTask_.Id + " Probably closed outgoing Stream_: (ODE)" + e.Message);
			}
			catch (NullReferenceException e)
			{
				LOG.Error(OutgoingTask_+" Caught Null Reference when sending message",e);
			}
			catch (Exception e)
			{
				LOG.Error(OutgoingTask_.Id + " Probably stopping outgoing: (E)" + e.Message);
			}
			finally
			{
				// we don't play swith Stream_ since we didn't create it
				LOG.Debug(OutgoingTask_.Id + " Throwing thread cancel to make sure outgoing thread stopped");
				// this is redundant if we got here because of thread stop
				this.CancelTokenSource_.Cancel();
				// debugger will always stop here in unit tests if test dynamically determines what Port_ ot use
				// just hit continue in the debugger
				this.CancelTokenSource_.Token.ThrowIfCancellationRequested();
				// should tell OutProt_ to shut down
				this.OutProt_ = null;
				this.OutgoingTask_ = null;
			}

        }

    }
}
