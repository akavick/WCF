using System;
using ChatLibrary;

namespace MyClient
{
    public class MyClient : IMyClient
    {
        public void Refresh(string message)
        {
            Console.WriteLine(message);
        }
    }
}
