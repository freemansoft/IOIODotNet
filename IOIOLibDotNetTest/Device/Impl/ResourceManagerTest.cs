using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IOIOLib.Device.Impl;
using IOIOLib.Device.Types;
using IOIOLib.IOIOException;
using System.Collections.Generic;

namespace IOIOLibDotNetTest.Device.Impl
{
	[TestClass]
	public class ResourceManagerTest
	{
		[TestMethod]
		public void ResourceManagerTest_BindHardware()
		{
			ResourceManager underTest = new ResourceManager(Hardware.IOIO0004);
		}

		[TestMethod]
		public void ResourceManagerTest_ICSPSingle()
		{
			ResourceManager underTest = new ResourceManager(Hardware.IOIO0004);
			underTest.Alloc(new Resource(ResourceType.ICSP));
		}

		/// <summary>
		/// all boards have only one ICSP at this time
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(OutOfResourceException))]
		public void ResourceManagerTest_ICSPTooMany()
		{
			// use ICSP because it is hard coded as only one
			ResourceManager underTest = new ResourceManager(Hardware.IOIO0004);
			underTest.Alloc(new Resource(ResourceType.ICSP));
			underTest.Alloc(new Resource(ResourceType.ICSP));
		}

		[TestMethod]
		public void ResourceManagerTest_SinglePins()
		{
			ResourceManager underTest = new ResourceManager(Hardware.IOIO0004);
			underTest.Alloc(new Resource(ResourceType.PIN, 1));
			underTest.Alloc(new Resource(ResourceType.PIN, 2));
			underTest.Alloc(new Resource(ResourceType.PIN, 3));
			try
			{
				underTest.Alloc(new Resource(ResourceType.PIN, 3));
				// should fail with duplicate request
				Assert.Fail("Should have thrown exception when requested alloc on alloc'd resource");
			}
			catch (OutOfResourceException e)
			{
				// this is good s
			}
		}
	}
}
