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
        public string HardwareId_ { get; private set; }
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        public string BootloaderId_ { get; private set; }
        /// <summary>
        /// provided by IOIO when connects
        /// </summary>
        public string FirmwareId_ { get; private set; }
        /// <summary>
        /// should we have a variable here or just look it up each time?
        /// this should probably not exist.  It should be posted as part of event to listeners
        /// </summary>
        public Device.Types.Hardware Hardware_ { get; private set; }


        internal EstablishConnectionFrom(string hardwareId, string bootloaderId, string firmwareId, Device.Types.Hardware hardware)
        {
            this.Hardware_ = null;

            this.HardwareId_ = hardwareId;
            this.BootloaderId_ = bootloaderId;
            this.FirmwareId_ = firmwareId;
            this.Hardware_ = hardware;
        }
    }
}
