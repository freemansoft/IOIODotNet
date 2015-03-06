using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOIOLib.Component
{
    /// <summary>
    /// Abstraction for this feature. See the Java version for an idea of functionality
    /// </summary>
    class TwiMaster : ITwiMaster
    {
        private int twiNum;
        private Types.TwiMasterRate rate;
        private bool smbus;

        public TwiMaster(int twiNum, Types.TwiMasterRate rate, bool smbus)
        {
            // TODO: Complete member initialization
            this.twiNum = twiNum;
            this.rate = rate;
            this.smbus = smbus;
        }
    }
}
