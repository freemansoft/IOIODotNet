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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOIOLib.Device.Types;
using IOIOLib.IOIOException;

namespace IOIOLib.Device.Impl
{
    class ResourceAllocatorSpecific : IResourceAllocator
    {
		/// <summary>
		/// Preserved initial value
		/// </summary>
		int Offset_;
		/// <summary>
		/// Preserved initial value
		/// </summary>
		int Count_;

		private ISet<int> Available_;
		private ISet<int> Allocated_;

		public ResourceAllocatorSpecific(int offset, int count)
        {
            Offset_ = offset;
            Count_ = count;
			Available_ = new HashSet<int>();
			Allocated_ = new HashSet<int>();
			for (int i = offset; i < offset + count; i++)
			{
				Available_.Add(i);
			}
		}

		/// <summary>
		/// we try and reserve the specific resource ID passed in
		/// </summary>
		/// <param name="r">MODIFIED by action</param>
		public void Alloc(Resource r)
        {
			if (r.Id_ == Resource.ID_NOT_SET)
			{
				throw new ArgumentException("requested resource id must be specified when Alloc() specific resource");
			}
			else if (Available_.Contains(r.Id_))
			{
				Allocated_.Add(r.Id_);
				Available_.Remove(r.Id_);
			}
			else
			{
				throw new OutOfResourceException("resource id not available: " + r.Id_);
			}
        }

		/// <summary>
		/// Frees the resource and sets the Resource id to ID_NOT_SET
		/// </summary>
		/// <param name="r">MODIFIED by action</param>
		public void Free(Types.Resource r)
        {
			if (r.Id_ == Resource.ID_NOT_SET)
			{
				throw new ArgumentException("requested resource id must be specified when Free() specific resource");
			}
			else if (Allocated_.Contains(r.Id_))
			{
				Available_.Add(r.Id_);
				Allocated_.Remove(r.Id_);
				r.Id_ = Types.Resource.ID_NOT_SET;
			}
			else
			{
				throw new ArgumentException("resource id not in use: " + r.Id_);
			}
		}
	}
}
