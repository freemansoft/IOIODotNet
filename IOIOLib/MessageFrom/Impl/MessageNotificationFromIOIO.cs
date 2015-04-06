using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom.Impl
{
    public class MessageNotificationFromIOIO<MessageType> : IMessageNotificationFromIOIO where MessageType : class
    {
        /// <summary>
        /// Dispatch method on the message itself that notifies the passed in observer via the most targeted method
        /// </summary>
        /// <param name="observer">non genericized observer</param>
        public void Notify(IObserverIOIO observer)
        {
            MessageType messageAsMessageType = this as MessageType;
            if (messageAsMessageType != null)
            {
                NotifyDynamic(observer, messageAsMessageType);
            }
        }


        /// <summary>
        /// Notifies an observer based on the MessageType
        /// </summary>
        /// <param name="observer">IObserverIOIO that is also IObserver&lt;MessageType&gt</param>
        /// <param name="self">this message as a cast MessageType</param>
        protected void NotifyDynamic(IObserverIOIO observer, MessageType self)
        {
            IObserver<MessageType> genericizedObserver = observer as IObserver<MessageType>;
            if (genericizedObserver != null)
            {
                genericizedObserver.OnNext(self);
            }
        }
    }
}
