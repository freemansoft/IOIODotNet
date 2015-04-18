using IOIOLib.MessageFrom;
using IOIOLib.Util;
using System;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// This class catches configuration information.
    /// It should also catch state and reset and other operations
    /// </summary>
    public class IOIOConnectionStateObserver : IObserver<IConnectedDeviceResponse>, IObserver<ISupportedInterfaceFrom>, IObserverIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(IOIOConnectionStateObserver));
        /// <summary>
        /// response from the checkInterfaceResponse call 
        /// </summary>
        internal ISupportedInterfaceFrom Supported_;

        internal IConnectedDeviceResponse EstablishConnectionFrom_;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(IConnectedDeviceResponse value)
        {
            this.EstablishConnectionFrom_ = value;
        }

        public void OnNext(ISupportedInterfaceFrom value)
        {
            this.Supported_ = value;
        }

        /// <summary>
        /// Legacy API
        /// </summary>
        /// <returns></returns>
        public virtual IConnectedDeviceResponse ConnectedDeviceDescription()
        {
            return EstablishConnectionFrom_;
        }
    }
}
