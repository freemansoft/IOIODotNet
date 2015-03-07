using IOIOLib.Device.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo
{
    public interface IPostMessageTo
    {        /// <summary>
        /// 
        /// </summary>
        /// <param name="outBound">the protocol bond to the device</param>
        /// <returns>return true if processing successful</returns>
        bool ExecuteMessage(IOIOProtocolOutgoing outBound);

    }
}
