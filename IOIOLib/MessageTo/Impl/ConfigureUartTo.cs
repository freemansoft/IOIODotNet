using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class ConfigureUartTo : IConfigureUartTo
    {
        public UartParity Parity { get; private set; }
        public int UartNum { get; private set; }
        public UartStopBits StopBits { get; private set; }
        public int Rate { get; private set; }
        public bool Speed4x { get; private set; }
        public DigitalInputSpec RXSpec { get; private set; }
        public DigitalOutputSpec TXSpec { get; private set; }



        internal ConfigureUartTo(Component.Types.DigitalInputSpec digitalInputSpec, Component.Types.DigitalOutputSpec digitalOutputSpec, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
        {
            // TODO: Complete resource allocation
            this.RXSpec = digitalInputSpec;
            this.TXSpec = digitalOutputSpec;
            this.CalculateRateAndSpeed4X(baud);
            this.Parity = parity;
            this.StopBits = stopbits;
            this.UartNum = 0;
            /*
                checkState();
                if (rx != null) {
                    hardware_.checkSupportsPeripheralInput(rx.Pin);
                }
                if (isTx != null) {
                    hardware_.checkSupportsPeripheralOutput(isTx.Pin);
                }
                Resource rxPin = rx != null ? new Resource(ResourceType.PIN, rx.Pin)
                        : null;
                Resource txPin = isTx != null ? new Resource(ResourceType.PIN, isTx.Pin)
                        : null;
                Resource uart = new Resource(ResourceType.UART);
                resourceManager_.alloc(rxPin, txPin, uart);

                UartImpl result = new UartImpl(this, txPin, rxPin, uart);
                addDisconnectListener(result);
                incomingState_.addUartListener(uart.id, result);
             * ***************************
        */
        }

        private void CalculateRateAndSpeed4X(int baud)
        {
            Speed4x = true;
            Rate = (int)(Math.Round(4000000.0f / baud) - 1);
            if (Rate > 65535)
            {
                Speed4x = false;
                Rate = (int)(Math.Round(1000000.0f / baud) - 1);
            }
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
