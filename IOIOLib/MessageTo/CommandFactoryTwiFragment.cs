using IOIOLib.Component.Types;
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

		public ITwiMasterConfigureCommand CreateTwiConfigure(int twiNum, Component.Types.TwiMasterRate rate, bool smbus)
		{
			return new TwiMasterConfigureCommand(twiNum, rate, smbus);
		}

        public ITwiMasterCloseCommand CreateTwiClose(TwiSpec twiDef)
        {
            return new TwiMasterCloseCommand(twiDef);
        }

        public ITwiMasterSendDataCommand CreateTwiSendData(
            TwiSpec twiDef,
            int address, bool isTenBitAddress,
            byte[] writeData, int numBytesRead)
        {
            return new TwiMasterSendDataCommand(twiDef, address, isTenBitAddress, writeData, numBytesRead);
        }
    }
}
