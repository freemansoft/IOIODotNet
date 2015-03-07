using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class I2cCloseFrom : II2cCloseFrom
    {
        private int i2cNum;

        public I2cCloseFrom(int i2cNum)
        {
            // TODO: Complete member initialization
            this.i2cNum = i2cNum;
        }
    }
}
