using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class ConfigurePulseInputTo : IConfigurePulseInputTo
    {
        //
        // from java IOIOImpl  we just need to  set the values and let the outgoing protocol make the calls
        /*
        checkState();
        hardware_.checkSupportsPeripheralInput(spec.pin);
        Resource pin = new Resource(ResourceType.PIN, spec.pin);
        Resource incap = new Resource(
                doublePrecision ? ResourceType.INCAP_DOUBLE
                        : ResourceType.INCAP_SINGLE);
        resourceManager_.alloc(pin, incap);

        IncapImpl result = new IncapImpl(this, mode, incap, pin, rate.hertz,
                mode.scaling, doublePrecision);
        addDisconnectListener(result);
        incomingState_.addIncapListener(incap.id, result);
        try {
            protocol_.setPinDigitalIn(spec.pin, spec.mode);
            protocol_.setPinIncap(spec.pin, incap.id, true);
            protocol_.incapConfigure(incap.id, doublePrecision,
                    mode.ordinal() + 1, rate.ordinal());
        } catch (IOException e) {
            result.close();
            throw new ConnectionLostException(e);
        }
         */
        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            throw new NotImplementedException();
        }
    }
}
