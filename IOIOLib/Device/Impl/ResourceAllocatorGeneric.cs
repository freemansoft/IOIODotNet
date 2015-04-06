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

using IOIOLib.IOIOException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOIOLib.Device.Types;

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
            foreach (int id in ids)
            {
                Available_.Add(id);
            }
        }

        /// <summary>
        /// We ignore the passed Id on the resource.  We populate the Id on the resource
        /// </summary>
        /// <param name="r">MODIFIED by action</param>
        public virtual void Alloc(Types.Resource r)
        {
			if (r.Id_ != Resource.ID_NOT_SET)
			{
				throw new ArgumentException("requested resource id must be ID_NOT_SET when requesting generic resource");
			}
			else if (Available_.Count == 0 )
            {
                throw new OutOfResourceException("No more resources of the Alloc() type: " +r.Type);
            }
            r.Id_ = Available_.First<int>();
            Allocated_.Add(r.Id_);
            Available_.Remove(r.Id_);
        }

		/// <summary>
		/// The resource we are freeing
		/// </summary>
		/// <param name="r">we will de-allocate the resource bound by Id_</param>
		/// <summary>
		/// Frees the resource and sets the Resource id to ID_NOT_SET
		/// </summary>
		/// <param name="r">MODIFIED by action</param>
		public virtual void Free(Types.Resource r)
        {
			if (r.Id_ == Resource.ID_NOT_SET)
			{
				throw new ArgumentException("requested resource id must be specified when Free() specific resource");
			}
			else if (Allocated_.Contains<int>(r.Id_))
			{
				Available_.Add(r.Id_);
				Allocated_.Remove(r.Id_);
				r.Id_ = Types.Resource.ID_NOT_SET;
			}
			else {
				throw new ArgumentException("Resource " + r.Id_ + " not allocated.");
			}
		}
	}
}
