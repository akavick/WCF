using System;
using Ex13;

namespace Ex13Client
{
    public class Ex13ClientCallback : IClientCallback
    {
        public void ReceiveTime(string str)
        {
            Console.WriteLine(str);
        }
    }
}
