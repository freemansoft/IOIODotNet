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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace IOIOLibDotNetTest.MessageFrom
{
    /// <summary>
    /// non genericized marker interface
    /// </summary>
    public interface IMessageHandler { }
    /// <summary>
    /// non genericized dispatch
    /// </summary>
    public interface IMessageBaseType {
        void Dispatch(IMessageHandler handler );
    }

    public interface IMessageTypeSubType1 : IMessageBaseType { }

    public interface IMessageTypeSubType2 : IMessageBaseType { }

    public interface IMessageTypeSubType23 : IMessageTypeSubType2 { }


    public class MessageBase<MessageType> : IMessageBaseType where MessageType : class, IMessageBaseType
    {
        /// <summary>
        /// http://stackoverflow.com/questions/1477471/design-pattern-for-handling-multiple-message-types
        /// </summary>
        /// <param name="handler"></param>
        public void Dispatch(IMessageHandler handler)
        {
            MessageType msg_as_msg_type = this as MessageType;
            if (msg_as_msg_type != null)
            {
                DynamicDispatch(handler, msg_as_msg_type);
            }
        }

        protected void DynamicDispatch(IMessageHandler handler, MessageType self)
        {
            IObserver<MessageType> handlerTarget = handler as IObserver<MessageType>;
            if (handlerTarget != null)
            {
                handlerTarget.OnNext(self);
            }
        }
    }

    public class MessageSubType1 : MessageBase<IMessageTypeSubType1>, IMessageTypeSubType1
    {
    }
    public class MessageSubType2 : MessageBase<IMessageTypeSubType2>, IMessageTypeSubType2
    {
    }

    public class MessageSubType23 : MessageBase<IMessageTypeSubType23>, IMessageTypeSubType23
    {
    }

    public class MockIncomingDispatcher
    {
        List<IMessageHandler> all = new List<IMessageHandler>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer">Should also be IObserver for some message types</param>
        /// <returns>nothing right now. Should be used to unsubscribe</returns>
        public IDisposable Subscribe(IMessageHandler observer)
        {
            all.Add(observer);
            return null;
        }

        public void NotifyAll(IMessageBaseType target)
        {
            foreach (IMessageHandler handler in all)
            {
                if (handler != null)
                {
                    target.Dispatch(handler);
                }
                else {
                    Console.WriteLine("You didn't define this thing as an IMessageHandler: " + handler);
                }

            }
        }
    }

    /// <summary>
    /// All messages should end up processed by this class
    /// </summary>
    public class ObserverOfBase : IObserver<IMessageBaseType>, IMessageHandler
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IMessageBaseType value)
        {
            Console.WriteLine("OnNext:"
                + "\n MessageType: " + value.GetType()
                + "\n ProcessedAs: MessageTypeBase"
                + "\n Observer: " + this.GetType().ToString() 
                );
        }
    }

    /// <summary>
    /// Only type 1 messages should be processed by this class
    /// </summary>
    public class ObserverOfSubType1 : IObserver<IMessageTypeSubType1>, IMessageHandler
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IMessageTypeSubType1 value)
        {
            Console.WriteLine("OnNext:"
                + "\n MessageType: " + value.GetType()
                + "\n ProcessedAs: IMessageTypeSubType1"
                + "\n Observer: " + this.GetType().ToString()
                );
        }

    }

    /// <summary>
    /// type 1 and type 2 will be processed by this class
    /// </summary>
    public class ObserverOfDeux : IObserver<IMessageTypeSubType1>, IObserver<IMessageTypeSubType2>, IMessageHandler
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IMessageTypeSubType1 value)
        {
            Console.WriteLine("OnNext:"
                + "\n MessageType: " + value.GetType()
                + "\n ProcessedAs: IMessageTypeSubType1"
                + "\n Observer: " + this.GetType().ToString()
                );
        }
        public void OnNext(IMessageTypeSubType2 value)
        {
            Console.WriteLine("OnNext:"
                + "\n MessageType: " + value.GetType()
                + "\n ProcessedAs: IMessageTypeSubType2"
                + "\n Observer: " + this.GetType().ToString()
                );
        }
    }

    [TestClass]
    public class DispatchBasedOnInterfaces
    {
        [TestMethod]
        public void DispatchBasedOnInterfaces_TestMultiple()
        {
            ObserverOfBase anObserverBase = new ObserverOfBase();
            // create 3 observers
            ObserverOfSubType1 anObservableSubType1 = new ObserverOfSubType1();
            ObserverOfDeux anObservableSubTypeSeveral = new ObserverOfDeux();
            MockIncomingDispatcher theObservable = new MockIncomingDispatcher();
            // and register them
            theObservable.Subscribe(anObserverBase);
            theObservable.Subscribe(anObservableSubType1);
            theObservable.Subscribe(anObservableSubTypeSeveral);
            
            // send two different messages
            theObservable.NotifyAll(new MessageSubType1());
            Console.WriteLine();
            theObservable.NotifyAll(new MessageSubType2());
            Console.WriteLine();
            theObservable.NotifyAll(new MessageSubType23());
        }
    }
}
