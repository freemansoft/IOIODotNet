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

namespace IOIOLib.Component.Types
{
    /** An enumeration for describing the module's operating mode. */
    public class PulseInputMode
    {
        /** Positive pulse measurement (rising-edge-to-falling-edge). */
        public static PulseInputMode POSITIVE = new PulseInputMode(1);
        /** Negative pulse measurement (falling-edge-to-rising-edge). */
        public static PulseInputMode NEGATIVE = new PulseInputMode(1);
        /** Frequency measurement (rising-edge-to-rising-edge). */
        public static PulseInputMode FREQ = new PulseInputMode(1);
        /** Frequency measurement (rising-edge-to-rising-edge) with 4x scaling. */
        public static PulseInputMode FREQ_SCALE_4 = new PulseInputMode(4);
        /** Frequency measurement (rising-edge-to-rising-edge) with 16x scaling. */
        public static PulseInputMode FREQ_SCALE_16 = new PulseInputMode(16);

        /** The scaling factor as an integer. */
        public int scaling {  get;  set; }

        private PulseInputMode(int s)
        {
            scaling = s;
        }
    }
}
