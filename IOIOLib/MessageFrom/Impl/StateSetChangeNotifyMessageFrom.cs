using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class StateSetChangeNotifyMessageFrom : IStateSetChangeNotifyMessageFrom
    {
        private int pin;
        private bool changeNotify;

        public StateSetChangeNotifyMessageFrom(int pin, bool changeNotify)
        {
            // TODO: Complete member initialization
            this.pin = pin;
            this.changeNotify = changeNotify;
        }
    }
}
