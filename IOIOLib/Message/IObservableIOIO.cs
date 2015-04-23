using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Message
{
    /// <summary>
    /// This is public because public classes implement it.
    /// I have no idea why you can't have a public class implement an internal interface.  
    /// Makes no sense no matter how much  hand waving people do.
    /// </summary>
    public interface IObservableIOIO
    {
        /// <summary>
        /// the one method subclass ned to implement
        /// This is called <b>before</b> an outbound message is sent
        /// It is called <b>after</b> an inbound message is received
        /// </summary>
        /// <param name="message"></param>
        void HandleMessage(IMessageIOIO message);

        /// <summary>
        /// Adds an observer to the the observable
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        IDisposable Subscribe(IObserverIOIO observer);
    }
}
