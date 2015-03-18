using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IOIOLib.Device.Impl;
using IOIOLib.Device.Types;
using IOIOLib.IOIOException;

namespace IOIOLibDotNetTest.Device.Impl
{
	[TestClass]
	public class ResourceAllocatorSpecificTest
	{
		[TestMethod]
		public void ResourceAllocatorSpecificTest_SimplePinRequest()
		{
			ResourceAllocatorSpecific underTest = new ResourceAllocatorSpecific(10, 4);
			Resource r0 = new Resource(ResourceType.PIN, 10);
			Resource r1 = new Resource(ResourceType.PIN, 11);
			Resource r2 = new Resource(ResourceType.PIN, 12);
			Resource r3 = new Resource(ResourceType.PIN, 13);
			underTest.Alloc(r0);
			underTest.Alloc(r1);
			underTest.Alloc(r2);
			underTest.Alloc(r3);
			Assert.AreNotEqual(Resource.ID_NOT_SET, r0.Id_);
			Assert.AreNotEqual(Resource.ID_NOT_SET, r1.Id_);
			Assert.AreNotEqual(Resource.ID_NOT_SET, r2.Id_);
			Assert.AreNotEqual(Resource.ID_NOT_SET, r3.Id_);
			underTest.Free(r0);
			underTest.Free(r1);
			underTest.Free(r2);
			underTest.Free(r3);
			Assert.AreEqual(Resource.ID_NOT_SET, r0.Id_);
			Assert.AreEqual(Resource.ID_NOT_SET, r1.Id_);
			Assert.AreEqual(Resource.ID_NOT_SET, r2.Id_);
			Assert.AreEqual(Resource.ID_NOT_SET, r3.Id_);
		}

		/// <summary>
		/// Fail to re-alloc same pin twice
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(OutOfResourceException))]
		public void ResourceAllocatorSpecificTest_SimplePinRequestRedundant()
		{
			ResourceAllocatorSpecific underTest = new ResourceAllocatorSpecific(10, 4);
			Resource r1 = new Resource(ResourceType.PIN, 10);
			Resource r2 = new Resource(ResourceType.PIN, 10);
			underTest.Alloc(r1);
			underTest.Alloc(r2);
			// should never get here because of exception of duplicate requests
		}

		/// <summary>
		///  fail to alloc something that was out of range
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(OutOfResourceException))]
		public void ResourceAllocatorSpecificTest_InvalidPin()
		{
			ResourceAllocatorSpecific underTest = new ResourceAllocatorSpecific(10, 4);
			Resource r1 = new Resource(ResourceType.PIN, 7);
			underTest.Alloc(r1);
			// should never get here because of exception of out of request
		}

		/// <summary>
		///  fail to alloc something that was out of range
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ResourceAllocatorSpecificTest_InvalidPinNotspecified()
		{
			ResourceAllocatorSpecific underTest = new ResourceAllocatorSpecific(10, 4);
			Resource r1 = new Resource(ResourceType.PIN, Resource.ID_NOT_SET);
			underTest.Alloc(r1);
			// should never get here because of exception of out of request
		}
		/// <summary>
		/// fail to free something never allocated
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ResourceAllocatorSpecificTest_FreeUnAllocated()
		{
			ResourceAllocatorSpecific underTest = new ResourceAllocatorSpecific(10, 4);
			Resource r1 = new Resource(ResourceType.PIN, 10);
			underTest.Free(r1);
			// should never get here because of exception of out of request
		}
	}
}
