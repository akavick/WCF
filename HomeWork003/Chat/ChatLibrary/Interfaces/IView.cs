using System;

namespace ChatLibrary.Interfaces
{
    public interface IView
    {
        void RefreshMainChat(string message);
        event Action<string> IncomingMessage;
    }
}