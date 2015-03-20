using System.Collections.Generic;
using IOIOLib.Device.Types;

namespace IOIOLib.Device
{
	/// <summary>
	/// This interface exists to simplify mocking
	/// </summary>
	public interface IResourceManager
	{
		void Alloc(Resource r);
		void Alloc(params object[] args);
		void Alloc(ICollection<Resource> resources);
		void Free(Resource r);
		void Free(params object[] args);
		void Free(ICollection<Resource> resources);
	}
}