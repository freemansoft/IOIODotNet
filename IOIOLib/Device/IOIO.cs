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
using IOIOLib.Device.Types;
using IOIOLib.MessageTo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device
{
    public interface IOIO : IDevice
    {
        /**
         * Establishes connection with the IOIO board.
         * <p>
         * This method is blocking until connection is established. This method can be aborted by
         * calling {@link #disconnect()}. In this case, it will throw a {@link ConnectionLostException}.
         *
         * @throws ConnectionLostException
         *             An error occurred during connection or disconnect() has been called during
         *             connection. The instance state is disconnected.
         * @throws IncompatibilityException
         *             An incompatible board firmware of hardware has been detected. The instance state
         *             is disconnected.
         * @see #disconnect()
         * @see #waitForDisconnect()
         */
        void waitForConnect();

        /**
         * Closes the connection to the board, or aborts a connection process started with
         * waitForConnect().
         * <p>
         * Once this method is called, this IOIO instance and all the instances obtain from it become
         * invalid and will throw an exception on every operation.
         * <p>
         * This method is asynchronous, i.e. it returns immediately, but it is not guaranteed that all
         * connection-related resources has already been freed and can be reused upon return. In cases
         * when this is important, client can call {@link #waitForDisconnect()}, which will block until
         * all resources have been freed.
         */
        void disconnect();

        /**
         * Blocks until IOIO has been disconnected and all connection-related resources have been freed,
         * so that a new connection can be attempted.
         *
         * @throws InterruptedException
         *             When interrupt() has been called on this thread. This might mean that an
         *             immediate attempt to create and connect a new IOIO object might fail for resource
         *             contention.
         * @see #disconnect()
         * @see #waitForConnect()
         */
        void waitForDisconnect();

        /**
         * Gets the connections state.
         *
         * @return The connection state.
         */
        IOIOState getState();

        /**
         * Resets the entire state (returning to initial state), without dropping the connection.
         * <p>
         * It is equivalent to calling {@link Closeable#close()} on every interface obtained from this
         * instance. A connection must have been established prior to calling this method, by invoking
         * {@link #waitForConnect()}.
         *
         * @throws ConnectionLostException
         *             Connection was lost before or during the execution of this method.
         * @see #hardReset()
         */
        void softReset();

        /**
         * Equivalent to disconnecting and reconnecting the board power supply.
         * <p>
         * The connection will be dropped and not reestablished. Full boot sequence will take place, so
         * firmware upgrades can be performed. A connection must have been established prior to calling
         * this method, by invoking {@link #waitForConnect()}.
         *
         * @throws ConnectionLostException
         *             Connection was lost before or during the execution of this method.
         * @see #softReset()
         */
        void hardReset();

        void postMessage(IPostMessageTo message);




        /**
         * Start a batch of operations. This is strictly an optimization and will not change
         * functionality: if the client knows that a sequence of several IOIO operations are going to be
         * performed immediately following each other, a call to {@link #beginBatch()} before the
         * sequence and {@link #endBatch()} after the sequence will cause the operations to be grouped
         * into one transfer to the IOIO, thus reducing latency. A matching {@link #endBatch()}
         * operation must always follow, or otherwise no operation will ever be actually executed.
         * {@link #beginBatch()} / {@link #endBatch()} blocks may be nested - the transfer will occur
         * when the outermost {@link #endBatch()} is invoked. Note that it is not guaranteed that no
         * transfers will happen while inside a batch - it should be treated as a hint. Code running
         * inside the block must be quick as it blocks <b>all</b> transfers to the IOIO, including those
         * performed from other threads.
         *
         * @throws ConnectionLostException
         *             Connection was lost before or during the execution of this method.
         */
        void beginBatch();

        /**
         * End a batch of operations. For explanation, see {@link #beginBatch()}.
         *
         * @throws ConnectionLostException
         *             Connection was lost before or during the execution of this method.
         */
        void endBatch();

        /**
         * Sends a message to the IOIO and waits for an echo.
         *
         * This is useful for synchronizing asynchronous calls across the entire API, for example: When
         * writing to a {@link IDigitalOutput} and then reading from a {@link IDigitalInput}, if you want
         * to guarantee that the reading was obtained after the write has taken place, call this method
         * in between.
         *
         * @throws ConnectionLostException
         *             Connection was lost before or during the execution of this method.
         * @throws InterruptedException
         *             When interrupt() has been called on this thread.
         */
        void sync();
    }
}
