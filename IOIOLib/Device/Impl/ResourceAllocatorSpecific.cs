using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    class ResourceAllocatorSpecific : IResourceAllocator
    {
        int Offset_;
        int Count_;
        public ResourceAllocatorSpecific(int offset, int count)
        {
            Offset_ = offset;
            Count_ = count;
        }

        public void Alloc(Types.Resource r)
        {
            throw new NotImplementedException();
        }

        public void Free(Types.Resource r)
        {
            throw new NotImplementedException();
        }
    }
}
