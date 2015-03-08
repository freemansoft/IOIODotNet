/*
 * Copyright 2011 Ytai Ben-Tsvi. All rights reserved.
 * Copyright 2015 Joe Freeman. All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 * 
 *    1. Redistributions of source code must retain the above copyright notice, this list of
 *       conditions and the following disclaimer.
 * 
 *    2. Redistributions in binary form must reproduce the above copyright notice, this list
 *       of conditions and the following disclaimer in the documentation and/or other materials
 *       provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED 'AS IS AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL ARSHAN POURSOHI OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied.
 */
 
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
        // from java IOIOImpl  we just need to  set the Values and let the outgoing protocol make the calls
        /*
        checkState();
        hardware_.checkSupportsPeripheralInput(spec.Pin);
        Resource Pin = new Resource(ResourceType.PIN, spec.Pin);
        Resource incap = new Resource(
                doublePrecision ? ResourceType.INCAP_DOUBLE
                        : ResourceType.INCAP_SINGLE);
        resourceManager_.alloc(Pin, incap);

        IncapImpl result = new IncapImpl(this, mode, incap, Pin, rate.hertz,
                mode.scaling, doublePrecision);
        addDisconnectListener(result);
        incomingState_.addIncapListener(incap.id, result);
        try {
            protocol_.setPinDigitalIn(spec.Pin, spec.mode);
            protocol_.setPinIncap(spec.Pin, incap.id, true);
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
