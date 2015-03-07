using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    class AnalogPinStatusFrom : IAnalogPinStatusFrom
    {

        public int Pin {get; protected set;}
        public bool IsOpen { get; protected set; }

        public AnalogPinStatusFrom(int pin, bool open)
        {
            // TODO: Complete member initialization
            this.Pin = pin;
            this.IsOpen = open;
        }
    }
}
