using System;
using System.Windows;
using System.Windows.Documents;

namespace WpfChatClient.Interfaces
{
    public interface IChatControl
    {
        event Action<byte[]> UserTryingToSendMessage;
        void PushMessage(string userName, FlowDocument doc);
        void PushMessage(string userName, byte[] arr);
        object Tag { get; set; }
    }
}