using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOIOLib.MessageTo.Impl
{
    public interface ICheckInterfaceVersionTo:IMesssageToIOIO, IPostMessageTo
    {
        byte[] InterfaceId { get; set; }
    }
}
