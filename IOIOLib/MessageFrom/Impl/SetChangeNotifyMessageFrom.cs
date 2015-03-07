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


        internal SetChangeNotifyMessageFrom(int pin, bool changeNotify)
        {
            this.Pin = pin;
            this.ChangeNotify = changeNotify;
        }
    }
}
