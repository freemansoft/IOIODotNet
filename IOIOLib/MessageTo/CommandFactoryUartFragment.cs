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


		public IUartConfigureCommand CreateOpenUart(Component.Types.DigitalInputSpec rx, Component.Types.DigitalOutputSpec tx, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
		{
			return new UartConfigureCommand(rx, tx, baud, parity, stopbits);
		}

		public IUartConfigureCommand CreateOpenUart(int rx, int tx, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
		{
			// are these the right pull / drain settings?
			return new UartConfigureCommand(new DigitalInputSpec(rx), new DigitalOutputSpec(tx), baud, parity, stopbits);
		}

		public IUartSendCommand CreateSendUart(UartSpec uart, byte[] data, int byteCount )
		{
			return new UartSendCommand(uart, data, byteCount);

		}
		public IUartCloseCommand CreateCloseUart(UartSpec UartSpec)
		{
			return new UartCloseCommand(UartSpec);
		}

	}
}
