using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IOIOLib.Util.Impl
{
    public class IOIOLogImpl : IOIOLog
    {
        private Type type;
        private ILog WrappedLogger;

        public IOIOLogImpl(Type type)
        {
            this.type = type;
            this.WrappedLogger = LogManager.GetLogger(type);
        }

        public void Debug(object message)
        {
            WrappedLogger.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            WrappedLogger.Debug(message, exception);
        }

        public void Info(object message)
        {
            WrappedLogger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            WrappedLogger.Info(message, exception);
        }

        public void Warn(object message)
        {
            WrappedLogger.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            WrappedLogger.Warn(message, exception);
        }

        public void Error(object message)
        {
            WrappedLogger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            WrappedLogger.Error(message, exception);
        }
    }
}
