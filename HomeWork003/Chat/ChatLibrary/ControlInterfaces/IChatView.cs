using System;

namespace ChatLibrary.ControlInterfaces
{
    public interface IChatView
    {
        void RefreshMainChat(string message);
        void RefreshPersonalChat(string message);

        event Action<string> IncomingMainChatMessage;
        event Action<string> IncomingPersonalChatMessage;
    }
}