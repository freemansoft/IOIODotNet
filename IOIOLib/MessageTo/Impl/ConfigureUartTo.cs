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

        public ConfigureUartTo(Component.Types.DigitalInputSpec digitalInputSpec, Component.Types.DigitalOutputSpec digitalOutputSpec, int baud, Component.Types.UartParity parity, Component.Types.UartStopBits stopbits)
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
        public UartParity Parity { get; set; }

        public int UartNum { get; set; }

        public UartStopBits StopBits { get; set; }

        public int Rate { get; set; }

        public bool Speed4x { get; set; }

        public DigitalInputSpec RXSpec { get; set; }
        public DigitalOutputSpec TXSpec { get; set; }



        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
