﻿/*
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

using IOIOLib.Device.Types;
using IOIOLib.Message;
using IOIOLib.Util;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    public class ResourceManager : IResourceManager
	{
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(ResourceManager));

        public Hardware BoundHardware { get; private set; }

        private IResourceAllocator[] Allocators_ = new IResourceAllocator[
            Enum.GetNames(typeof(ResourceType)).Length];

        private ObserverTxStatusI2c I2cObserver_;
        private ObserverTxStatusSpi SpiObserver_;
        private ObserverTxStatusUart UartObserver_;
        // visible to IOIO - ugh
        // do I need to initialize this if the only constructor populates it?
        internal ConcurrentBag<IObserverIOIO> Observers_ { get; private set; }
            = new ConcurrentBag<IObserverIOIO>();

        public ResourceManager(Hardware hardware)
        {
			this.BoundHardware = hardware;
            Allocators_[(int)ResourceType.PIN] = new ResourceAllocatorSpecific(0, hardware.NumPins());
            Allocators_[(int)ResourceType.TWI] = new ResourceAllocatorSpecific(0, hardware.NumTwiModules());
            Allocators_[(int)ResourceType.ICSP] = new ResourceAllocatorGeneric(0, 1);
            Allocators_[(int)ResourceType.OUTCOMPARE] = new ResourceAllocatorGeneric(0, hardware.NumPwmModules);
            Allocators_[(int)ResourceType.UART] = new ResourceAllocatorGeneric(0, hardware.NumUartModules);
            Allocators_[(int)ResourceType.SPI] = new ResourceAllocatorGeneric(0, hardware.NumSpiModules);
            Allocators_[(int)ResourceType.INCAP_SINGLE] = new ResourceAllocatorGeneric(
                hardware.IncapSingleModules);
            Allocators_[(int)ResourceType.INCAP_DOUBLE] = new ResourceAllocatorGeneric(
                hardware.IncapDoubleModules);
            Allocators_[(int)ResourceType.SEQUENCER] = new ResourceAllocatorGeneric(0, 1);

            I2cObserver_ = new ObserverTxStatusI2c();
            SpiObserver_ = new ObserverTxStatusSpi();
            UartObserver_ = new ObserverTxStatusUart();
            Observers_ = new ConcurrentBag<IObserverIOIO>() { I2cObserver_, SpiObserver_, UartObserver_ };

    }

    /// <summary>
    /// Th
    /// </summary>
    /// <param name="args">variable number of args.  can just call with Alloc(a,b,c,d)</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Alloc(params Object[] args)
        {
            int i = 0;
            try
            {
                for (i = 0; i < args.Length; ++i)
                {
                    if (args[i] != null)
                    {
                        if (args[i] is Resource)
                        {
                            Alloc((Resource)args[i]);
                        }
                        else if (args[i] is Resource[])
                        {
                            // this shouldn't be necessary but compler couldn't handle (Resource[])args[i]
                            Alloc(((Resource[])args[i]).ToList<Resource>());
                        }
                        else if (args[i] is ICollection)
                        {
                            Alloc((ICollection<Resource>)args[i]);
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                for (int j = 0; j < i; ++j)
                {
                    if (args[j] is Resource)
                    {
                        Free((Resource)args[j]);
                    }
                    else if (args[j] is Resource[])
                    {
                        // this shouldn't be necessary but compler couldn't handle (Resource[])args[i]
                        Free(((Resource[])args[j]).ToList<Resource>());
                    }
                    else if (args[j] is ICollection)
                    {
                        Free((ICollection<Resource>)args[j]);
                    }
                }
                throw e;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Free(params Object[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] is Resource)
                {
                    Free((Resource)args[i]);
                }
                else if (args[i] is Resource[])
                {
                    // this shouldn't be necessary but compler couldn't handle (Resource[])args[i]
                    Free(((Resource[])args[i]).ToList<Resource>());
                }
                else if (args[i] is ICollection)
                {
                    Free((ICollection<Resource>)args[i]);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Alloc(ICollection<Resource> resources)
        {
            int i = 0;
            try
            {
                foreach (Resource foo in resources)
                {
                    Alloc(foo);
                    ++i;
                }
            }
            catch (Exception e)
            {
                foreach (Resource foo in resources)
                {
                    if (i-- > 0)
                    {
                        Free(foo);
                    }
                }
                throw e;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Free(ICollection<Resource> resources)
        {
            foreach (Resource r in resources)
            {
                Free(r);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Alloc(Resource r)
        {
            if (r != null)
            {
                Allocators_[(int)r.Type].Alloc(r);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Free(Resource r)
        {
            if (r != null)
            {
                Allocators_[(int)r.Type].Free(r);
            }
        }

        /// <summary>
        /// Registers the bus observers with the observable, 
        /// usually an IOIOImpl observer handler to catch inbound messages via the inboudn protocol handler
        /// should also be added to the outbound protocol handler so that we can get in and out on the busses
        /// </summary>
        /// <param name="observable"></param>
        internal void RegisterWithObservable(IObservableIOIO observable)
        {
            LOG.Debug("Registering " + Observers_.Count + " observers");
            foreach (IObserverIOIO oneObserver in this.Observers_)
            {
                observable.Subscribe(oneObserver);
            }
        }
    }
}
