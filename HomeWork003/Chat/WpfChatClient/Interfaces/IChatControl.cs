using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace WpfChatClient.Interfaces
{
    public interface IChatControl
    {
        event Func<byte[], Task> UserTryingToSendMessage;
        void PushMessage(string userName, byte[] arr);
        object Tag { get; set; }
    }
}