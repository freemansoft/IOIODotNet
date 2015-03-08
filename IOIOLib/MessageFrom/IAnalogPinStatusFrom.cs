using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    interface IAnalogPinStatusFrom : IAnalogInFrom
    {

        bool IsOpen { get; }
    }
}
