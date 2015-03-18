using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IOIOLib.Device.Impl;
using IOIOLib.Device.Types;
using IOIOLib.IOIOException;
using System.Collections.Generic;

namespace IOIOLibDotNetTest.Device.Impl
{
	[TestClass]
	public class ResourceAllocatorGenericTest
	{
		[TestMethod]
		public void ResourceAllocatorGenericTest_Alloc2Generics()
		{
			ResourceAllocatorGeneric underTest = new ResourceAllocatorGeneric(4, 3);
			Resource r1 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			Resource r2 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			underTest.Alloc(r1);
			underTest.Alloc(r2);
			Assert.AreNotEqual(Resource.ID_NOT_SET, r1.Id_);
			Assert.AreNotEqual(Resource.ID_NOT_SET, r2.Id_);
			Assert.AreNotEqual(r1.Id_, r2.Id_);
		}

		public void ResourceAllocatorGenericTest_AllocAndFree()
		{
			ResourceAllocatorGeneric underTest = new ResourceAllocatorGeneric(4, 3);
			Resource r1 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			underTest.Alloc(r1);
			underTest.Free(r1);
			Assert.AreEqual(Resource.ID_NOT_SET, r1.Id_);
		}

		[TestMethod]
		public void ResourceAllocatorGenericTest_Alloc3Of3()
		{
			ResourceAllocatorGeneric underTest = new ResourceAllocatorGeneric(4, 3);
			Resource r1 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			Resource r2 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			Resource r3 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			Resource r4 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			underTest.Alloc(r1);
			underTest.Alloc(r2);
			underTest.Alloc(r3);
			ISet<int> allocated = new HashSet<int>();
			allocated.Add(r1.Id_);
			allocated.Add(r2.Id_);
			allocated.Add(r3.Id_);
			Assert.AreEqual(3, allocated.Count);
			Assert.IsTrue(allocated.Contains(4));
			Assert.IsTrue(allocated.Contains(5));
			Assert.IsTrue(allocated.Contains(5));
			Assert.AreEqual(3,allocated.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(OutOfResourceException))]
		public void ResourceAllocatorGenericTest_Alloc4Of3()
		{
			ResourceAllocatorGeneric underTest = new ResourceAllocatorGeneric(4, 3);
			Resource r1 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			Resource r2 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			Resource r3 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			Resource r4 = new Resource(ResourceType.UART, Resource.ID_NOT_SET);
			underTest.Alloc(r1);
			underTest.Alloc(r2);
			underTest.Alloc(r3);
			underTest.Alloc(r4);
		}
	}
}
