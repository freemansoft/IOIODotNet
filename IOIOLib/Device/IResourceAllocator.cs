using IOIOLib.Device.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device
{
    public interface IResourceAllocator
    {
        void Alloc(Resource r);

        void Free(Resource r);
    }
}
