using System;

namespace ChatLibrary.Interfaces
{
    public interface IView
    {
        void RefreshMainChat(string message);
        void RefreshPersonalChat(string message);

        event Action<string> IncomingMainChatMessage;
        event Action<string> IncomingPersonalChatMessage;
    }
}