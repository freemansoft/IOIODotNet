using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageTo.Impl
{
    public class CheckInterfaceVersionTo : ICheckInterfaceVersionTo
    {
        public byte[] InterfaceId { get; private set; }

        internal CheckInterfaceVersionTo(byte[] interfaceId)
        {
            this.InterfaceId = interfaceId;
        }

        public bool ExecuteMessage(Device.Impl.IOIOProtocolOutgoing outBound)
        {
            outBound.checkInterfaceVersion();
            return true;
        }
    }
}
