using System;

namespace ChatLibrary
{
    public class MyChatClient : IChatClient
    {
        public void Refresh(string message)
        {
            Console.WriteLine(message);
        }
    }
}
