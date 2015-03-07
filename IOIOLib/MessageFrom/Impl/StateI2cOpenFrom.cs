using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class StateI2cOpenFrom : IStateI2cOpenFrom
    {
        private int i2cNum;

        public StateI2cOpenFrom(int i2cNum)
        {
            // TODO: Complete member initialization
            this.i2cNum = i2cNum;
        }
    }
}
