using IOIOLib.IOIOException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    internal class ResourceAllocatorGeneric : IResourceAllocator
    {
        /// <summary>
        /// Preserved initial value
        /// </summary>
        private int Offset_;
        /// <summary>
        /// preserved initial value
        /// </summary>
        private int Count_ ;
        /// <summary>
        /// preserved initial value
        /// </summary>
        private int[] Ids_;

        private ISet<int> Available_;
        private ISet<int> Allocated_;

        /// <summary>
        /// Creates allocator to manage passed in resources
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public ResourceAllocatorGeneric(int offset, int count)
        {
            // TODO: Complete member initialization
            this.Offset_ = offset;
            this.Count_ = count;
            Available_ = new HashSet<int>();
            Allocated_ = new HashSet<int>();
            for (int i = offset; i < offset+count; i++)
            {
                Available_.Add(i);
            }
        }

        /// <summary>
        /// Creates allocator to manage passed in resources
        /// </summary>
        /// <param name="ids"></param>
        public ResourceAllocatorGeneric(int[] ids)
        {
            // TODO: Complete member initialization
            this.Ids_ = ids;
            Available_ = new HashSet<int>();
            Allocated_ = new HashSet<int>();
            foreach (int i in ids)
            {
                Available_.Add(ids[i]);
            }
        }

        /// <summary>
        /// We populate the resource
        /// </summary>
        /// <param name="r">resource whose id we set</param>
        public void Alloc(Types.Resource r)
        {
            if (Available_.Count == 0 )
            {
                throw new OutOfResourceException("No more resources of the requested type: "+r.Type);
            }
            r.Id_ = Available_.First<int>();
            Allocated_.Add(r.Id_);
            Available_.Remove(r.Id_);
        }

        /// <summary>
        /// The resource we are freeing
        /// </summary>
        /// <param name="r">we will de-allocate the resource bound by Id_</param>
        public void Free(Types.Resource r)
        {
            if (Allocated_.Contains<int>(r.Id_))
            {
                throw new ArgumentException("Resource " + r.Id_ + " not allocated.");
            }
            Available_.Add(r.Id_);
            Allocated_.Remove(r.Id_);
        }
    }
}
