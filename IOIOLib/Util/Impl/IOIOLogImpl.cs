/*
 * Copyright 2011 Ytai Ben-Tsvi. All rights reserved.
 * Copyright 2015 Joe Freeman. All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 * 
 *    1. Redistributions of source code must retain the above copyright notice, this list of
 *       conditions and the following disclaimer.
 * 
 *    2. Redistributions in binary form must reproduce the above copyright notice, this list
 *       of conditions and the following disclaimer in the documentation and/or other materials
 *       provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED 'AS IS AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL ARSHAN POURSOHI OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied.
 */
 
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
        private Type Type_;
        private ILog WrappedLogger_;

        public IOIOLogImpl(Type type)
        {
            this.Type_ = type;
            this.WrappedLogger_ = LogManager.GetLogger(type);
        }

        public void Debug(object message)
        {
            WrappedLogger_.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            WrappedLogger_.Debug(message, exception);
        }

        public void Info(object message)
        {
            WrappedLogger_.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            WrappedLogger_.Info(message, exception);
        }

        public void Warn(object message)
        {
            WrappedLogger_.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            WrappedLogger_.Warn(message, exception);
        }

        public void Error(object message)
        {
            WrappedLogger_.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            WrappedLogger_.Error(message, exception);
        }
    }
}
