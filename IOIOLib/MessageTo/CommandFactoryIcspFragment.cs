﻿using IOIOLib.Component.Types;
using IOIOLib.MessageTo.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
	 public partial class IOIOMessageCommandFactory
	{

		public IIcspMasterConfigureCommand CreateIcspMasterConfigure()
		{
			return new IcspMasterConfigureCommand();
		}

	}
}
