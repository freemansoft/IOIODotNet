using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class SetChangeNotifyMessageFrom : ISetChangeNotifyMessageFrom
    {
        
        public int Pin { get; private set; }
        public bool ChangeNotify { get; set; }


        public SetChangeNotifyMessageFrom(int pin, bool changeNotify)
        {
            // TODO: Complete member initialization
            this.Pin = pin;
            this.ChangeNotify = changeNotify;
        }
    }
}
