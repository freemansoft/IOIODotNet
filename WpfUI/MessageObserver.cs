using IOIOLib.Message;
using IOIOLib.MessageFrom;
using IOIOLib.MessageTo;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfUI
{
    internal class MessageObserver : IObserverIOIO, IObserver<IMessageFromIOIO>, IObserver<IPostMessageCommand>
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

        public void OnNext(IPostMessageCommand value)
        {
            this.MessageLog_.Dispatcher.BeginInvoke((Action)(() =>
                this.MessageLog_.AppendText(value.ToString() + "\n")
                ));
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