using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface IEstablishConnectionFrom : IMessageFromIOIO
    {
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        string HardwareId_ { get; }
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        string BootloaderId_ { get; }
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        string FirmwareId_ { get; }
        /// <summary>
        /// should we have a variable here or just look it up each time?
        /// this should probably not exist.  It should be posted as part of event to listeners
        /// </summary>
        Device.Types.Hardware Hardware_ { get; }
    }
}
