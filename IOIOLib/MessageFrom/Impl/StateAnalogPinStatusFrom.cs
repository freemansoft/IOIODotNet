using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class StateAnalogPinStatusFrom : IStateAnalogPinStatusFrom
    {
        private int p1;
        private int p2;

        public StateAnalogPinStatusFrom(int p1, int p2)
        {
            // TODO: Complete member initialization
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
