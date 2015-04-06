using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

///
/// This implementation is described at http://joe.blog.freemansoft.com/2015/04/message-routing-using-double-dispatch.html
/// Portions of this code were derived from http://stackoverflow.com/questions/1477471/design-pattern-for-handling-multiple-message-types
/// 
namespace IOIOLibDotNetTest.MessageFrom
{
    /// <summary>
    /// non genericized marker interface used to provide first dispatch
    /// </summary>
    public interface IObserver { }
    /// <summary>
    /// non genericized dispatch implemented by all message classes
    /// </summary>
    public interface IMessageNotification {
        void Notify(IObserver observer );
    }

    public interface IMessageBase { }
    /// <summary>
    /// A message type IMessageTypeSub1 --> IMessageBaseType
    /// </summary>
    public interface IMessageType1 : IMessageBase { }
    /// <summary>
    /// A message type IMessageTypeSub2 --> IMessageBaseType
    /// </summary>
    public interface IMessageType2 : IMessageBase { }
    /// <summary>
    /// A message type IMessageTypeSub23 --> IMessageTypeSub2 --> IMessageBaseType
    /// </summary>
    public interface IMessageType2SubMessage : IMessageType2 { }


    /// <summary>
    /// Base class for all message implements  primary and secondary dispatch
    /// </summary>
    /// <typeparam name="MessageType"></typeparam>
    public class MessageBase<MessageType> : IMessageNotification where MessageType : class
    {
        /// <summary>
        /// Dispatch method on the message itself that notifies the passed in observer via the most targeted method
        /// </summary>
        /// <param name="observer">non genericized observer</param>
        public void Notify(IObserver observer)
        {
            MessageType msg_as_msg_type = this as MessageType;
            if (msg_as_msg_type != null)
            {
                NotifyDynamic(observer, msg_as_msg_type);
            }
        }

        /// <summary>
        /// Notifies an observer based on the MessageType
        /// </summary>
        /// <param name="observer">IMessageHandler that is also IObserver&lt;MessageType&gt</param>
        /// <param name="self">this message as a cast MessageType</param>
        protected void NotifyDynamic(IObserver observer, MessageType self)
        {
            IObserver<MessageType> genericizedObserver = observer as IObserver<MessageType>;
            if (genericizedObserver != null)
            {
                genericizedObserver.OnNext(self);
            }
        }
    }

    /// <summary>
    /// simple message that is of type IMessageSubType1
    /// </summary>
    public class MessageType1 : MessageBase<IMessageType1>, IMessageType1
    {
    }
    /// <summary>
    /// simple message that is of type IMessageSubType2
    /// </summary>
    public class MessageType2 : MessageBase<IMessageType2>, IMessageType2
    {
    }
    /// <summary>
    /// simple message that is of type IMessageSubType23 subclass of 2
    /// </summary>
    public class MessageType2SubMessage : MessageBase<IMessageType2SubMessage>, IMessageType2SubMessage
    {
    }


    /// <summary>
    /// All messages should end up processed by this class.
    /// Simple message observer
    /// </summary>
    public class ObserverOfBase : IObserver<IMessageBase>, IObserver
    {
        internal int NotifiedBaseCount_;

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IMessageBase value)
        {
            NotifiedBaseCount_++;
            Console.WriteLine("OnNext:"
                + "\n MessageType: " + value.GetType()
                + "\n ProcessedAs: IMessageBase"
                + "\n ByObserver: " + this.GetType().ToString() 
                );
        }
    }

    /// <summary>
    /// Only one type message should end up processed by this class.
    /// Simple message observer
    /// </summary>
    public class ObserverOfSubType1 : IObserver<IMessageType1>, IObserver
    {
        internal int NotifiedType1Count_;

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IMessageType1 value)
        {
            NotifiedType1Count_++;
            Console.WriteLine("OnNext:"
                + "\n MessageType: " + value.GetType()
                + "\n ProcessedAs: IMessageType1"
                + "\n ByObserver: " + this.GetType().ToString()
                );
        }

    }

    /// <summary>
    /// type 1 and type 2 will be processed by this class
    /// Simple message observer 
    /// </summary>
    public class ObserverOf1And2 : IObserver<IMessageType1>, IObserver<IMessageType2>, IObserver
    {
        internal int NotifiedType1Count_;
        internal int NotifiedType2Count_;

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IMessageType1 value)
        {
            NotifiedType1Count_++;
            Console.WriteLine("OnNext:"
                + "\n MessageType: " + value.GetType()
                + "\n ProcessedAs: IMessageType1"
                + "\n ByObserver: " + this.GetType().ToString()
                );
        }
        public void OnNext(IMessageType2 value)
        {
            NotifiedType2Count_++;
            Console.WriteLine("OnNext:"
                + "\n MessageType: " + value.GetType()
                + "\n ProcessedAs: IMessageType2"
                + "\n ByObserver: " + this.GetType().ToString()
                );
        }
    }


    /// <summary>
    /// The implmeentation that forwards passes each registered observer to the message for notification
    /// </summary>
    public class MessageDistributor
    {
        List<IObserver> RegisteredObservers = new List<IObserver>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer">Should also be IObserver for some message types</param>
        /// <returns>nothing right now. Should be used to unsubscribe</returns>
        public IDisposable Subscribe(IObserver observer)
        {
            RegisteredObservers.Add(observer);
            return null;
        }

        public void NotifyAll(IMessageNotification target)
        {
            foreach (IObserver observer in RegisteredObservers)
            {
                if (observer != null)
                {
                    target.Notify(observer);
                }
                else
                {
                    Console.WriteLine("You didn't define this thing as an IMessageHandler: " + observer);
                }

            }
        }
    }

    [TestClass]
    public class ValidateMessageDispatchApproach
    {
        [TestMethod]
        public void ValidateMessageDispatchApproach_TestMultiple()
        {
            ObserverOfBase anObserverBase = new ObserverOfBase();
            // create 3 observers
            ObserverOfSubType1 anObservableSubType1 = new ObserverOfSubType1();
            ObserverOf1And2 anObservableSubTypeSeveral = new ObserverOf1And2();
            MessageDistributor theObservable = new MessageDistributor();
            // and register them
            theObservable.Subscribe(anObserverBase);
            theObservable.Subscribe(anObservableSubType1);
            theObservable.Subscribe(anObservableSubTypeSeveral);

            // send three different message types
            // base handler always gets the message
            theObservable.NotifyAll(new MessageType1());
            Assert.AreEqual(1, anObserverBase.NotifiedBaseCount_);
            Assert.AreEqual(1, anObservableSubType1.NotifiedType1Count_);
            Assert.AreEqual(1, anObservableSubTypeSeveral.NotifiedType1Count_);
            Assert.AreEqual(0, anObservableSubTypeSeveral.NotifiedType2Count_);

            Console.WriteLine();
            theObservable.NotifyAll(new MessageType2());
            Assert.AreEqual(2, anObserverBase.NotifiedBaseCount_);
            Assert.AreEqual(1, anObservableSubType1.NotifiedType1Count_);
            Assert.AreEqual(1, anObservableSubTypeSeveral.NotifiedType1Count_);
            Assert.AreEqual(1, anObservableSubTypeSeveral.NotifiedType2Count_);

            Console.WriteLine();
            theObservable.NotifyAll(new MessageType2SubMessage());
            Assert.AreEqual(3, anObserverBase.NotifiedBaseCount_);
            Assert.AreEqual(1, anObservableSubType1.NotifiedType1Count_);
            Assert.AreEqual(1, anObservableSubTypeSeveral.NotifiedType1Count_);
            Assert.AreEqual(2, anObservableSubTypeSeveral.NotifiedType2Count_);
        }
    }
}
