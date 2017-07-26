using System;
using System.Windows;

namespace WpfChatClient.Interfaces
{
    public interface IChatControl
    {
        event Action<IChatMessage> SendClick;
    }
}