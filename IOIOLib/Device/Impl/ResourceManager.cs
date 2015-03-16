using IOIOLib.Device.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    public class ResourceManager
    {

        private IResourceAllocator[] Allocators_ = new IResourceAllocator[
            Enum.GetNames(typeof(ResourceType)).Length];

        public ResourceManager(Hardware hardware)
        {
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
        }

        /// <summary>
        /// Th
        /// </summary>
        /// <param name="args">variable number of args.  can just call with Alloc(a,b,c,d)</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Alloc(params Object[] args)
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
        public void Free(params Object[] args)
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
        public void Alloc(ICollection<Resource> resources)
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
        public void Free(ICollection<Resource> resources)
        {
            foreach (Resource r in resources)
            {
                Free(r);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Alloc(Resource r)
        {
            if (r != null)
            {
                Allocators_[(int)r.Type].Alloc(r);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Free(Resource r)
        {
            if (r != null)
            {
                Allocators_[(int)r.Type].Free(r);
            }
        }
    }
}
