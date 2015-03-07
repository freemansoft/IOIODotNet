using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class EstablishConnectionFrom : IEstablishConnectionFrom
    {
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        internal string HardwareId_;
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        internal string BootloaderId_;
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        internal string FirmwareId_;
        /// <summary>
        /// should we have a variable here or just look it up each time?
        /// this should probably not exist.  It should be posted as part of event to listeners
        /// </summary>
        internal Device.Types.Hardware Hardware_ = null;

        public EstablishConnectionFrom(string hardwareId, string bootloaderId, string firmwareId, Device.Types.Hardware hardware)
        {
            // TODO: Complete member initialization
            this.HardwareId_ = hardwareId;
            this.BootloaderId_ = bootloaderId;
            this.FirmwareId_ = firmwareId;
            this.Hardware_ = hardware;
        }
    }
}
