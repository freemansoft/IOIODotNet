using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class SetChangeNotifyMessageFrom : ISetChangeNotifyMessageFrom
    {
        private int pin;
        private bool changeNotify;

        public SetChangeNotifyMessageFrom(int pin, bool changeNotify)
        {
            // TODO: Complete member initialization
            this.pin = pin;
            this.changeNotify = changeNotify;
        }
    }
}
