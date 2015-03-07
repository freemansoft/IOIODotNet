using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class I2cCloseFrom : II2cCloseFrom
    {

        public int I2cNum { get; private set; }

        internal I2cCloseFrom(int i2cNum)
        {
            this.I2cNum = i2cNum;
        }
    }
}
