using IOIOLib.MessageFrom;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfUI
{
    internal class MessageObserver : IObserverIOIO, IObserver<IMessageFromIOIO>
    {
        private TextBox MessageLog_;

        public MessageObserver(TextBox messageLog)
        {
            this.MessageLog_ = messageLog;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(IMessageFromIOIO value)
        {
            //MessageBox.Show(string.Format("IMessageFromIOIO {0}", value.ToString()));
            this.MessageLog_.Dispatcher.BeginInvoke((Action)(() => 
                this.MessageLog_.AppendText( value.ToString() + "\n")
                ));
        }
    }
}